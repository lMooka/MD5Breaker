using MD5Breaker.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets.PacketDataStructure
{
    [Serializable()]
    public class InitBlocksData : PacketData
    {
        public string Hash;
        public DecrypterRange Range;
    }
}
