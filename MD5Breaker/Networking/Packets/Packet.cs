using MD5Breaker.Core.Exceptions;
using MD5Breaker.Networking.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public abstract class Packet
    {
        public static int HeaderSize = sizeof(ushort) * 2;
        public static int bufferMaxLength = 512;

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

        public Packet(byte[] buf)
            : this((ushort)buf.Length, 2)
        {
            buffer = buf;
        }

        protected void WriteUShort(ushort value, int offset)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, sizeof(ushort));
        }

        protected void WriteInt(int value, int offset)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, sizeof(int));
        }

        protected void WriteUInt(uint value, int offset)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, sizeof(int));
        }

        protected void WriteULong(ulong value, int offset)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(value), 0, buffer, offset, sizeof(ulong));
        }

        protected void WriteString(string msg, int offset)
        {
            Buffer.BlockCopy(Encoding.UTF8.GetBytes(msg), 0, buffer, offset, msg.Length);
        }

        protected void WriteObject(object obj, int offset)
        {
            var sbuffer = GenericSerializer.GetBinary(obj);

            if (sbuffer.Length > Packet.bufferMaxLength)
                throw new PacketOutOfLengthException();

            Buffer.BlockCopy(sbuffer, 0, buffer, offset, sbuffer.Length);
        }

        protected T ReadObject<T>(byte[] bytes, int offset, int count)
        {
            byte[] tmpbuf = new byte[count];

            Buffer.BlockCopy(bytes, offset, tmpbuf, 0, count);
            return GenericSerializer.GetObject<T>(bytes);
        }
    }
}
