using MD5Breaker.Core;
using MD5Breaker.Networking.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public class BlockProcessPacket2 : Packet
    {
        public DecrypterRange Range;

        public BlockProcessPacket2(DecrypterRange range)
            : base((ushort)(HeaderSize + sizeof(ulong) * 2 + sizeof(uint)), 6)
        {
            WriteObject(range, HeaderSize);
        }

        public BlockProcessPacket2(byte[] buf)
            : base(buf)
        {
            Range = ReadObject<DecrypterRange>(buf, HeaderSize, buf.Length - HeaderSize);
        }
    }
}
