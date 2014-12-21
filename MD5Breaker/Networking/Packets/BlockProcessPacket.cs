using MD5Breaker.Core;
using MD5Breaker.Networking.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public class BlockProcessPacket : Packet
    {
        private DecrypterRange _Range;
        public DecrypterRange Range
        {
            get
            {
                return _Range;
            }
            set
            {
                _Range = value;
                WriteObject(value, HeaderSize);
            }
        }

        public BlockProcessPacket(DecrypterRange range)
            : base((ushort)(HeaderSize + GenericSerializer.GetByteLength(range)), 6)
        {
            this.Range = range;
        }

        public BlockProcessPacket(byte[] buf)
            : base(buf)
        {
            Range = ReadObject<DecrypterRange>(buf, HeaderSize, buf.Length - HeaderSize);
        }
    }
}
