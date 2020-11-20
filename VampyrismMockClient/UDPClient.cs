using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace VampyrismMockClient
{
    class UDPClient
    {
        IPEndPoint recv;
        String host;
        int port;

        UInt16 localSeqNum = 0;
        UInt16 remoteSeqNum = 0;

        UdpClient c;

        Dictionary<int, byte[]> sent;

        public UDPClient(String host, int port)
        {
            this.host = host;
            this.port = port;
            this.sent = new Dictionary<int, byte[]>();
        }

        public (int, byte[]) BuildPacket(UInt16 seq, String message)
        {
            int length = 1 + 4 + 2 + 2 + Encoding.ASCII.GetBytes(message).Length;
            byte[] res = new byte[length];

            BitConverter.GetBytes((byte)1).CopyTo(res, 0);
            BitConverter.GetBytes(length).CopyTo(res, 1);
            BitConverter.GetBytes(seq).CopyTo(res, 5);
            BitConverter.GetBytes((UInt16)Encoding.ASCII.GetBytes(message).Length).CopyTo(res, 7);
            Encoding.ASCII.GetBytes(message).CopyTo(res, 9);

            return (length, res);
        }

        public void Init()
        {
            this.c = new UdpClient();
            this.c.Connect("127.0.0.1", 9000);
            IPEndPoint e = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000);

            Task.Run(() =>
            {
                while (true)
                {
                    byte[] data = new byte[1024];
                    data = this.c.Receive(ref e);

                    this.localSeqNum = (UInt16)(BitConverter.ToUInt16(data, 2) + Convert.ToUInt16(1));
                }
            });

            Task a = Task.Run(() =>
            {
                while (true)
                {
                    //System.Console.WriteLine("SENT: " + "ping from a client! Seq: " + Convert.ToString(i));
                    //this.c.Send(Encoding.ASCII.GetBytes("ping from a client! Seq: " + Convert.ToString(i)), 
                    //    Encoding.ASCII.GetBytes("ping from a client! Seq: " + Convert.ToString(i)).Length);

                    (int len, byte[] p) = this.BuildPacket(this.localSeqNum, "hello from client with seq = " + Convert.ToString(this.localSeqNum));
                    
                    //if(this.sent.ContainsKey((this.local)))
                    //this.sent.Add(this.localSeqNum, p);
                    
                    this.c.Send(p, len);

                    System.Console.WriteLine(Encoding.ASCII.GetString(p));

                    System.Threading.Thread.Sleep(250);
                }
            });

            a.Wait();
        }

        private void RecieveCallback(IAsyncResult result)
        {

        }
    }
}
