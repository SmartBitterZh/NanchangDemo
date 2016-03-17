using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MySocketServer2
{
  /// <summary>
  /// 
  /// </summary>
  public sealed class SocketBufferManager : IDisposable
  {
    private byte[] m_buffer;
    private int m_bufferSize;
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

    internal bool SetBuffer(SocketAsyncEventArgs args)
    {
      if(this.m_freeIndexPool.Count>0)
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

    public void FreeBuffer(SocketAsyncEventArgs args)
    {
      m_freeIndexPool.Push(args.Offset);
      args.SetBuffer(null,0,0);
    }
    public void Dispose()
    {
      m_freeIndexPool.Clear();
      m_freeIndexPool = null;
      m_buffer = null;
    }
  }
}
