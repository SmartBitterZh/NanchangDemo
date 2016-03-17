using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySocketServer
{
  /// <summary>
  /// SAEA池，用于集中管控Socket，重复利用Socket（主要处理发送数据包和接收数据包，并转发给业务逻辑层）。
  /// 
  /// 一个容器，一个容纳AcceptSAEA对象的容器。给你两个socket服务器，你能很快判别两个服务器性能的优异吗？
  /// 很简单，你瞬间向一台服务器打入5、6万的连接，看看会不会都连上，如果都连上，说明这台socket服务器的并发处理连接的能力还是不错的.
  /// 那如何提高socket服务器的并发连接能力呢？答案：poolOfAcceptEventArgs！
  /// 
  /// 这个类才是真正的连接池类，这个类真正的为server提供一个可用的用户连接，并且维持这个连接直到用户断开，并把不用的连接放回连接池中供下一用户连接。
  /// 
  /// Note:这个类的设计缺陷是使用了太多的lock语句，对对象做了太多的互斥操作，所以我尽量的把lock内的语句化简或挪到lock外部执行。
  /// </summary>
  public class SocketAsyncEventArgsPool : IDisposable
  {
    /// <summary>
    /// 从字面意思上就知道这是一个连接栈，用来存放空闲的连接的，使用时pop出来，使用完后push进去
    /// </summary>
    internal Stack<DuplexSocketAsyncEventArgsWithId> m_pool;
    /// <summary>
    /// 这个也很好理解，busypool是一个字典类型的，用来存放正在使用的连接的，key是用户标识，
    /// 设计的目的是为了统计在线用户数目和查找相应用户的连接，当然这是很重要的，为什么设计成字典类型的，
    /// 是因为我们查找时遍历字典的关键字就行了而不用遍历每一项的UID，这样效率会有所提高。
    /// </summary>
    internal IDictionary<string, DuplexSocketAsyncEventArgsWithId> m_busypool;
    /// <summary>
    /// 这是一个存放用户标识的数组，起一个辅助的功能。
    /// </summary>
    private string[] m_keys;
    /// <summary>
    /// 连接池中可用的连接数。
    /// </summary>
    internal Int32 Count
    {
      get
      {
        lock (this.m_pool)
        {
          return this.m_pool.Count;
        }
      }
    }
    /// <summary>
    /// 在线用户的标识列表。
    /// </summary>
    internal string[] OnlineUID
    {
      get
      {
        lock (this.m_busypool)
        {
          m_busypool.Keys.CopyTo(m_keys, 0);
        }
        return m_keys;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="capacity">最大连接数</param>
    internal SocketAsyncEventArgsPool(Int32 capacity)
    {
      this.m_keys = new string[capacity];
      this.m_pool = new Stack<DuplexSocketAsyncEventArgsWithId>(capacity);
      this.m_busypool = new Dictionary<string, DuplexSocketAsyncEventArgsWithId>(capacity);
    }

    /// <summary>
    /// 用于获取一个可用连接给用户。
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    internal DuplexSocketAsyncEventArgsWithId Pop(string uid)
    {
      if (string.IsNullOrEmpty(uid))
        return null;
      DuplexSocketAsyncEventArgsWithId _si = null;
      lock (this.m_pool)
      {
        _si = this.m_pool.Pop();
      }

      _si.UID = uid;
      _si.State = true;
      m_busypool.Add(uid, _si);
      return _si;
    }

    /// <summary>
    /// 把一个使用完的连接放回连接池。
    /// </summary>
    /// <param name="item"></param>
    internal void Push(DuplexSocketAsyncEventArgsWithId item)
    {
      if (item == null)
        throw new ArgumentNullException("DuplexSocketAsyncEventArgsWithId 对象为空");
      if (item.State == true)
      {
        if (m_busypool.Keys.Count != 0)
        {
          if (m_busypool.Keys.Contains(item.UID))
            m_busypool.Remove(item.UID);
          else
            throw new ArgumentException("SocketAsyncEventWithId不在忙碌队列中");
        }
        else
          throw new ArgumentException("忙碌队列为空");
      }
      item.UID = "-1";
      item.State = false;
      lock (this.m_pool)
      {
        this.m_pool.Push(item);
      }
    }

    /// <summary>
    /// 查找在线用户连接，返回这个连接。
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    internal DuplexSocketAsyncEventArgsWithId FindByUID(string uid)
    {
      if (string.IsNullOrEmpty(uid))
        return null;
      string _key = OnlineUID.FirstOrDefault(si => { return si == uid; });
      if (string.IsNullOrEmpty(_key))
        throw new ArgumentException("未发现用户链接 " + uid);
      DuplexSocketAsyncEventArgsWithId _si = m_busypool[_key];
      return _si;
    }
    /// <summary>
    /// 判断某个用户的连接是否在线。
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    internal bool BusyPoolContains(string uid)
    {
      lock (this.m_busypool)
      {
        return m_busypool.Keys.Contains(uid);
      }
    }

    public void Dispose()
    {
      if (this.m_busypool != null && this.m_busypool.Count > 0)
      {
        foreach (var item in this.m_busypool)
          item.Value.Dispose();
        this.m_busypool.Clear();
      }
      if (this.m_pool != null && this.m_pool.Count > 0)
        this.m_pool.Clear();
      this.m_keys = null;
    }
  }
}
