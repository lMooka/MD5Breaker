using MD5Breaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public class InitBlocksPacket : Packet
    {

        private short _lengthStart;
        private short _lengthCurrent;
        private short _lengthEnd;

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

        public InitBlocksPacket(DecrypterRange range)
            : base((ushort)(HeaderSize), 5)
        {
            this.Range = range;
            
            WriteObject(range, HeaderSize);
        }

        public InitBlocksPacket(byte[] buf)
            : base((ushort)buf.Length, 5)
        {
            Range = ReadObject<DecrypterRange>(buf);
        }
    }
}
