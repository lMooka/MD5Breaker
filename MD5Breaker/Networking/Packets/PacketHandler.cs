using MD5Breaker.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Packets
{
    public delegate void PacketNotifierEvent(string message);


    public static class PacketHandler
    {
        public static event PacketNotifierEvent OnMessageReceived;
        public static event PacketNotifierEvent OnHashFoundEvent;

        public static Packet Handle(Connection conn, byte[] packet)
        {            
            ushort packetType = BitConverter.ToUInt16(packet, 2);
            var cm = ConnectionManager.Instance;
            var pm = ProcessingManager.Instance;

            switch (packetType)
            {
                case 0:
                    var ciPacket = new ClientInfoPacket(packet);
                    conn.RemoteClientID = ciPacket.ClientID;

                    string ip = conn.socket.RemoteEndPoint.ToString().Split(':')[0];
                    ushort port = ciPacket.ListenPort;

                    cm.Broadcast(new ConnectionPacket(ciPacket.ClientID, ip, port));
                    return ciPacket;

                case 1:
                    MessagePacket mPacket = new MessagePacket(packet);
                    if (OnMessageReceived != null)
                        OnMessageReceived(mPacket.Message);
                    break;

                case 2:
                    ConnectionPacket cPacket = new ConnectionPacket(packet);
                    OnMessageReceived(string.Format("packet received: {0}:{1}", cPacket.IP, cPacket.Port));

                    if (cm.GetConnection(cPacket.ClientID) == null && cm.ClientID != cPacket.ClientID)
                    {
                        //OnMessageReceived(string.Format("trying to connect: {0}:{1}", cPacket.IP, cPacket.Port));
                        cm.Connect(cPacket.IP, cPacket.Port);
                    }
                    break;

                case 3:
                    ProcessingBlockNotificationPacket pbnp = new ProcessingBlockNotificationPacket(packet);
                    pm.SetProcessingState(pbnp.BlockId, pbnp.State);
                    break;

                case 4:
                    HashFoundPacket hfPacket = new HashFoundPacket(packet);
                    OnHashFoundEvent(hfPacket.Password);
                    break;

                case 5:
                    InitBlocksPacket ibp = new InitBlocksPacket(packet);
                    ProcessingManager.Instance.SetRange(ibp.Range);
                    OnMessageReceived(ibp.Range.endRange.ToString());
                    break;
            }

            return null;
        }
    }
}
