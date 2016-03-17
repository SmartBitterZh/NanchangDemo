using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySocketServer2
{
  public enum SocketAsyncType
  {
    Connect = 0,
    Accept = 1,
    Receive = 2, 
    Send = 3
  }
}
