using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.ServiceModel.Channels;
using System.Text;

namespace SocketService
{
  public class AsyncUserToken
  {
    Socket m_socket;
    public AsyncUserToken() : this(null) { }
    public AsyncUserToken(Socket socket) { m_socket = socket; }
    public Socket Socket { get { return m_socket; } set { m_socket = value; } }
  }
}
