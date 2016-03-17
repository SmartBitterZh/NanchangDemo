using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MySocketServer.SocketHelper
{
  [Serializable]
  public class BaseRequest
  {
    public string UID { get; set; }

    public string ControlName { get; set; }
    public string ControlFunc { get; set; }
    public string URL { get; set; }

    public string Message { get; set; }
  }
}
