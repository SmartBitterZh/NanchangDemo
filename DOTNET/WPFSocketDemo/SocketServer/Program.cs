using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketServer
{
  class Program
  {
    static void Main(string[] args)
    {
      IPAddress _ip = IPAddress.Parse("127.0.0.1");
      SocketServer.ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      SocketServer.ServerSocket.Bind(new IPEndPoint(_ip, SocketServer.MyProt));

      SocketServer.ServerSocket.Listen(10);

      Console.WriteLine("Start {0} successfull", SocketServer.ServerSocket.LocalEndPoint.ToString());
      ThreadPool.QueueUserWorkItem(SocketServer.ListenClientConnect);
      Console.ReadLine(); 
    }
  }
}
