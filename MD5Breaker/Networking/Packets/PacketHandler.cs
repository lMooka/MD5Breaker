﻿using System;
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

        public static Packet Handle(Connection conn, byte[] packet)
        {

            ushort packetType = BitConverter.ToUInt16(packet, 2);
            var cm = ConnectionManager.Instance;

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
            }

            return null;
        }
    }
}