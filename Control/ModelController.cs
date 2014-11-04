using MD5Breaker.Networking;
using MD5Breaker.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control
{
    public class ModelController
    {
        private static ModelController _instance;
        public static ModelController Instance
        {
            get
            {
                return (_instance ?? (_instance = new ModelController()));
            }
        }


        public void Connect(string IP, int port)
        {
            ConnectionManager.Instance.Connect(IP, port);
        }

        public void Listen(int port)
        {
            ConnectionManager.Instance.Start(port, 100);
        }

        public void SendMessage(string msg)
        {
            ConnectionManager.Instance.Broadcast(new MessagePacket(msg));
        }
    }
}
