using MD5Breaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public class BlockProcessPacket : Packet
    {
        private uint endOffset;
        private uint currentOffset;
        public DecrypterRange DecryRange;

        public BlockProcessPacket(DecrypterRange decryRange)
            : base((ushort)(HeaderSize + (3 * sizeof(uint)) + (decryRange.startRange.Length * sizeof(uint) + decryRange.endRange.Length * sizeof(uint) + decryRange.currentRange.Length * sizeof(uint))), 6)
        {
            this.DecryRange = decryRange;
            endOffset = (uint)decryRange.startRange.Length;
            currentOffset = (uint)(endOffset + decryRange.endRange.Length);

            WriteUInt(decryRange.charOffset, HeaderSize);
            WriteUInt(endOffset, HeaderSize + sizeof(uint));
            WriteUInt(currentOffset, HeaderSize + 2 * sizeof(uint));

            int counter = 0;
            foreach (uint value in decryRange.startRange)
                WriteUInt(value, HeaderSize + counter++ * sizeof(uint));

            foreach (uint value in decryRange.endRange)
                WriteUInt(value, HeaderSize + counter++ * sizeof(uint));

            foreach (uint value in decryRange.currentRange)
                WriteUInt(value, HeaderSize + counter++ * sizeof(uint));
        }

        public BlockProcessPacket(byte[] buf)
            : base(buf)
        {
            
        }
    }
}
