using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MySocketServer2
{
  public class SocketAsyncResult<T> : IAsyncResult
  {
    private SocketBaseRequest _request;
    private SocketClient _socketClient;
    private AsyncCallback _userCallback;

    public SocketAsyncResult(SocketBaseRequest request, SocketClient socketClient, AsyncCallback userCallback, object asyncState)
    {
      this._request = request;
      this._socketClient = socketClient;
      this._userCallback = userCallback;
      this._asyncState = asyncState;
      // 异步执行操作
      ThreadPool.QueueUserWorkItem((obj) => { AsyncRequest(obj); }, this);
    }

    #region IAsyncResult接口
    private object _asyncState;
    public object AsyncState { get { return _asyncState; } }

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
    /// 
    /// </summary>
    public SocketBaseResponse<T> FinnalyResult { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int EndCallCount = 0;
    /// <summary>
    /// 
    /// </summary>
    public string ResultStr;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    private void AsyncRequest(object obj)
    {
      SocketAsyncResult<T> asyncResult = obj as SocketAsyncResult<T>;

      if (_socketClient.SendRequest(_request))
      {
        asyncResult.ResultStr = _socketClient.Listen();
      }
      asyncResult.FinnalyResult = SerializeHelper.JsonDeserialize<SocketBaseResponse<T>>(asyncResult.ResultStr);

      // 是否同步完成
      asyncResult._completedSynchronously = false;
      asyncResult._isCompleted = true;
      ((ManualResetEvent)asyncResult.AsyncWaitHandle).Set();
      if (asyncResult._userCallback != null)
        asyncResult._userCallback(asyncResult);
    }
  }

  public class SocketSendLib<T>
  {
    public  IAsyncResult BeginSend(SocketBaseRequest request, SocketClient socketClient, AsyncCallback userCallback, object asyncState)
    {
      SocketAsyncResult<T> result = new SocketAsyncResult<T>(request, socketClient, userCallback, asyncState);
      return result;
    }

    public  SocketBaseResponse<T> EndSend(ref string resultStr, IAsyncResult ar)
    {
      SocketAsyncResult<T> result = ar as SocketAsyncResult<T>;
      if (Interlocked.CompareExchange(ref result.EndCallCount, 1, 0) == 1)
        throw new Exception("End方法只能调用一次。");
      result.AsyncWaitHandle.WaitOne();
      resultStr = result.ResultStr;
      return result.FinnalyResult;
    }

    public string Send(SocketBaseRequest request, SocketClient socketClient, ref string resultStr)
    {
      string _request = string.Empty;
      if (socketClient.SendRequest(request))
        resultStr = _request = socketClient.Listen();

      return _request;
    }
  }
}
