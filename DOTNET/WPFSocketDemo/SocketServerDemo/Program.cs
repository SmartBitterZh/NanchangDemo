using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketServerDemo
{
  class Program
  {
    static void Main(string[] args)
    {
      //SocketWithBeginEnd.Accept();
      AsynchronousSocketListener.StartListening();
      Console.ReadKey();
    }
  }
}
