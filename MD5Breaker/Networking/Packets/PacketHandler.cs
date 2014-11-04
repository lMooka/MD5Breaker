using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public delegate void PacketNotifierEvent(Packet packet);

    public static class PacketHandler
    {
        public static event PacketNotifierEvent OnPacketMessageReceived;

        public static void Handle(byte[] packet)
        {

            ushort packetType = BitConverter.ToUInt16(packet, 2);

            switch (packetType)
            {
                case 1:
                    MessagePacket mPacket = new MessagePacket(packet);
                    if (OnPacketMessageReceived != null)
                        OnPacketMessageReceived(mPacket);
                    break;

                case 2:
                    ConnectionPacket cPacket = new ConnectionPacket(packet);
                    var cm = ConnectionManager.Instance;

                    if (cm.GetConnection(cPacket.ConnHash) == null)
                        cm.Connect(cPacket.IP, cPacket.Port);

                    break;
            }
        }
    }
}
