using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalculateApi
{
  public class CalculateTest
  {
    static void Test()
    {
      CalculateLib _cal = new CalculateLib();

      // 基于IAsyncResult构造一个异步API
      IAsyncResult _calculateResult = _cal.BeginCalculate(123, 456, AfterCallback, _cal);
      // 执行异步调用后，若我们需要控制后续执行代码在异步操作执行完之后执行，可通过下面三种方式阻止其他工作：
      // 1、IAsyncResult 的 AsyncWaitHandle 属性，带异步操作完成时获得信号。
      // 2、通过 IAsyncResult 的 IsCompleted 属性进行轮询。通过轮询还可实现进度条功能。
      // 3、调用异步操作的 End*** 方法。
      // ***********************************************************
      // 1、calculateResult.AsyncWaitHandle.WaitOne();
      // 2、while (calculateResult.IsCompleted) { Thread.Sleep(1000); }
      // 3、

      string resultStr = string.Empty;
      int _result = _cal.EndCalculate(ref resultStr, _calculateResult);

      Console.WriteLine("Test:" + resultStr);
      Console.ReadKey();

    }
    private static void AfterCallback(ref string resultStr, IAsyncResult ar)
    {
      // 执行异步调用后，若我们不需要阻止后续代码的执行，
      // 那么我们可以把异步执行操作后的响应放到回调中进行。
      CalculateLib cal = ar.AsyncState as CalculateLib;

      Console.WriteLine("Callback:" + resultStr);
      //int result = cal.EndCalculate(ref resultStr, calculateResult1);
      //if (result > 0) { }
    }
  }
}
