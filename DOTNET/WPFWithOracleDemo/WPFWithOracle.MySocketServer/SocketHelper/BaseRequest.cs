using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WPFWithOracle.MySocketServer.SocketHelper
{
  [Serializable]
  public class BaseRequest
  {
    public string UID { get; set; }
    public MessageEntityType MessageType { get; set; }
    public MessageFunction MessageCommand { get; set; }
    public string Message { get; set; }
  }
}
