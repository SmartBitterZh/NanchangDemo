using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SocketService
{
  class Program
  {
    static void Main(string[] args)
    {
      IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1333);
      //IPEndPoint iep = new IPEndPoint(IPAddress.Parse("192.168.210.76"), 1333);
      ServerSocketAsyncEventArgs objServer = new ServerSocketAsyncEventArgs(1000, 10);
      objServer.Init();
      objServer.Start(iep);
      Console.ReadKey();

    }
  }
}
