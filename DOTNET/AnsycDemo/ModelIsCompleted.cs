using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AnsycDemo
{
  /// <summary>
  /// 异步调用方法总结：
  /// 3.轮询
  /// 之前提到的两种方法，只能等下异步方法执行完毕，
  /// 在完毕之前没有任何提示信息，整个程序就像没有响应一样，用户体验不好，
  /// 可以通过检查IasyncResult类型的IsCompleted属性来检查异步调用是否完成，
  /// 如果没有完成，则可以适时地显示一些提示信息
  /// </summary>
  public class ModelIsCompleted
  {
    public delegate void PrintDelegate(string s);
    public static void Test()
    {
      PrintDelegate printDelegate = Print;
      Console.WriteLine("主线程:" + Thread.CurrentThread.ManagedThreadId);
      IAsyncResult result = printDelegate.BeginInvoke("Hello world.", null, null);
      Console.WriteLine("主线程:" + Thread.CurrentThread.ManagedThreadId + ",继续执行...");
      while (!result.IsCompleted)
      {
        Console.WriteLine(".");
        Thread.Sleep(500);
      }

      Console.WriteLine("主线程:" + Thread.CurrentThread.ManagedThreadId + "  Press any key to continue...");
      Console.ReadKey(true);
    }
    public static void Print(string s)
    {
      Console.WriteLine("当前线程：" + Thread.CurrentThread.ManagedThreadId + s);
      Thread.Sleep(5000);
    }
  }
}
