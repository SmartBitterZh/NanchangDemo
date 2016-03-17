using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SocketServer
{
  public class SocketServer
  {
    public static byte[] result = new byte[1024];
    public static int MyProt = 8885;   //端口  
    public static Socket ServerSocket;
    public static void Test(string[] args)
    {
      //服务器IP地址  
      IPAddress ip = IPAddress.Parse("127.0.0.1");
      ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      ServerSocket.Bind(new IPEndPoint(ip, MyProt));  //绑定IP地址：端口  
      ServerSocket.Listen(10);    //设定最多10个排队连接请求  
      Console.WriteLine("启动监听{0}成功", ServerSocket.LocalEndPoint.ToString());
      //通过Clientsoket发送数据  
      Thread myThread = new Thread(ListenClientConnect);
      myThread.Start();
      Console.ReadLine();
    }

    /// <summary>  
    /// 监听客户端连接  
    /// </summary>  
    public static void ListenClientConnect(object state)
    {
      while (true)
      {
        Socket clientSocket = ServerSocket.Accept();
        clientSocket.Send(Encoding.ASCII.GetBytes("Server Say Hello"));
        Thread receiveThread = new Thread(ReceiveMessage);
        receiveThread.Start(clientSocket);
      }
    }

    /// <summary>  
    /// 接收消息  
    /// </summary>  
    /// <param name="clientSocket"></param>  
    public static void ReceiveMessage(object clientSocket)
    {
      Socket myClientSocket = (Socket)clientSocket;
      while (true)
      {
        try
        {
          //通过clientSocket接收数据  
          int receiveNumber = myClientSocket.Receive(result);
          Console.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
          myClientSocket.Shutdown(SocketShutdown.Both);
          myClientSocket.Close();
          break;
        }
      }
    }  
  }
}
