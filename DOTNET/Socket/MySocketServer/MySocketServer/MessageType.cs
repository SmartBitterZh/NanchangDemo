using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySocketServer
{
  public enum MessageType
  {
    /// <summary>
    ///    发送成功
    /// </summary>
    Success = 100,
    /// <summary>
    ///    发送失败
    /// </summary>
    Failed = 200,
    /// <summary>
    /// 用户不在线
    /// </summary>
    UseOffline = 300,

    Error = 400
  }
}
