using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySocketServer2
{
  public class SocketBaseRequest
  {
    public string UID { get; set; }
    public string ControlName { get; set; }
    public string ControlFunc { get; set; }
    public string URL { get; set; }
    public string Message { get; set; }
  }
}
