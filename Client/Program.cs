using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD5Breaker.Network;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionManager cm = new ConnectionManager();

            cm.Connect("127.0.0.1", 25565);

            while (true)
                Console.ReadLine();
        }
    }
}
