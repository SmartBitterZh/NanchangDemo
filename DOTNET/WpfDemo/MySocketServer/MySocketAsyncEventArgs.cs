using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MySocketServer
{
  /// <summary>
  /// 考虑到能快速的反应用户的连接请求我采用了连接池的技术，类似于sqlserver的连接池，
  /// 当然我的“池”还不够好，为了能快速的处理接受的数据我又加入了一个缓冲区池，
  /// 说白了就是给每一个连接对象事先开辟好了空间。
  /// 在传输方面，为了保证数据的有效性我们采用客户端和服务器端的验证
  /// </summary>
  public sealed class MySocketAsyncEventArgs : SocketAsyncEventArgs
  {
    /// <summary>
    /// 用户标识符，用来标识这个连接是那个用户的。
    /// </summary>
    internal string UID = Guid.NewGuid().ToString();
    /// <summary>
    /// 标识该连接是用来发送信息还是监听接收信息的。
    /// param:Receive/Send/Connect/Accept;
    /// </summary>
    private string Property;

    /// <summary>
    /// MySocketAsyncEventArgs类只带有一个参数的构造函数，说明类在实例化时就被说明是用来完成接收还是发送任务的
    /// </summary>
    /// <param name="property"></param>
    internal MySocketAsyncEventArgs(string property)
    {
      this.Property = property;
    }
  }
}
