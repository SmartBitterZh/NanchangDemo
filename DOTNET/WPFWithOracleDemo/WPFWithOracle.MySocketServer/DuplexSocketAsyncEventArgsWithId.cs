using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.MySocketServer
{
  /// <summary>
  /// 双工通信
  /// Socket服务器提高通信效率是一个永恒的话题，提高通信效率有很多种方法，双工通信就是其中之一。
  /// 一个SAEA对象在同一时刻只能用来接收数据或发送数据，有人想，
  /// 如果一个SAEA对象在同一时刻既能发送数据又能接受数据，那肯定会提高socket通信效率。
  /// 恩，很有想法！可是你能让你的头在同一时刻既往左转又往右转吗？答案是不行的，那如何实现双工通信呢？
  /// 既然一个SAEA对象在同一时刻只能做一件事，那我自定义DuplexSAEA对象，在该对象中封装两个SAEA，一个用于接受，一个用于发送，问题不就解决了吗。
  /// 
  /// 类是一个用户的连接的最小单元，也就是说对一个用户来说有两个SocketAsyncEventArgs对象，
  /// 这两个对象是一样的，但是有一个用来发送消息，一个接收消息，这样做的目的是为了实现双工通讯，
  /// 提高用户体验。默认的用户标识是"-1”，状态是false表示不可用
  /// </summary>
  public class DuplexSocketAsyncEventArgsWithId : IDisposable
  {
    /// <summary>
    /// 用户标识，跟MySocketAsyncEventArgs的UID是一样的，
    /// 在对DuplexSocketAsyncEventArgsWithId的UID属性赋值的时候也对MySocketAsyncEventArgs的UID属性赋值。
    /// </summary>
    private string m_uid = "-1";
    private bool m_state = false;
    private MySocketAsyncEventArgs m_receivesaea;
    private MySocketAsyncEventArgs m_sendsaea;
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
    internal DuplexSocketAsyncEventArgsWithId()
    {
      m_receivesaea = new MySocketAsyncEventArgs("Receive");
      m_sendsaea = new MySocketAsyncEventArgs("Send");
    }
    /// <summary>
    /// 接收消息
    /// </summary>
    public MySocketAsyncEventArgs ReceiveSAEA { get { return m_receivesaea; } set { m_receivesaea = value; } }
    /// <summary>
    /// 发送消息
    /// </summary>
    public MySocketAsyncEventArgs SendSAEA { get { return m_sendsaea; } set { m_sendsaea = value; } }
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
