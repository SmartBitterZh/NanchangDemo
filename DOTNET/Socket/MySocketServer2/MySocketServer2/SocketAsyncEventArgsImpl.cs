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
  public sealed class SocketAsyncEventArgsImpl : SocketAsyncEventArgs
  {
    /// <summary>
    /// 
    /// </summary>
    internal string UID = Guid.NewGuid().ToString();
    /// <summary>
    /// param:Receive/Send/Connect/Accept;
    /// </summary>
    private SocketAsyncType propertyType;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="property"></param>
    internal SocketAsyncEventArgsImpl(SocketAsyncType propertyType)
    {
      this.propertyType = propertyType;
    }
  }
}
