using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CalculateApi
{
  public delegate void RefAsyncCallback(ref string resultStr, IAsyncResult ar);

  public class CalculateAsyncResult : IAsyncResult
  {
    private int _calcNum1;
    private int _calcNum2;
    private RefAsyncCallback _userCallback;
    private object _asyncState;
    public object AsyncState { get { return _asyncState; } }
    /// <summary>
    /// 存储最后结果值
    /// </summary>
    public int FinnalyResult { get; set; }
    /// <summary>
    /// End方法只应调用一次，超过一次报错
    /// </summary>
    public int EndCallCount = 0;
    /// <summary>
    /// ref参数
    /// </summary>
    public string ResultStr;

    public CalculateAsyncResult(int num1, int num2, RefAsyncCallback userCallback, object asyncState)
    {
      this._calcNum1 = num1;
      this._calcNum2 = num2;
      this._userCallback = userCallback;
      this._asyncState = asyncState;
      // 异步执行操作
      ThreadPool.QueueUserWorkItem((obj) => { AsyncCalculate(obj); }, this);
    }
    #region IAsyncResult接口

    private ManualResetEvent _asyncWaitHandle;
    public WaitHandle AsyncWaitHandle
    {
      get
      {
        if (this._asyncWaitHandle == null)
        {
          ManualResetEvent event2 = new ManualResetEvent(false);
          Interlocked.CompareExchange<ManualResetEvent>(ref this._asyncWaitHandle, event2, null);
        }
        return _asyncWaitHandle;
      }
    }

    private bool _completedSynchronously;
    public bool CompletedSynchronously { get { return _completedSynchronously; } }

    private bool _isCompleted;
    public bool IsCompleted { get { return _isCompleted; } }
    #endregion

    /// <summary>
    /// 异步进行耗时计算
    /// </summary>
    /// <param name="ob">CalculateAsyncResult实例本身</param>
    private static void AsyncCalculate(object obj)
    {
      CalculateAsyncResult _asyncResult = obj as CalculateAsyncResult;
      Thread.SpinWait(1000);
      _asyncResult.FinnalyResult = _asyncResult._calcNum1 * _asyncResult._calcNum2;
      _asyncResult.ResultStr = _asyncResult.FinnalyResult.ToString();

      // 是否同步完成
      _asyncResult._completedSynchronously = false;
      _asyncResult._isCompleted = true;
      ((ManualResetEvent)_asyncResult.AsyncWaitHandle).Set();
      if (_asyncResult._userCallback != null)
        _asyncResult._userCallback(ref _asyncResult.ResultStr, _asyncResult);
    }
  }
}
