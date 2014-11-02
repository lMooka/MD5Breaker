using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public static class PacketHandler
    {
        public static void Handle(byte[] packet)
        {
            ushort packetType = BitConverter.ToUInt16(packet, 2);

            switch (packetType)
            {
                case 1:
                    Message msg = new Message(Encoding.UTF8.GetString(packet, 4, packet.Length - Packet.HeaderSize));
                    Console.WriteLine(msg.message);
                    break;
            }
        }
    }
}
