using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MySocketServer
{
  /// <summary>
  /// Buffer池，用于集中管控Socket缓冲区，防止内存碎片。
  /// 
  /// 每一个SocketAsyncEventArgs对象（以下简称SAEA）在内存中都有其对应的缓存空间，
  /// 如果不对这些缓存空间进行同一管理，当SAEA对象逐渐增多时，这些SAEA对象的缓存空间会越来越大，
  /// 它们在系统内存中不是连续的，造成很多内存碎片，而且这些缓存不能重复利用，
  /// 当创建、销毁SAEA对象时，造成CPU很多额外消耗，影响服务器性能。面对这问题如何解决呢？用Buffer池管理！
  /// 
  /// 
  /// 该类是一个管理连接缓冲区的类，职责是为每一个连接维持一个接收数据的区域。
  /// 它的设计也采用了类似与池的技术，先实例化好多内存区域，并把每一块的地址放入栈中，
  /// 每执行依次pop时拿出一块区域来给SocketAsyncEventArgs对象作为Buffer.
  /// </summary>
  public sealed class SocketBufferManager : IDisposable
  {
    /// <summary>
    /// the underlying byte array maintained by the Buffer Manager
    /// </summary>
    private byte[] m_buffer;
    private int m_bufferSize;
    /// <summary>
    /// the total number of bytes controlled by the buffer pool
    /// </summary>
    private int m_numBytes;
    private int m_currentIndex;
    private Stack<int> m_freeIndexPool;
    public SocketBufferManager()
    {
    }
    public SocketBufferManager(int totalBytes, int bufferSize)
    {
      m_numBytes = totalBytes;
      m_currentIndex = 0;
      m_bufferSize = bufferSize;
      m_freeIndexPool = new Stack<int>();
    }

    public void InitBuffer()
    {
      m_buffer = new byte[m_numBytes];
    }

    /// <summary>
    /// Assigns a buffer from the buffer pool to the 
    /// specified SocketAsyncEventArgs object
    /// </summary>
    /// <param name="args"></param>
    /// <returns>true if the buffer was successf ully set, else false</returns>
    internal Boolean SetBuffer(SocketAsyncEventArgs args)
    {
      if (this.m_freeIndexPool.Count > 0)
        args.SetBuffer(this.m_buffer, this.m_freeIndexPool.Pop(), this.m_bufferSize);
      else
      {
        if ((this.m_numBytes - this.m_bufferSize) < this.m_currentIndex)
          return false;

        args.SetBuffer(this.m_buffer, this.m_currentIndex, this.m_bufferSize);
        this.m_currentIndex += this.m_bufferSize;
      }

      return true;
    }
    /// <summary>
    /// Removes the buffer from a SocketAsyncEventArg object.  
    /// This frees the buffer back to the buffer pool
    /// </summary>
    /// <param name="args"></param>
    public void FreeBuffer(SocketAsyncEventArgs args)
    {
      m_freeIndexPool.Push(args.Offset);
      args.SetBuffer(null, 0, 0);
    }

    public void Dispose()
    {
      m_freeIndexPool.Clear();
      m_freeIndexPool = null;
      m_buffer = null;
    }
  }
}
