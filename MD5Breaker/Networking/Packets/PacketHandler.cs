using MD5Breaker.Core;
using MD5Breaker.Networking.Packets.PacketDataStructure;
using MD5Breaker.Networking.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MD5Breaker.Networking.Packets
{

    public static class PacketHandler
    {
        public static event ChatMessage OnMessageReceived;
        public static event ChatMessage OnHashFoundEvent;

        public static Packet Handle(Connection conn, byte[] packet)
        {
            ushort packetType;

            try
            {
                var obj = GenericSerializer.GetObject<InitBlocksData>(packet);
                packetType = obj.Type;
            }
            catch (Exception e)
            {
                packetType = BitConverter.ToUInt16(packet, 2);
            }
            
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
                    ProcessingBlockNotifyPacket pbnp = new ProcessingBlockNotifyPacket(packet);
                    pm.SetProcessingState(pbnp.BlockId, pbnp.State);
                    OnMessageReceived(string.Format("Block {0} | {1}.", pbnp.BlockId, pbnp.State.ToString()));
                    //MessageBox.Show(string.Format("Block {0} | {1}.", pbnp.BlockId, pbnp.State.ToString()));
                    break;

                case 4:
                    HashFoundPacket hfPacket = new HashFoundPacket(packet);
                    OnMessageReceived("Password found " + hfPacket.Password);
                    OnHashFoundEvent(hfPacket.Password);
                    pm.Initialized = false;
                    break;

                case 5:
                    InitBlocksPacket ibp = new InitBlocksPacket(packet);
                    
                    if (pm.Initialized)
                        return ibp;

                    OnMessageReceived("Initializing blocks...");
                    pm.Setup(ibp.MD5Hash, ibp.Range);
                    OnMessageReceived("Blocks Initialized.");
                    break;

                case 6:
                    BlockProcessPacket bpp = new BlockProcessPacket(packet);
                    break;
            }

            return null;
        }
    }
}
