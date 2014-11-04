using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    /* Packet 2
     * 
     * Envia uma ordem de conexão para o cliente.
     * Qualquer cliente que receber este Packet irá tentar conectar-se ao cliente ordenado.
     */
    public class ConnectionPacket : Packet
    {
        private int _connHash;
        private string _IP;
        private ushort _Port;

        public ushort ConnHash
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
                WriteUShort(value, (ushort)HeaderSize);
            }
        }
        public ushort Port
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
                WriteUShort(value, (ushort)HeaderSize + sizeof(int));
            }
        }
        public string IP
        {
            get
            {
                return _IP;
            }
            private set
            {
                _IP = value;
                WriteString(_IP, HeaderSize + sizeof(int) + sizeof(ushort));
            }
        }



        public ConnectionPacket(string IP, ushort port)
            : base((ushort)(HeaderSize + sizeof(int) + sizeof(ushort) + IP.Length), 2)
        {
            this.IP = IP;
            this.Port = port;
        }

        public ConnectionPacket(byte[] buf)
            : base((ushort)buf.Length, 2)
        {
                _connHash = BitConverter.ToInt32(buf, HeaderSize);
                _Port = BitConverter.ToUInt16(buf, HeaderSize + sizeof(int));
                _IP = Encoding.UTF8.GetString(buf, HeaderSize + sizeof(int) + sizeof(ushort), buf.Length - HeaderSize - sizeof(int) - sizeof(ushort));
        }
    }
}
