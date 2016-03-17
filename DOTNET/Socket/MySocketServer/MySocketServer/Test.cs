using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MySocketServer
{
  public class Test
  {
    static SocketListener _listen;
    static Thread _thread;
    static void Main(string[] args)
    {
      _listen = new SocketListener(80000, 4096, null);
      _listen.Init();

      _thread = new Thread(_listen.Listen);
      _thread.IsBackground = true;

      _listen.OnSended += _listen_OnSended;
      _listen.OnMsgReceived += _listen_OnMsgReceived;
           
      _listen.Start(8088, "127.0.0.1");
      Console.WriteLine("Server start");

      Console.ReadKey();
    }

    static void _listen_OnMsgReceived(string uid, string info)
    {
      Console.WriteLine(uid + ":" + info);
    }

    static void _listen_OnSended(string uid, string exception)
    {
      Console.WriteLine(uid + ":" + exception);
    }
  }
}
