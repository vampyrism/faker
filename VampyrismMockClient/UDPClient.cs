using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace VampyrismMockClient
{
    class UDPClient
    {
        IPEndPoint recv;
        String host;
        int port;

        UdpClient c;

        public UDPClient(String host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public void Init()
        {
            this.c = new UdpClient();
            this.c.Connect("localhost", 9000);
            this.c.Send(Encoding.ASCII.GetBytes("ping"), Encoding.ASCII.GetBytes("ping").Length);
        }

        private void RecieveCallback(IAsyncResult result)
        {

        }
    }
}
