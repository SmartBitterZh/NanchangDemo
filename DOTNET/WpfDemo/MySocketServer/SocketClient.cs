using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MySocketServer.SocketHelper;
using DBControl;

namespace MySocketServer
{
  public class SocketClient : IDisposable
  {
    /// <summary>
    /// 客户端连接Socket
    /// </summary>
    private Socket clientSocket;
    /// <summary>
    /// 连接状态
    /// </summary>
    private bool connected = false;
    /// <summary>
    /// 连接点
    /// </summary>
    private IPEndPoint hostEndPoint;
    /// <summary>
    /// 连接信号量
    /// </summary>
    private static AutoResetEvent autoConnectEvent = new AutoResetEvent(false);
    /// <summary>
    /// 接受到数据时的委托
    /// </summary>
    /// <param name="info"></param>
    public delegate void ReceiveMsgHandler(string info);
    /// <summary>
    /// 接收到数据时调用的事件
    /// </summary>
    public event ReceiveMsgHandler OnMsgReceived;
    /// <summary>
    /// 开始监听数据的委托
    /// </summary>
    public delegate void StartListenHandler();
    /// <summary>
    /// 开始监听数据的事件
    /// </summary>
    public event StartListenHandler StartListenThread;
    /// <summary>
    /// 发送信息完成的委托
    /// </summary>
    /// <param name="successorfalse"></param>
    public delegate void SendCompleted(bool successorfalse);
    /// <summary>
    /// 发送信息完成的事件
    /// </summary>
    public event SendCompleted OnSended;
    /// <summary>
    /// 监听接收的SocketAsyncEventArgs
    /// </summary>
    private DuplexSocketAsyncEventArgsWithId _readWirted;
    /// <summary>
    /// 
    /// </summary>
    private SocketBufferManager m_bufferManager;

    /// <summary>
    /// 初始化客户端
    /// </summary>
    /// <param name="hostName">服务端地址{IP地址}</param>
    /// <param name="port">端口号</param>
    public SocketClient(string hostName, int port)
    {
      IPHostEntry host = Dns.GetHostEntry(hostName);
      IPAddress[] addressList = host.AddressList;
      this.hostEndPoint = new IPEndPoint(addressList[addressList.Length - 1], port);
      this.clientSocket = new Socket(this.hostEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
      this.m_bufferManager = new SocketBufferManager(32768, 32768);

    }

    /// <summary>
    /// 连接服务端
    /// </summary>
    public bool Connect()
    {
      MySocketAsyncEventArgs connectArgs = new MySocketAsyncEventArgs("Connect");
      connectArgs.UserToken = new AsyncUserToken(clientSocket);
      connectArgs.RemoteEndPoint = this.hostEndPoint;
      connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnect);
      clientSocket.ConnectAsync(connectArgs);
      //等待连接结果
      autoConnectEvent.WaitOne();
      SocketError errorCode = connectArgs.SocketError;
      if (errorCode == SocketError.Success)
      {
        this.m_bufferManager.InitBuffer();
        _readWirted = new DuplexSocketAsyncEventArgsWithId();
        //_readWirted.State = true;
        _readWirted.ReceiveSAEA.Completed += OnReceive;
        _readWirted.SendSAEA.Completed += OnSend;

        _readWirted.ReceiveSAEA.UserToken = new AsyncUserToken(clientSocket);
        _readWirted.SendSAEA.UserToken = new AsyncUserToken(clientSocket);

        this.m_bufferManager.SetBuffer(_readWirted.ReceiveSAEA);
        Listen();

        if (StartListenThread != null)
          StartListenThread();
        return true;
      }
      else
        //throw new SocketException((Int32)errorCode);
        return false;
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
              OnMsgReceived(msg);

          bool _willRaiseEvent = (receiver.UserToken as AsyncUserToken).Socket.ReceiveAsync(receiver);
          if (!_willRaiseEvent)
            ProcessReceive(receiver);
        }
      }
    }

    /// <summary>
    /// 开始监听线程的入口函数
    /// </summary>
    public void Listen()
    {
      if (!(_readWirted.ReceiveSAEA.UserToken as AsyncUserToken).Socket.ReceiveAsync(_readWirted.ReceiveSAEA))
        ProcessReceive(_readWirted.ReceiveSAEA);
    }

    /// <summary>
    /// 发送信息
    /// </summary>
    /// <param name="message"></param>
    public void SendMessage(string message)
    {
      if (this.connected)
      {
        message = string.Format("[length={0}]{1}", message.Length, message);
        byte[] sendBuffer = Encoding.Unicode.GetBytes(message);
        MySocketAsyncEventArgs senderSocketAsyncEventArgs = _readWirted.SendSAEA;
        senderSocketAsyncEventArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
        senderSocketAsyncEventArgs.RemoteEndPoint = this.hostEndPoint;
        clientSocket.SendAsync(senderSocketAsyncEventArgs);
      }
      else
      {
        throw new SocketException((int)SocketError.NotConnected);
      }
    }

    /// <summary>
    /// 发送信息
    /// </summary>
    /// <param name="message"></param>
    public void SendRequest(BaseRequest request)
    {
      if (this.connected)
      {
        string message = "[JSON]" + SerializeHelper.JsonSerializer(request);
        message = string.Format("[length={0}]{1}", message.Length, message);
        byte[] sendBuffer = Encoding.Unicode.GetBytes(message);
        MySocketAsyncEventArgs senderSocketAsyncEventArgs = _readWirted.SendSAEA;
        senderSocketAsyncEventArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
        senderSocketAsyncEventArgs.RemoteEndPoint = this.hostEndPoint;
        clientSocket.SendAsync(senderSocketAsyncEventArgs);
      }
      else
      {
        throw new SocketException((int)SocketError.NotConnected);
      }
    }
    /// <summary>
    /// 断开连接
    /// </summary>
    public void Disconnect()
    {
      clientSocket.Disconnect(false);
    }

    /// <summary>
    /// 连接的完成方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnConnect(object sender, SocketAsyncEventArgs e)
    {
      autoConnectEvent.Set();
      this.connected = (e.SocketError == SocketError.Success);
    }
    /// <summary>
    /// 接收的完成方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnReceive(object sender, SocketAsyncEventArgs e)
    {
      string msg = Encoding.Unicode.GetString(e.Buffer, 0, e.BytesTransferred);
      Listen();
      OnMsgReceived(msg);
    }
    /// <summary>
    /// 发送的完成方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSend(object sender, SocketAsyncEventArgs e)
    {
      if (e.SocketError == SocketError.Success)
      {
        OnSended(true);
      }
      else
      {
        OnSended(false);
        this.ProcessError(e);
      }
    }
    /// <summary>
    /// 处理错误
    /// </summary>
    /// <param name="e"></param>
    private void ProcessError(SocketAsyncEventArgs e)
    {
      Socket s = (e.UserToken as AsyncUserToken).Socket;
      if (s.Connected)
      {
        try
        {
          s.Shutdown(SocketShutdown.Both);
        }
        catch (Exception)
        {
          //client already closed
        }
        finally
        {
          if (s.Connected)
          {
            s.Close();
          }
        }
      }
      throw new SocketException((Int32)e.SocketError);
    }

    #region IDisposable Members
    public void Dispose()
    {
      m_bufferManager.Dispose();
      m_bufferManager = null;
      autoConnectEvent.Close();
      if (this.clientSocket.Connected)
      {
        this.clientSocket.Close();
      }
    }
    #endregion
  }
}
