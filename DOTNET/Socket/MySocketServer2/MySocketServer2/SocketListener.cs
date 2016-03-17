using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MySocketServer2
{
  public class SocketListener : IDisposable
  {
    private static Dictionary<string, string> _IPID = new Dictionary<string, string>();

    private static Mutex m_mutex = new Mutex();
    /// <summary>
    /// current connections
    /// </summary>
    private int m_numConnections;
    /// <summary>
    /// 最大并发量
    /// </summary>
    private int m_numConcurrence;
    /// <summary>
    /// 读取写入字节
    /// </summary>
    private const int m_opsToPreAlloc = 1;
    private SocketBufferManager m_bufferManager;
    private Socket m_listenSocket;
    private SocketServerState m_serverState;
    private SocketAsyncEventArgsPool m_readWritePool;
    /// <summary>
    /// 并发量信息
    /// </summary>
    private Semaphore m_semaphoreAcceptedClients;
    /// <summary>
    /// SOCKET 的  ReceiveTimeout属性
    /// </summary>
    public int ReceiveTimeout
    {
      get
      {
        return m_listenSocket.ReceiveTimeout;
      }

      set
      {
        m_listenSocket.ReceiveTimeout = value;

      }
    }

    /// <summary>
    /// SOCKET 的 SendTimeout
    /// </summary>
    public int SendTimeout
    {
      get
      {
        return m_listenSocket.SendTimeout;
      }

      set
      {
        m_listenSocket.SendTimeout = value;
      }

    }
    /// <summary>
    /// 返回服务器状态
    /// </summary>
    public SocketServerState State
    {
      get
      {
        return m_serverState;
      }
    }
    /// <summary>
    /// 获取当前在线用户的UID
    /// </summary>
    public string[] OnlineUID
    {
      get { return m_readWritePool.OnlineUID; }
    }
    /// <summary>
    /// 获取当前的并发数
    /// </summary>
    public int NumConnections
    {
      get { return this.m_numConnections; }
    }
    /// <summary>
    /// 最大并发数
    /// </summary>
    public int MaxConcurrence
    {
      get { return this.m_numConcurrence; }
    }

    #region enent

    /// <summary>
    /// 接收信息是的事件委托
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="info"></param>
    public delegate void ReceiveMsgHandler(string uid, SocketRequestMessage info);
    public event ReceiveMsgHandler OnMsgReceived;

    /// <summary>
    /// 发送信息完成后的委托
    /// </summary>
    /// <param name="successorfalse"></param>
    public delegate void SendCompletedHandler(string uid, string exception);
    /// <summary>
    /// 发送信息完成后的事件
    /// </summary>
    public event SendCompletedHandler OnSended;


    public event EventHandler<LogOutEventArgs> MessageOut;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="ip"></param>
    public delegate void AcceptUserHandler(string uid, string ip);
    public event AcceptUserHandler AcceptUser;

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="numConcurrence"></param>
    /// <param name="receiveBufferSize"></param>
    public SocketListener(int numConcurrence, int receiveBufferSize)
    {
      this.m_serverState = SocketServerState.Initialing;
      this.m_numConnections = 0;
      this.m_numConcurrence = numConcurrence;
      this.m_bufferManager = new SocketBufferManager(receiveBufferSize * numConcurrence * m_opsToPreAlloc, receiveBufferSize);
      this.m_readWritePool = new SocketAsyncEventArgsPool(numConcurrence);
      this.m_semaphoreAcceptedClients = new Semaphore(numConcurrence, numConcurrence);
    }

    /// <summary>
    /// 输出消息
    /// </summary>
    /// <param name="o"></param>
    /// <param name="type"></param>
    /// <param name="message"></param>
    protected void LogOutEvent(Object sender, SocketMessageType type, string message)
    {
      if (MessageOut != null)
        MessageOut.BeginInvoke(sender, new LogOutEventArgs(type, message), new AsyncCallback(CallBackEvent), MessageOut);
    }
    /// <summary>
    /// 事件处理完的回调函数
    /// </summary>
    /// <param name="ar"></param>
    private void CallBackEvent(IAsyncResult ar)
    {
      EventHandler<LogOutEventArgs> MessageOut = ar.AsyncState as EventHandler<LogOutEventArgs>;
      if (MessageOut != null)
        MessageOut.EndInvoke(ar);
    }

    public void Init()
    {
      this.m_bufferManager.InitBuffer();
      SocketAsyncEventArgsWithIdDuplex _readWirted;
      for (int i = 0; i < this.m_numConcurrence; i++)
      {
        _readWirted = new SocketAsyncEventArgsWithIdDuplex();
        _readWirted.ReceiveSAEA.Completed += OnCompleted;
        _readWirted.SendSAEA.Completed += OnCompleted;
        this.m_bufferManager.SetBuffer(_readWirted.ReceiveSAEA);
        this.m_readWritePool.Push(_readWirted);
      }
      m_serverState = SocketServerState.Inited;
    }

    public void Start(int port, string host = "")
    {
      IPAddress[] _addressList = Dns.GetHostEntry(Environment.MachineName).AddressList;
      IPEndPoint _localEndPoint = (string.IsNullOrEmpty(host)) ?
                                  (new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], port)) :
                                  (new IPEndPoint(IPAddress.Parse(host), port));
      this.m_listenSocket = new Socket(_localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
      if (_localEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
      {
        this.m_listenSocket.SetSocketOption(SocketOptionLevel.IPv6, (SocketOptionName)27, false);
        this.m_listenSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, _localEndPoint.Port));
      }
      else
        this.m_listenSocket.Bind(_localEndPoint);
      this.m_listenSocket.Listen(100);
      SendTimeout = 1000;
      ReceiveTimeout = 1000;

      this.StartAccept(null);

      m_serverState = SocketServerState.Running;
      m_mutex.WaitOne();
    }


    /// <summary>
    /// 发送数据包
    /// </summary>
    /// <param name="sock"></param>
    /// <param name="data"></param>
    public void SendData(Socket sock, byte[] data)
    {
      sock.BeginSend(data, 0, data.Length, SocketFlags.None, SendDataAsynCallBack, sock);
    }

    void SendDataAsynCallBack(IAsyncResult result)
    {
      try
      {
        Socket sock = result.AsyncState as Socket;
        if (sock != null)
        {
          sock.EndSend(result);
        }
      }
      catch
      {

      }
    }
    private void StartAccept(SocketAsyncEventArgs accepter)
    {
      if (accepter == null)
      {
        accepter = new SocketAsyncEventArgs();
        accepter.Completed += new EventHandler<SocketAsyncEventArgs>(OnCompleted);
      }
      else
        accepter.AcceptSocket = null;
      this.m_semaphoreAcceptedClients.WaitOne();
      if (!this.m_listenSocket.AcceptAsync(accepter))
        this.ProcessAccept(accepter);
    }

    public string GetUid(SocketAsyncEventArgs e)
    {
      string _uid;
      if (e == null)
        return string.Empty;

      string _ip = (e.AcceptSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
      if (_IPID.ContainsKey(_ip))
        _uid = _IPID[_ip];
      else
      {
        _uid = Guid.NewGuid().ToString();
        if (e is SocketAsyncEventArgsImpl)
          _uid = (e as SocketAsyncEventArgsImpl).UID;
        _IPID.Add(_ip, _uid);
      }
      return _uid;
    }
    private void ProcessAccept(SocketAsyncEventArgs accepter)
    {
      if (accepter.LastOperation != SocketAsyncOperation.Accept)
        return;

      try
      {
        if (accepter.SocketError == SocketError.Success)
        {
          string _uid = GetUid(accepter);
          if (m_readWritePool.BusyPoolContains(_uid))
          {
            LogOutEvent(null, SocketMessageType.Error, string.Format("The client[{0}] is alreadly in connnect", accepter.AcceptSocket.RemoteEndPoint));
            this.CloseClientSocket(_uid, false);
            return;
          }

          SocketAsyncEventArgsWithIdDuplex _reader = this.m_readWritePool.Pop(_uid);
          _reader.ReceiveSAEA.UserToken = accepter.AcceptSocket;
          _reader.SendSAEA.UserToken = accepter.AcceptSocket;

          if (AcceptUser != null)
            AcceptUser(_uid, (accepter.AcceptSocket.RemoteEndPoint as IPEndPoint).Address.ToString());

          if (!(_reader.ReceiveSAEA.UserToken as Socket).ReceiveAsync(_reader.ReceiveSAEA))
            ProcessReceive(_reader.ReceiveSAEA);
        }

      }
      finally
      {
        Interlocked.Increment(ref this.m_numConnections);
        this.StartAccept(accepter);
      }
    }
    private void ProcessSend(SocketAsyncEventArgs sender)
    {
      if (sender.LastOperation != SocketAsyncOperation.Send)
        return;
      string _uid = (sender as SocketAsyncEventArgsImpl).UID;
      if (sender.BytesTransferred > 0)
      {
        if (sender.SocketError == SocketError.Success)
          OnSended(_uid, SocketMessageType.Success.ToString());
        else
          OnSended(_uid, SocketMessageType.Failed.ToString());
      }
      else
        this.CloseClientSocket(_uid, false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="msg"></param>
    /// <param name="type"></param>
    public void SendMessageAsync(string uid, string msg, bool allUser = false, SocketMessageInfoType type = SocketMessageInfoType.NONE)
    {
      if ((!allUser && string.IsNullOrEmpty(uid)) || string.IsNullOrEmpty(msg))
        return;
      if (allUser)
      {
        if (this.m_readWritePool.m_busypool.Count == 0)
          LogOutEvent(null, SocketMessageType.Error, string.Format("No client online"));

        foreach (string id in this.m_readWritePool.m_busypool.Keys)
          SendMessageAsync(id, msg, false, type);
      }
      else
      {
        SocketAsyncEventArgsWithIdDuplex _socket = m_readWritePool.FindByUID(uid);
        if (_socket == null)
          OnSended(uid, SocketMessageType.UseOffline.ToString());
        else
        {
          string _msgTmpl = @"[length={0}][MSG={1}]{2}";
          SocketAsyncEventArgsImpl _e = _socket.SendSAEA;
          if (_e.SocketError == SocketError.Success)
          {
            int _i = 0;
            try
            {
              string _msg = string.Format(_msgTmpl, msg.Length, type.ToString(), msg);
              byte[] _sendBuffer = Encoding.Unicode.GetBytes(_msg);
              _e.SetBuffer(_sendBuffer, 0, _sendBuffer.Length);
              bool _willRaiseEvent = (_e.UserToken as Socket).SendAsync(_e);
              if (!_willRaiseEvent)
                this.ProcessSend(_e);
            }
            catch (Exception ee)
            {
              if (_i <= 5)
              {
                _i++;
                Thread.Sleep(10);
                SendMessageAsync(uid, msg, allUser, type);
              }
              else
              {
                OnSended(uid, ee.ToString());
              }
            }
          }
          else
          {
            OnSended(uid, SocketMessageType.Failed.ToString());
            this.CloseClientSocket(_e.UID, false);
          }
        }
      }
    }

    private void ProcessReceive(SocketAsyncEventArgs receiver)
    {
      if (receiver.LastOperation != SocketAsyncOperation.Receive)
        return;
      if (receiver.BytesTransferred > 0)
      {
        if (receiver.SocketError == SocketError.Success)
        {
          int _byteTransferred = receiver.BytesTransferred;
          string _received = Encoding.Unicode.GetString(receiver.Buffer, receiver.Offset, _byteTransferred);

          SocketRequestMessage _msg = SocketRequestHandler.GetActualString(_received);

          if (this.OnMsgReceived != null)
            OnMsgReceived((receiver as SocketAsyncEventArgsImpl).UID, _msg);

          bool _willRaiseEvent = (receiver.UserToken as Socket).ReceiveAsync(receiver);
          if (!_willRaiseEvent)
            ProcessReceive(receiver);
        }
      }
      else
        this.CloseClientSocket((receiver as SocketAsyncEventArgsImpl).UID, true);
    }
    void OnCompleted(object sender, SocketAsyncEventArgs e)
    {
      switch (e.LastOperation)
      {
        case SocketAsyncOperation.Accept:
          this.ProcessAccept(e);
          break;
        case SocketAsyncOperation.Receive:
          this.ProcessReceive(e);
          break;
        case SocketAsyncOperation.Send:
          this.ProcessSend(e);
          break;
      }
    }
    private void CloseClientSocket(string uid, bool isReceive = false)
    {
      if (string.IsNullOrEmpty(uid))
        return;
      SocketAsyncEventArgsWithIdDuplex _saea = m_readWritePool.FindByUID(uid);
      if (_saea != null)
      {
        m_bufferManager.FreeBuffer(_saea.ReceiveSAEA);
        Socket _socket = (_saea.ReceiveSAEA.UserToken as Socket);
        try
        {
          _socket.Shutdown(SocketShutdown.Both);
          _socket = null;
        }
        catch
        {
          LogOutEvent(null, SocketMessageType.Error, "客户端已经关闭");
        }
        this.m_semaphoreAcceptedClients.Release();
        Interlocked.Decrement(ref this.m_numConnections);
        this.m_readWritePool.Push(_saea);
        if (this.m_readWritePool.Count == 1)
          StartAccept(null);
      }
    }

    /// <summary>
    /// 断开此SOCKET
    /// </summary>
    /// <param name="sock"></param>
    public void Disconnect(Socket sock)
    {
      sock.BeginDisconnect(false, AsynCallBackDisconnect, sock);
    }

    void AsynCallBackDisconnect(IAsyncResult result)
    {
      Socket sock = result.AsyncState as Socket;
      if (sock != null)
      {
        sock.EndDisconnect(result);
      }
    }
    public void Dispose()
    {
      m_bufferManager.Dispose();
      m_bufferManager = null;
      m_readWritePool.Dispose();
      m_readWritePool = null;
      try
      {
        m_listenSocket.Shutdown(SocketShutdown.Both);
        m_listenSocket.Close();
      }
      catch
      {
      }
    }
  }


  public class LogOutEventArgs : EventArgs
  {

    /// <summary>
    /// 消息类型
    /// </summary>     
    private SocketMessageType messClass;
    /// <summary>
    /// 消息类型
    /// </summary>  
    public SocketMessageType MessClass
    {
      get { return messClass; }
    }
    /// <summary>
    /// 消息
    /// </summary>
    private string mess;
    public string Mess
    {
      get { return mess; }
    }
    public LogOutEventArgs(SocketMessageType messclass, string str)
    {
      messClass = messclass;
      mess = str;

    }


  }
}
