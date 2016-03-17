using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MySocketServer.SocketHelper;

namespace MySocketServer
{
  /// <summary>
  /// 对外部真正有意义的类，这个类监听用户的连接请求并从连接池取出一个可用连接给用户，并且时刻监听用户发来的数据并处理。
  /// 在设计这个类时为了迎合双工通信我把监听的任务放到另一个线程中去，
  /// 这也是我为什么要给每个用户两个SocketAsyncEventArgs的原因。
  /// 当然两个线程是不够的还要异步。比较重要的语句我都用粗体标注了。
  /// socket的方法都是成对出现的，
  /// ReceiveAsync对应OnReceiveCompleted，SendAsync对应OnSendCompleted，
  /// 所以理解起来也不算太难，只是要注意一点：就是接收和发送消息是在两个线程里的。
  /// </summary>
  public class SocketListener : IDisposable
  {
    private SocketListener _listener;
    public virtual SocketListener GetInstence()
    {
      _listener = new SocketListener(80000, 4096, null);
      _listener.Init();
      return _listener;
    }

    static Dictionary<string, string> _ipID = new Dictionary<string, string>();
    #region properties
    /// <summary>
    /// 
    /// </summary>
    private SocketBufferManager m_bufferManager;
    /// <summary>
    /// socket server
    /// </summary>
    private Socket m_listenSocket;
    /// <summary>
    /// lock 同步
    /// </summary>
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
    /// 
    /// </summary>
    private ServerState m_serverState;
    /// <summary>
    /// 读取写入字节
    /// </summary>
    private const int m_opsToPreAlloc = 1;
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
    /// <summary>
    /// 返回服务器状态
    /// </summary>
    public ServerState State
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
    #endregion

    #region enent
    /// <summary>
    /// 回调委托
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    public delegate string GetIDByIPHandler(string ip);
    private GetIDByIPHandler GetIDByIP;

    /// <summary>
    /// 接收信息是的事件委托
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="info"></param>
    public delegate void ReceiveMsgHandler(string uid, string info);
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="ip"></param>
    public delegate void AcceptUserHandler(string uid, string ip);
    public event AcceptUserHandler AcceptUser;

    public event EventHandler<LogOutEventArgs> MessageOut;

    #endregion

    /// <summary>
    /// 初始化服务器端
    /// </summary>
    /// <param name="numConcurrence">并发的连接数量(1000以上)</param>
    /// <param name="receiveBufferSize">每一个收发缓冲区的大小(32768)</param>
    public SocketListener(int numConcurrence, int receiveBufferSize, GetIDByIPHandler GetIDByIP)
    {
      this.m_serverState = ServerState.Initialing;
      this.m_numConnections = 0;
      this.m_numConcurrence = numConcurrence;
      this.m_bufferManager = new SocketBufferManager(receiveBufferSize * numConcurrence * m_opsToPreAlloc, receiveBufferSize);
      this.m_readWritePool = new SocketAsyncEventArgsPool(numConcurrence);
      this.m_semaphoreAcceptedClients = new Semaphore(numConcurrence, numConcurrence);
      if (GetIDByIP != null)
        this.GetIDByIP = GetIDByIP;
      else
        this.GetIDByIP = (ip) => { return _ipID[ip]; };
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
      DuplexSocketAsyncEventArgsWithId _readWirted;
      for (int i = 0; i < this.m_numConcurrence; i++)
      {
        _readWirted = new DuplexSocketAsyncEventArgsWithId();
        //_readWirted.State = true;
        _readWirted.ReceiveSAEA.Completed += OnCompleted;
        _readWirted.SendSAEA.Completed += OnCompleted;

        _readWirted.ReceiveSAEA.UserToken = new AsyncUserToken(null);
        _readWirted.SendSAEA.UserToken = new AsyncUserToken(null);

        //只给接收的SocketAsyncEventArgs设置缓冲区
        this.m_bufferManager.SetBuffer(_readWirted.ReceiveSAEA);
        this.m_readWritePool.Push(_readWirted);
      }
      m_serverState = ServerState.Inited;
    }
    /// <summary>
    /// start server
    /// </summary>
    /// <param name="port"></param>
    /// <param name="host"></param>
    public void Start(int port, string host = "")
    {
      IPAddress[] _addressList = Dns.GetHostEntry(Environment.MachineName).AddressList;
      IPEndPoint _localEndPoint = (string.IsNullOrEmpty(host)) ? (new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], port)) : (new IPEndPoint(IPAddress.Parse(host), port));
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
      //开始监听已连接用户的发送数据

