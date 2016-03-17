using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySocketServer2
{
  /// <summary>
  /// 
  /// </summary>
  public class SocketAsyncEventArgsWithIdDuplex : IDisposable
  {
    /// <summary>
    /// 用户标识，跟MySocketAsyncEventArgs的UID是一样的，
    /// 在对DuplexSocketAsyncEventArgsWithId的UID属性赋值的时候也对MySocketAsyncEventArgs的UID属性赋值。
    /// </summary>
    private string m_uid = "-1";
    private bool m_state = false;
    private SocketAsyncEventArgsImpl m_receivesaea;
    private SocketAsyncEventArgsImpl m_sendsaea;
    internal string UID
    {
      get { return m_uid; }
      set
      {
        m_uid = value;
        m_receivesaea.UID = value;
        m_sendsaea.UID = value;
      }
    }
    //constructor
    internal SocketAsyncEventArgsWithIdDuplex()
    {
      m_receivesaea = new SocketAsyncEventArgsImpl(SocketAsyncType.Receive);
      m_sendsaea = new SocketAsyncEventArgsImpl(SocketAsyncType.Send);
    }
    /// <summary>
    /// 接收消息
    /// </summary>
    public SocketAsyncEventArgsImpl ReceiveSAEA { get { return m_receivesaea; } set { m_receivesaea = value; } }
    /// <summary>
    /// 发送消息
    /// </summary>
    public SocketAsyncEventArgsImpl SendSAEA { get { return m_sendsaea; } set { m_sendsaea = value; } }
    /// <summary>
    /// 表示连接的可用与否，一旦连接被实例化放入连接池后State即变为True
    /// </summary>
    public bool State { get { return m_state; } set { m_state = value; } }

    public void Dispose()
    {
      if (m_receivesaea != null)
        m_receivesaea.Dispose();
      if (m_sendsaea != null)
        m_sendsaea.Dispose();
    }
  }
}
