using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFWithOracle.MySocketServer.SocketHelper
{
  public enum MessageFunction
  {
    ConnectDB,
    FindAll,
    Insert,
    Delete,
    Update
  }
}
