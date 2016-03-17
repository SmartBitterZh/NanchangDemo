using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AnsycDemo
{
  partial class ModelEvent
  {
    /// <summary>
    /// 自定义委托
    /// </summary>
    /// <param name="s"></param>
    public delegate void PrintCompletedEventHandler(string s);

    public event PrintCompletedEventHandler PrintCompleted;


    public event EventHandler PrintSuccessfull;

    public void Test()
    {
      PrintCompleted += PrintMSG;
      PrintSuccessfull += ModelEvent_PrintSuccessfull;

      Console.WriteLine("主线程");

      Print("Hello World.");

      Console.WriteLine("主线程继续执行...");

      Console.WriteLine("Press any key to continue...");
      Console.ReadKey(true);
    }

    void ModelEvent_PrintSuccessfull(object sender, EventArgs e)
    {
      PrintCompleted("异步线程执行成功");
    }

    public void Print(string s)
    {
      PrintMSG("异步线程开始执行：" + s);
      if (PrintSuccessfull != null)
        PrintSuccessfull(null, null);
      Thread.Sleep(5000);
      if (PrintCompleted != null)
        PrintCompleted("异步线程执行完成");

    }
    public void PrintMSG(string s)
    {
      Console.WriteLine(s);
    }

  }


  public class PrintEventArgs : EventArgs
  {
    public PrintEventArgs(bool isSuccess)
    {
      this.isSuccess = isSuccess;
    }
    public bool isSuccess;
  }
}
