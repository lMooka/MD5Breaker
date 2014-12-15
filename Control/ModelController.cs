using MD5Breaker.Core;
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

        public void CrackHash(string hash, uint min, uint max)
        {
            int i;
            uint[] start = new uint[min];
            uint[] end = new uint[max];

            for (i = 0; i < min; i++)
                start[i] = 0;

            for (i = 0; i < max; i++)
                end[i] = Convert.ToUInt32(MD5Decrypter.CharRange.Length);

            ProcessingManager.Instance.InitBlocks(new DecrypterRange(start, end, Convert.ToUInt32(MD5Decrypter.CharRange.Length)));
            ProcessingManager.Instance.Crack(hash);
        }
    }
}
