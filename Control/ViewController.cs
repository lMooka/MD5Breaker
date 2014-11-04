using MD5Breaker.Networking;
using MD5Breaker.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Control
{
    public delegate void MessageReceivedEvent(string Message);

    public class ViewController
    {
        private static ViewController _instance;
        public static ViewController Instance
        {
            get
            {
                return (_instance ?? (_instance = new ViewController()));
            }
        }

        public MessageReceivedEvent OnMessageReceived;

        public ViewController()
        {
            Init();
        }

        public void Init()
        {
            var cm = ConnectionManager.Instance;

            cm.ProblemReportEvent += cm_ProblemReportEvent;
            cm.ClientConnected += cm_ClientConnected;
            PacketHandler.OnPacketMessageReceived += PacketHandler_OnPacketMessageReceived;
        }

        void PacketHandler_OnPacketMessageReceived(Packet packet)
        {
            if (packet is MessagePacket)
                OnMessageReceived(string.Format((packet as MessagePacket).Message));
        }

        private void cm_ClientConnected(Connection connection)
        {
            OnMessageReceived(string.Format("{0} conectou-se.", connection.socket.RemoteEndPoint.ToString()));
        }

        private void cm_ProblemReportEvent(Exception e)
        {
            OnMessageReceived(string.Format("Error: {0}", e.Message));
        }
    }
}
