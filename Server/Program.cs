using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD5Breaker.Networking;
using MD5Breaker.Networking.Packets;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionManager cm = new ConnectionManager();

            cm.Start(25565, 100);

            while (true)
                cm.Broadcast(new Message(Console.ReadLine()));
        }
    }
}
