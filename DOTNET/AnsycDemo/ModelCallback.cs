using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AnsycDemo
{
  /// <summary>
  /// 异步调用方法总结：
  /// 4.回调
  /// 之前三种方法者在等待异步方法执行完毕后才能拿到执行的结果，期间主线程均处于等待状态。
  /// 回调和它们最大的区别是，在调用BeginInvoke时只要提供了回调方法，那么主线程就不必要再等待异步线程工作完毕，
  /// 异步线程在工作结束后会主动调用我们提供的回调方法，并在回调方法中做相应的处理，例如显示异步调用的结果。
  /// </summary>
  public class ModelCallback
  {
    public delegate void PrintDelegate(string s);
    public static void Test()
    {
      PrintDelegate printDelegate = Print;
      Console.WriteLine("主线程.");
      printDelegate.BeginInvoke("Hello world.", PrintComeplete, printDelegate);
      Console.WriteLine("主线程继续执行...");

      Console.WriteLine("Press any key to continue...");
      Console.ReadKey(true);
    }
    public static void Print(string s)
    {
      Console.WriteLine("当前线程：" + s);
      Thread.Sleep(5000);
    }
    //回调方法要求
    //1.返回类型为void
    //2.只有一个参数IAsyncResult
    public static void PrintComeplete(IAsyncResult result)
    {
      (result.AsyncState as PrintDelegate).EndInvoke(result);
      Console.WriteLine("当前线程结束." + result.AsyncState.ToString());
    }
  }
}
