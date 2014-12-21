using MD5Breaker.Core;
using MD5Breaker.Networking.Packets.PacketDataStructure;
using MD5Breaker.Networking.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public class InitBlocksPacket : Packet
    {
        protected InitBlocksData PData { get; set; }

        //private string _MD5Hash;
        public string MD5Hash
        {
            get
            {
                return PData.Hash;
            }
            private set
            {
                PData.Hash = value.Substring(0, 32);
                WriteObject(PData, 0);
            }
        }
        //private DecrypterRange _Range;
        public DecrypterRange Range
        {
            get
            {
                return PData.Range;
            }
            set
            {
                PData.Range = value;
                WriteObject(PData, 0);
            }
        }

        //public InitBlocksPacket(string hash, DecrypterRange range)
        //    : base((ushort)(HeaderSize + 32 + GenericSerializer.GetByteLength(range)), 5)
        //{
        //    this.Range = range;
        //    this.MD5Hash = hash;
        //}

        //public InitBlocksPacket(byte[] buf)
        //    : base((ushort)buf.Length, 5)
        //{
        //    this.MD5Hash = Encoding.UTF8.GetString(buf, HeaderSize, 32);
        //    this.Range = ReadObject<DecrypterRange>(buf, (HeaderSize + 32), buf.Length - (HeaderSize + 32));
        //}

        public InitBlocksPacket(string hash, DecrypterRange range)
            //: base((ushort)(1), 5)
        {
            var tmpdata = new InitBlocksData();

            tmpdata.Type = 5;
            tmpdata.Hash = hash;
            tmpdata.Range = range;

            buffer = new byte[GenericSerializer.GetByteLength(tmpdata)];
            PData = tmpdata;
            WriteObject(PData, 0);
        }

        public InitBlocksPacket(byte[] buf)
            : base(buf)
        {
            //this.MD5Hash = Encoding.UTF8.GetString(buf, HeaderSize, 32);
            //this.Range = ReadObject<DecrypterRange>(buf, HeaderSize, buf.Length - HeaderSize);
            this.buffer = buf;
            this.PData = ReadObject<InitBlocksData>(buf, 0, buf.Length);
        }
    }
}
