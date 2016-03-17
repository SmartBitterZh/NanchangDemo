using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;

namespace MySocketServer
{
  public abstract class Session : ISession
  {
    public Context Context { get; private set; }
    public Socket Client { get; private set; }
    public IPEndPoint LocalEndPoint { get { return (IPEndPoint)Client.LocalEndPoint; } }
    public IPEndPoint RemoteEndPoint { get { return (IPEndPoint)Client.RemoteEndPoint; } }
    public void Initize(Socket socket)
    {
      this.Client = socket;
    }
    public void Send(MySocketAsyncEventArgs e, string uid, string msg)
    {
      int i = 0;
      try
      {
        string _msg = string.Format(msg, msg.Length, msg);
        byte[] _sendBuffer = Encoding.Unicode.GetBytes(_msg);
        e.SetBuffer(_sendBuffer, 0, _sendBuffer.Length);
        bool _willRaiseEvent = (e.UserToken as Socket).SendAsync(e);
        if (!_willRaiseEvent)
          this.ProcessSend(e);
      }
      catch (Exception ex)
      {
        if (i <= 5)
        {
          i++;
          //如果发送出现异常就延迟0.01秒再发
          Thread.Sleep(10);
          Send(e, uid, msg);
        }
        else
        {
          Send(e, uid, ex.ToString());
        }
      }
    }

    public void ProcessReceive(MySocketAsyncEventArgs e)
    {
      if (e.BytesTransferred <= 0 || e.SocketError != SocketError.Success)
      {
        OnClosed(e);
        return;
      }

      if (e.LastOperation != SocketAsyncOperation.Receive)
        return;
      if (e.BytesTransferred > 0)
      {
        if (e.SocketError == SocketError.Success)
        {
          int byteTransferred = e.BytesTransferred;
          string received = Encoding.Unicode.GetString(e.Buffer, e.Offset, byteTransferred);
          ////检查消息的准确性
          //string[] msg = RequestHandler.GetActualString(received);
          //foreach (string m in msg)
          //  OnMsgReceived(((MySocketAsyncEventArgs)e).UID, m);

          //可以在这里设一个停顿来实现间隔时间段监听，这里的停顿是单个用户间的监听间隔
          //发送一个异步接受请求，并获取请求是否为成功
          bool willRaiseEvent = (e.UserToken as Socket).ReceiveAsync(e);
          if (!willRaiseEvent)
            ProcessReceive(e);
        }
      }

    }

    public void ProcessSend(MySocketAsyncEventArgs e)
    {
      if (e.SocketError != SocketError.Success)
      {
        OnClosed(e);
        return;
      }

      AsyncUserToken token = (AsyncUserToken)e.UserToken;
      bool willRaiseEvent = token.Socket.ReceiveAsync(e);
      if (!willRaiseEvent)
        ProcessReceive(e);
    }

    public event EventHandler<MySocketAsyncEventArgs> Closed;

    public virtual void OnClosed(MySocketAsyncEventArgs e)
    {
      if (Closed != null)
        Closed(this, e);
    }
  }
}
