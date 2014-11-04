using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public abstract class Packet
    {
        public static int HeaderSize = 4;

        public byte[] Data
        {
            get { return buffer; }
        }

        protected byte[] buffer;

        public Packet(ushort length, ushort type)
        {
            buffer = new byte[length];
            WriteUShort(length, 0);
            WriteUShort(type, 2);
        }

        public void WriteUShort(ushort value, int offset)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 2);
        }

        public void WriteInt(ushort value, int offset)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, 4);
        }

        protected void WriteString(string msg)
        {
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(msg), 0, buffer, HeaderSize, msg.Length);
        }
    }
}
