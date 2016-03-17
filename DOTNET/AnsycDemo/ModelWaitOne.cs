using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AnsycDemo
{
  /// <summary>
  /// 异步调用方法总结：属于等待类型
  /// 2.WaitOne
  /// 可以看到，与EndInvoke类似，只是用WaitOne函数代码了EndInvoke而已。
  /// </summary>
  public class ModelWaitOne
  {
    public delegate void PrintDelegate(string s);
    public static void Test()
    {
      PrintDelegate printDelegate = Print;
      Console.WriteLine("主线程");
      IAsyncResult result = printDelegate.BeginInvoke("Hello World.", null, null);
      Console.WriteLine("主线程继续执行...");
      result.AsyncWaitHandle.WaitOne(-1, false);

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
