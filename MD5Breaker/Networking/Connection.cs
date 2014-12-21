using MD5Breaker.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking
{
    public delegate void ConnectionLostEvent(Connection connection, Exception e);

    public class Connection : IDisposable
    {
        public event ConnectionLostEvent ConnectionClosed;
        public int RemoteClientID { get; set; }
        public Socket socket { get; private set; }
        byte[] buffer;

        public Connection(Socket socket)
        {
            this.socket = socket;
            buffer = new byte[Packet.bufferMaxLength];
        }

        void ReceivedCallback(IAsyncResult result)
        {
            try
            {
                Socket clientSocket = result.AsyncState as Socket;

                int bufSize = clientSocket.EndReceive(result);

                byte[] buf = new byte[bufSize];
                Buffer.BlockCopy(buffer, 0, buf, 0, bufSize);

                //handle
                PacketHandler.Handle(this, buf);

                buffer = new byte[Packet.bufferMaxLength];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallback, clientSocket);
            }
            catch (Exception e)
            {
                ConnectionClosed(this, e);
            }
        }

        public void Activate()
        {
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallback, socket);
        }

        public void Dispose()
        {
            ConnectionClosed = null;
            socket.Dispose();
            this.Dispose();
        }

        public void Send(Packet packet)
        {
            socket.Send(packet.Data);
        }
    }
}
