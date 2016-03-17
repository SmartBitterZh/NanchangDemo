using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySocketServer2
{
  public class SocketAsyncEventArgsPool : IDisposable
  {
    /// <summary>
    /// 
    /// </summary>
    internal Stack<SocketAsyncEventArgsWithIdDuplex> m_pool;
    /// <summary>
    ///
    /// </summary>
    internal IDictionary<string, SocketAsyncEventArgsWithIdDuplex> m_busypool;
    /// <summary>
    /// 这是一个存放用户标识的数组，起一个辅助的功能。
    /// </summary>
    private string[] m_keys;
    /// <summary>
    /// 连接池中可用的连接数。
    /// </summary>
    internal int Count
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
    internal SocketAsyncEventArgsPool(int capacity)
    {
      this.m_keys = new string[capacity];
      this.m_pool = new Stack<SocketAsyncEventArgsWithIdDuplex>(capacity);
      this.m_busypool = new Dictionary<string, SocketAsyncEventArgsWithIdDuplex>(capacity);
    }

    /// <summary>
    /// 用于获取一个可用连接给用户。
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    internal SocketAsyncEventArgsWithIdDuplex Pop(string uid)
    {
      if (string.IsNullOrEmpty(uid))
        return null;
      SocketAsyncEventArgsWithIdDuplex _si = null;
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
    internal void Push(SocketAsyncEventArgsWithIdDuplex item)
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
    internal SocketAsyncEventArgsWithIdDuplex FindByUID(string uid)
    {
      if (string.IsNullOrEmpty(uid))
        return null;
      string _key = OnlineUID.FirstOrDefault(si => { return si == uid; });
      if (string.IsNullOrEmpty(_key))
        throw new ArgumentException("未发现用户链接 " + uid);
      SocketAsyncEventArgsWithIdDuplex _si = m_busypool[_key];
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
