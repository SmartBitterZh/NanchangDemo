using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySocketServer2
{
  public enum SocketMessageInfoType
  {
    NONE = 0,
    JSON = 1,
    XML = 2,
    BYTE_ARRAY = 3,
    REQUEST = 4
  }
}
