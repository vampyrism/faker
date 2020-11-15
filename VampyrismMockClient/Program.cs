using System;

namespace VampyrismMockClient
{
    class Program
    {
        static void Main(string[] args)
        {
            UDPClient c = new UDPClient("127.0.0.1", 25565);
            c.Init();
        }
    }
}
