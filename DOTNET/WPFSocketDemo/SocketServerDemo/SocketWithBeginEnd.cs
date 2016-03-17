using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketServerDemo
{
  public class SocketWithBeginEnd
  {
    static byte[] ReceiveBuffer = new byte[1024];

    public static void Accept()
    {
      Socket NewSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      IPEndPoint LocalEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8088);
      try
      {
        NewSocket.Bind(LocalEP);
        NewSocket.Listen(10);
        NewSocket.BeginAccept(new AsyncCallback(BeginConnection), NewSocket);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message.ToString());
      }
    }
    public static void BeginConnection(IAsyncResult IA)
    {
      Socket ServerSocket = (Socket)IA.AsyncState;

      Socket ClientSocket = ServerSocket.EndAccept(IA);
      Send(ClientSocket, "Server say hello");
      ClientSocket.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReadCallBack), ClientSocket);
      ServerSocket.BeginAccept(new AsyncCallback(BeginConnection), ServerSocket);
    }


    public static void ReadCallBack(IAsyncResult IA)
    {
      Socket SocketClient = (Socket)IA.AsyncState;
      int ByteRead = 0;
      try
      {
        ByteRead = SocketClient.EndReceive(IA);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message.ToString());
      }
      if (ByteRead > 0)
      {
        string Content = Encoding.ASCII.GetString(ReceiveBuffer, 0, ByteRead);
        Console.WriteLine("接收客户端{0}消息{1}", SocketClient.RemoteEndPoint.ToString(), Content);
      }
      SocketClient.BeginReceive(ReceiveBuffer, 0, ReceiveBuffer.Length, SocketFlags.None, new AsyncCallback(ReadCallBack), SocketClient);
    }


    private static void Send(Socket handler, string data)
    {
      // 消息格式转换.
      byte[] byteData = Encoding.ASCII.GetBytes(data);
      // 开始发送数据给远程目标.
      handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
    }

    private static void SendCallback(IAsyncResult ar)
    {
      // 从state对象获取socket.
      Socket handler = (Socket)ar.AsyncState;
      //完成数据发送
      int bytesSent = handler.EndSend(ar);
      Console.WriteLine("Sent {0} bytes to client.", bytesSent);
      //handler.Shutdown(SocketShutdown.Both);
      //handler.Close();
    }
  }
}
