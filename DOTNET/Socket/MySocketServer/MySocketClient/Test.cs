using MySocketServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MySocketClient
{
  public class Test
  {
    static List<string> _idIP = new List<string>();
    static void Main(string[] args)
    {
      SocketClient _client = new SocketClient("127.0.0.1", 8088);
      _client.OnSended += _client_OnSended;
      _client.OnMsgReceived += _client_OnMsgReceived;

      if (_client.Connect())
      {
        _client.Send("Hello Server.");
        _client.Send(Console.ReadLine());
        _client.Send(Console.ReadLine());
        _client.Send(Console.ReadLine());
        _client.Send(Console.ReadLine());
      }
      else
        Console.WriteLine("failed");
      Console.ReadKey();
    }

    static void _client_OnMsgReceived(string info)
    {
      Console.WriteLine(info);
    }

    static void _client_OnSended(bool successorfalse)
    {
      //Console.WriteLine("message send success");
    }

  }
}
