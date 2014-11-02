using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD5Breaker.Networking;
using MD5Breaker.Networking.Packets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionManager cm = new ConnectionManager();

            cm.Connect("127.0.0.1", 25565);

            while (true)
                cm.Broadcast(new Message(Console.ReadLine()));
        }
    }
}
