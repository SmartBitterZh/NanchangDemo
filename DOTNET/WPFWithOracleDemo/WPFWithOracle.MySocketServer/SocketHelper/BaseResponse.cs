using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFWithOracle.MySocketServer.SocketHelper
{
  [Serializable]
  public abstract class BaseResponse
  {
    public bool Success { get; set; }

    public string Message { get; set; }

    public virtual object Context { get; set; }
  }
}
