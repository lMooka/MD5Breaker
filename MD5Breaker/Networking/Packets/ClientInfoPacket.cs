using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public class ClientInfoPacket : Packet
    {
        private int _ClientID;
        private ushort _ListenPort;

        public int ClientID
        {
            get
            {
                return _ClientID;
            }
            set
            {
                _ClientID = value;
                WriteInt(value, HeaderSize);
            }
        }
        public ushort ListenPort
        {
            get
            {
                return _ListenPort;
            }
            set
            {
                _ListenPort = value;
                WriteUShort(value, HeaderSize + sizeof(int));
            }
        }

        public ClientInfoPacket(int clientID, ushort listenPort)
            : base((ushort)(HeaderSize + sizeof(int) + sizeof(ushort)), 0)
        {
            this.ClientID = clientID;
            this.ListenPort = listenPort;
        }

        public ClientInfoPacket(byte[] buf)
            : base((ushort)buf.Length, 0)
        {
            _ClientID = BitConverter.ToInt32(buf, HeaderSize);
            _ListenPort = BitConverter.ToUInt16(buf, HeaderSize + sizeof(int));
        }
    }
}
