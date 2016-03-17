using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MySocketServer2
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
    public delegate void ReceiveMsgHandler(SocketRequestMessage info);
    /// <summary>
    /// 接收到数据时调用的事件
    /// </summary>
    public event ReceiveMsgHandler OnMsgReceived;

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
    private SocketAsyncEventArgsWithIdDuplex _readWirted;
    /// <summary>
    /// 
    /// </summary>
    private SocketBufferManager m_bufferManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hostName"></param>
    /// <param name="port"></param>
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
      SocketAsyncEventArgsImpl connectArgs = new SocketAsyncEventArgsImpl(SocketAsyncType.Connect);
      connectArgs.UserToken = clientSocket;
      connectArgs.RemoteEndPoint = this.hostEndPoint;
      connectArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnConnect);
      clientSocket.ConnectAsync(connectArgs);
      //等待连接结果
      autoConnectEvent.WaitOne();
      SocketError errorCode = connectArgs.SocketError;
      if (errorCode == SocketError.Success)
      {
        this.m_bufferManager.InitBuffer();
        _readWirted = new SocketAsyncEventArgsWithIdDuplex();

        _readWirted.ReceiveSAEA.Completed += OnReceive;
        _readWirted.SendSAEA.Completed += OnSend;

        _readWirted.ReceiveSAEA.UserToken = clientSocket;
        _readWirted.SendSAEA.UserToken = clientSocket;

        this.m_bufferManager.SetBuffer(_readWirted.ReceiveSAEA);
        ListenAsync();

        return true;
      }
      else
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnConnect(object sender, SocketAsyncEventArgs e)
    {
      autoConnectEvent.Set();
      this.connected = (e.SocketError == SocketError.Success);
    }

    /// <summary>
    /// 开始监听线程的入口函数
    /// </summary>
    public void ListenAsync(Socket socket = null)
    {
      if (socket == null)
        socket = (_readWirted.ReceiveSAEA.UserToken as Socket);
      if (!socket.ReceiveAsync(_readWirted.ReceiveSAEA))
        ProcessReceive(_readWirted.ReceiveSAEA);
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

          SocketRequestMessage _msgs = SocketRequestHandler.GetActualString(_received);

          if (this.OnMsgReceived != null)
            OnMsgReceived(_msgs);

          bool _willRaiseEvent = (receiver.UserToken as Socket).ReceiveAsync(receiver);
          if (!_willRaiseEvent)
            ProcessReceive(receiver);
        }
      }
    }

    /// <summary>
    /// 开始监听线程的入口函数
    /// </summary>
    public string Listen(Socket socket = null)
    {
      if (socket == null)
        socket = (_readWirted.ReceiveSAEA.UserToken as Socket);

      byte[] _bytes = new byte[1024];
      StringBuilder _sb = new StringBuilder();

      int flag = socket.Receive(_bytes, 0, _bytes.Length, SocketFlags.None);
      string _msg = Encoding.Unicode.GetString(_bytes);
      _sb.Append(_msg);
      SocketRequestMessage _rquestMsg = SocketRequestHandler.GetActualString(_msg);
      //如果没有接收到定长的数据，循环接收  
      while (flag != _rquestMsg.MessageLength)
      {
        flag += socket.Receive(_bytes, flag, _bytes.Length - flag, SocketFlags.None);
        _sb.Append(Encoding.Unicode.GetString(_bytes));
      }
      return _sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnReceive(object sender, SocketAsyncEventArgs e)
    {
      string msg = Encoding.Unicode.GetString(e.Buffer, 0, e.BytesTransferred);
      SocketRequestMessage _msgs = SocketRequestHandler.GetActualString(msg);
      ListenAsync();
      if (OnMsgReceived != null)
        OnMsgReceived(_msgs);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSend(object sender, SocketAsyncEventArgs e)
    {
      if (OnSended == null)
        return;
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
      Socket s = (e.UserToken as Socket);
      if (s.Connected)
      {
        try
        {
          s.Shutdown(SocketShutdown.Both);
        }
        catch (Exception)
        {
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

    /// <summary>
    /// 发送数据包
    /// </summary>
    /// <param name="sock"></param>
    /// <param name="data"></param>
    public void SendData(byte[] data)
    {
      if (this.connected)
      {
        SocketAsyncEventArgsImpl senderSocketAsyncEventArgs = _readWirted.SendSAEA;
        senderSocketAsyncEventArgs.SetBuffer(data, 0, data.Length);
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
    public void SendMessageAsync(string message)
    {
      message = string.Format("[length={0}][MSG={1}]{2}", message.Length, SocketMessageInfoType.NONE.ToString(), message);
      byte[] sendBuffer = Encoding.Unicode.GetBytes(message);
      SendData(sendBuffer);
    }
    /// <summary>
    /// 发送信息
    /// </summary>
    /// <param name="message"></param>
    public bool SendMessage(string message)
    {
      if (this.connected)
      {
        message = string.Format("[length={0}][MSG={1}]{2}", message.Length, SocketMessageInfoType.NONE.ToString(), message);
        byte[] sendBuffer = Encoding.Unicode.GetBytes(message);
        return clientSocket.Send(sendBuffer) >= 0;
      }
      else
      {
        throw new SocketException((int)SocketError.NotConnected);
      }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool SendRequest(SocketBaseRequest request, SocketMessageInfoType type = SocketMessageInfoType.REQUEST)
    {
      if (this.connected)
      {
        string _str = SerializeHelper.JsonSerializer(request);
        string message = string.Format("[length={0}][MSG={1}]{2}", _str.Length, type.ToString(), _str);
        byte[] sendBuffer = Encoding.Unicode.GetBytes(message);
        return clientSocket.Send(sendBuffer) >= 0;
      }
      else
      {
        throw new SocketException((int)SocketError.NotConnected);
      }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public void SendRequestAsync(SocketBaseRequest request, SocketMessageInfoType type = SocketMessageInfoType.REQUEST)
    {
      if (this.connected)
      {
        string _str = SerializeHelper.JsonSerializer(request);
        string message = string.Format("[length={0}][MSG={1}]{2}", _str.Length, type.ToString(), _str);
        byte[] sendBuffer = Encoding.Unicode.GetBytes(message);
        SendData(sendBuffer);
      }
      else
      {
        throw new SocketException((int)SocketError.NotConnected);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public SocketBaseResponse<T> RequestAsync<T>(SocketBaseRequest request, AsyncCallback callback)
    {
      if (this.connected)
      {

        _readWirted.ReceiveSAEA.Completed -= OnReceive;

        SocketSendLib<T> _sender = new SocketSendLib<T>();
        IAsyncResult _sendRequest = _sender.BeginSend(request, this, callback, _sender);
        string resultStr = string.Empty;
        SocketBaseResponse<T> _result = _sender.EndSend(ref resultStr, _sendRequest);

        _readWirted.ReceiveSAEA.Completed += OnReceive;
        return _result;
      }
      else
      {
        throw new SocketException((int)SocketError.NotConnected);
      }
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }
  }
}
