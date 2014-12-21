using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    
  public class HashFoundPacket : Packet
    {
        private string _Password;

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                WriteString(value, HeaderSize);
            }
        }

        public HashFoundPacket(string password)
            : base((ushort)(HeaderSize + (password.Length)), 4)
        {
            this.Password = password;
        }

        public HashFoundPacket(byte[] buf)
            : base((ushort)buf.Length, 4)
        {
              _Password = Encoding.UTF8.GetString(buf, HeaderSize, buf.Length - Packet.HeaderSize);
        }
    }
}
