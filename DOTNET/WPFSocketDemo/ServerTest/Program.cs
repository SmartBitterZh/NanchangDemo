using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketTest
{
  class Program
  {
    private static byte[] result = new byte[1024];
    static void Main(string[] args)
    {
      AnsycSocketTest();
    }

    private static void SocketTest()
    {
      IPAddress _ip = IPAddress.Parse("127.0.0.1");
      Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

      try
      {
        clientSocket.Connect(new IPEndPoint(_ip, 8088)); //配置服务器IP与端口  
        Console.WriteLine("连接服务器成功");
      }
      catch
      {
        Console.WriteLine("连接服务器失败，请按回车键退出！");
        return;
      }

      int _receiveLength = clientSocket.Receive(result);
      Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, _receiveLength));
      //通过 clientSocket 发送数据  
      for (int i = 0; i < 10; i++)
      {
        try
        {
          Thread.Sleep(1000);    //等待1秒钟  
          string sendMessage = " client send Message Hellp " + DateTime.Now;
          clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
          Console.WriteLine("向服务器发送消息：" + sendMessage);
        }
        catch
        {
          clientSocket.Shutdown(SocketShutdown.Both);
          clientSocket.Close();
          break;
        }
      }
      Console.WriteLine("发送完毕，按回车键退出");
      Console.ReadLine();
    }

    /// <summary>
    /// IAsyncResult
    /// </summary>
    private static void AnsycSocketTest()
    {
      IPAddress _ip = IPAddress.Parse("127.0.0.1");
      Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

      try
      {
        clientSocket.Connect(new IPEndPoint(_ip, 8088)); //配置服务器IP与端口  
        Console.WriteLine("连接服务器成功");
      }
      catch
      {
        Console.WriteLine("连接服务器失败，请按回车键退出！");
        return;
      }

      int _receiveLength = clientSocket.Receive(result);
      Console.WriteLine("接收服务器消息：{0}", Encoding.ASCII.GetString(result, 0, _receiveLength));

      //通过 clientSocket 发送数据  
      for (int i = 0; i < 10; i++)
      {
        try
        {
          Thread.Sleep(1000);    //等待1秒钟  
          string sendMessage = " client send Message Hellp " + DateTime.Now;

          clientSocket.BeginSend(Encoding.ASCII.GetBytes(sendMessage), 0, 1024, 0, new AsyncCallback(SendCallback), clientSocket);
          Console.WriteLine("向服务器发送消息：" + sendMessage);
        }
        catch
        {
          clientSocket.Shutdown(SocketShutdown.Both);
          clientSocket.Close();
          break;
        }
      }
      Console.WriteLine("发送完毕，按回车键退出");
      Console.ReadLine();
    }


    private static void SendCallback(IAsyncResult ar)
    {
      // 从state对象获取socket.
      Socket handler = (Socket)ar.AsyncState;
      //完成数据发送
      int bytesSent = handler.EndSend(ar);
      Console.WriteLine("Sent {0} bytes to client.", bytesSent);
    }
  }
}
