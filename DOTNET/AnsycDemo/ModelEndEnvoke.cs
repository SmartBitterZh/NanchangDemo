using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AnsycDemo
{
  /// <summary>
  /// 异步调用方法总结：属于等待类型
  /// 1.BeginEnvoke EndEnvoke
  /// 当使用BeginInvoke异步调用方法时，如果方法未执行完，
  /// EndInvoke方法就会一直阻塞，直到被调用的方法执行完毕
  /// </summary>
  public class ModelEndEnvoke
  {
    public delegate void PrintDelegate(string s);
    public static void Test()
    {
      PrintDelegate printDelegate = Print;
      Console.WriteLine("主线程");

      IAsyncResult result = printDelegate.BeginInvoke("Hello World.", null, null);
      Console.WriteLine("主线程继续执行...");
      //当使用BeginInvoke异步调用方法时，如果方法未执行完，
      //EndInvoke方法就会一直阻塞，直到被调用的方法执行完毕
      printDelegate.EndInvoke(result);

      Console.WriteLine("Press any key to continue...");
      Console.ReadKey(true);
    }

    public static void Print(string s)
    {
      Console.WriteLine("异步线程开始执行：" + s);
      Thread.Sleep(5000);
      Console.WriteLine("异步线程执行完成");
    }
  }
}
