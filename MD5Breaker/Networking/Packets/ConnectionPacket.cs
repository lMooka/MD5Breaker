using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public class ConnectionPacket : Packet
    {
        private string _IP;

        public string IP
        {
            get
            {
                return _IP;
            }
            private set
            {
                _IP = value;
                WriteString(value);
            }
        }

        public ConnectionPacket(string IP)
            : base((ushort)(HeaderSize + IP.Length), 2)
        {
            this.IP = IP;
        }
    }
}
