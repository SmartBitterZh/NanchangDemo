using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MySocketServer
{
  public interface ISession
  {
    void Initize(Socket socket);
    void ProcessReceive(MySocketAsyncEventArgs e);
    void ProcessSend(MySocketAsyncEventArgs e);
    IPEndPoint LocalEndPoint { get; }
    IPEndPoint RemoteEndPoint { get; }

    event EventHandler<MySocketAsyncEventArgs> Closed;
  }
}
