using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MySocketServer
{
  public class AsyncUserToken
  {
    private Socket m_socket;
    public AsyncUserToken(Socket socket)
    {
      m_socket = socket;
    }
    public Socket Socket { get { return m_socket; } set { m_socket = value; } }
    //public ISession Session { get; set; }
    //public Context Context { get; set; }
  }
}
