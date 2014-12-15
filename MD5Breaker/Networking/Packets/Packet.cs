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
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, obj);
            byte[] serialBuf = ms.ToArray();

            Buffer.BlockCopy(serialBuf, 0, buffer, offset, serialBuf.Length);
        }

        protected T ReadObject<T>(byte[] bytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();

            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);

            return (T)binForm.Deserialize(memStream);
        }
    }
}