      m_serverState = ServerState.Running;
      m_mutex.WaitOne();
    }
    /// <summary>
    /// send message
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="msg"></param>
    public void SendMessage(string msg, string uid = "", bool allUser = false)
    {
      if ((!allUser && string.IsNullOrEmpty(uid)) || string.IsNullOrEmpty(msg))
        return;

      if (allUser)
      {
        if (this.m_readWritePool.m_busypool.Count == 0)
          LogOutEvent(null, SocketMessageType.Error, string.Format("No client online"));

        foreach (string id in this.m_readWritePool.m_busypool.Keys)
          SendMessage(msg, id);
      }
      else
      {
        DuplexSocketAsyncEventArgsWithId _socket = m_readWritePool.FindByUID(uid);

        //说明用户已经断开
        //100   发送成功
        //200   发送失败
        //300   用户不在线
        //其它  表示异常的信息
        if (_socket == null)
          OnSended(uid, SocketMessageType.UseOffline.ToString());
        else
        {
          string _msgTmpl = @"[length={0}]{1}";
          MySocketAsyncEventArgs _e = _socket.SendSAEA;
          if (_e.SocketError == SocketError.Success)
          {
            int _i = 0;
            try
            {
              string _msg = string.Format(_msgTmpl, msg.Length, msg);
              byte[] _sendBuffer = Encoding.Unicode.GetBytes(_msg);
              _e.SetBuffer(_sendBuffer, 0, _sendBuffer.Length);
              bool _willRaiseEvent = (_e.UserToken as AsyncUserToken).Socket.SendAsync(_e);
              if (!_willRaiseEvent)
                this.ProcessSend(_e);
            }
            catch (Exception ee)
            {
              if (_i <= 5)
              {
                _i++;
                //如果发送出现异常就延迟0.01秒再发
                Thread.Sleep(10);
                SendMessage(msg, uid);
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


    public void Stop()
    {
      if (m_listenSocket != null)
      {
        m_listenSocket.Close();
        m_listenSocket = null;
        Dispose();
        m_mutex.ReleaseMutex();
        m_serverState = ServerState.Stoped;
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
      bool willRaiseEvent = this.m_listenSocket.AcceptAsync(accepter);
      if (!willRaiseEvent)
      {
        this.ProcessAccept(accepter);
      }
    }
    public string GetUid(SocketAsyncEventArgs e)
    {
      string _uid;
      if (e == null)
        return Guid.NewGuid().ToString();

      string _ip = (e.AcceptSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
      if (_ipID.ContainsKey(_ip))
        _uid = GetIDByIP(_ip);
      else
      {
        _uid = Guid.NewGuid().ToString();
        if (e is MySocketAsyncEventArgs)
          _uid = (e as MySocketAsyncEventArgs).UID;
        _ipID.Add(_ip, _uid);
      }
      return _uid;
    }
    private void ProcessAccept(SocketAsyncEventArgs accepter)
    {
      if (accepter.LastOperation != SocketAsyncOperation.Accept)
        return;
      //if (accepter.BytesTransferred <= 0)    //检查发送的长度是否大于0,不是就返回
      //  return;

      string _uid = GetUid(accepter);

      try
      {
        if (string.IsNullOrEmpty(_uid))
          return;
        if (m_readWritePool.BusyPoolContains(_uid))
        {
          LogOutEvent(null, SocketMessageType.Error, string.Format("The Socket Not Connect {0}", accepter.AcceptSocket.RemoteEndPoint));
          this.CloseClientSocket(_uid, false);
          return;
        }

        DuplexSocketAsyncEventArgsWithId _reader = this.m_readWritePool.Pop(_uid);
        _reader.ReceiveSAEA.UserToken = new AsyncUserToken(accepter.AcceptSocket);
        _reader.SendSAEA.UserToken = new AsyncUserToken(accepter.AcceptSocket);

        if (AcceptUser != null)
          AcceptUser(_uid, (accepter.AcceptSocket.RemoteEndPoint as IPEndPoint).Address.ToString());

        //if (m_bufferManager.SetBuffer(_reader.ReceiveSAEA))
        if (!(_reader.ReceiveSAEA.UserToken as AsyncUserToken).Socket.ReceiveAsync(_reader.ReceiveSAEA))
          ProcessReceive(_reader.ReceiveSAEA);

      }
      catch (Exception ee)
      {
        LogOutEvent(null, SocketMessageType.Error, ee.Message);
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
      string _uid = (sender as MySocketAsyncEventArgs).UID;
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

          string[] _msgs = RequestHandler.GetActualString(_received);

          if (this.OnMsgReceived != null)
            foreach (string msg in _msgs)
              OnMsgReceived((receiver as MySocketAsyncEventArgs).UID, msg);

          bool _willRaiseEvent = (receiver.UserToken as AsyncUserToken).Socket.ReceiveAsync(receiver);
          if (!_willRaiseEvent)
            ProcessReceive(receiver);
        }
      }
      else
        this.CloseClientSocket((receiver as MySocketAsyncEventArgs).UID, true);
    }

    private void CloseClientSocket(string uid, bool isReceive = false)
    {
      if (string.IsNullOrEmpty(uid))
        return;
      DuplexSocketAsyncEventArgsWithId _saea = m_readWritePool.FindByUID(uid);
      if (_saea != null)
      {
        m_bufferManager.FreeBuffer(_saea.ReceiveSAEA);
        Socket _socket = (_saea.ReceiveSAEA.UserToken as AsyncUserToken).Socket;
        try
        {
          _socket.Shutdown(SocketShutdown.Both);
          _socket = null;
        }
        catch
        {
          LogOutEvent(null, SocketMessageType.Error, "客户端已经关闭");
          //OnSended(uid, "客户端已经关闭");
        }
        this.m_semaphoreAcceptedClients.Release();
        Interlocked.Decrement(ref this.m_numConnections);
        this.m_readWritePool.Push(_saea);
        if (this.m_readWritePool.Count == 1)
          StartAccept(null);
      }
    }

    void OnCompleted(object sender, SocketAsyncEventArgs e)
    {
      switch (e.LastOperation)
      {
        case SocketAsyncOperation.Accept:
          {
            this.ProcessAccept(e);
          }
          break;
        case SocketAsyncOperation.Receive:
          ProcessReceive(e);
          break;
        case SocketAsyncOperation.Send:
          ProcessSend(e);
          break;

      }
    }

    public void Dispose()
    {
      m_bufferManager.Dispose();
      m_bufferManager = null;
      m_readWritePool.Dispose();
      m_readWritePool = null;
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
