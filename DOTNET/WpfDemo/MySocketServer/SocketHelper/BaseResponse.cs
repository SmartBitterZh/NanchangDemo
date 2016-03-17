using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySocketServer.SocketHelper
{
  [Serializable]
  public class BaseResponse<T>
  {
    public bool Success { get; set; }

    public string Message { get; set; }

    public virtual IList<T> Context { get; set; }
  }
}
