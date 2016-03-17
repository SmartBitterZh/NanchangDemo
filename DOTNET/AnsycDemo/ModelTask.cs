using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnsycDemo
{
  public class ModelTask
  {
    static Task _task;
    public static void Test()
    {
      CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
      Console.WriteLine("主线程.");
      _task = Task.Factory.StartNew(() => Print("Hello world."), cancelTokenSource.Token);

      _task.ContinueWith(task => { Console.WriteLine("当前线程结束."); },
                                   TaskContinuationOptions.OnlyOnFaulted);

      Console.WriteLine("主线程继续执行...");

      Console.WriteLine("Press any key to continue...");
      Console.ReadKey(true);
    }
    public static void Print(string s)
    {
      Console.WriteLine("当前线程：" + s);
      Thread.Sleep(5000);
      if (_task.IsCompleted)
        PrintComeplete();
    }
    /// <summary>
    /// 
    /// </summary>
    public static void PrintException()
    {
      Console.WriteLine("当前线程出错.");
    }
    /// <summary>
    /// 
    /// </summary>
    public static void PrintComeplete()
    {
      Console.WriteLine("当前线程结束.");
    }
  }
}
