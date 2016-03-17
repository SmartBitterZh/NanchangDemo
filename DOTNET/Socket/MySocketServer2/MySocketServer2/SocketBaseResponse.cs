using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySocketServer2
{
  public class SocketBaseResponse<T>
  {
    public bool Success { get; set; }

    public string Message { get; set; }

    public virtual IList<T> Context { get; set; }
  }
}
