using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CalculateApi
{
  public class CalculateLib
  {
    public IAsyncResult BeginCalculate(int num1, int num2, RefAsyncCallback userCallback, object asyncState)
    {
      CalculateAsyncResult result = new CalculateAsyncResult(num1, num2, userCallback, asyncState);
      return result;
    }
    public int EndCalculate(ref string resultStr, IAsyncResult ar)
    {
      CalculateAsyncResult result = ar as CalculateAsyncResult;
      if (Interlocked.CompareExchange(ref result.EndCallCount, 1, 0) == 1)
      {
        throw new Exception("End方法只能调用一次。");
      }
      result.AsyncWaitHandle.WaitOne();

      resultStr = result.ResultStr;

      return result.FinnalyResult;
    }

    public int Calculate(int num1, int num2, ref string resultStr)
    {
      resultStr = (num1 * num2).ToString();
      return num1 * num2;
    }
  }
}
