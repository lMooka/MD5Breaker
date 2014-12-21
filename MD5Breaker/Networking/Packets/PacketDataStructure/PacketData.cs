using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets.PacketDataStructure
{
    [Serializable()]
    public class PacketData
    {
        public ushort Length { get; set; }
        public ushort Type { get; set; }
    }
}
