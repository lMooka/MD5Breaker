using MD5Breaker.Networking.Packets;
using System;
using System.Collections.Generic;
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

        public Socket socket { get; private set; }

        public static int bufferSize = 512;
        byte[] buffer;

        public Connection(Socket socket)
        {
            this.socket = socket;
            buffer = new byte[bufferSize];
        }

        void ReceivedCallback(IAsyncResult result)
        {
            try
            {
                Socket clienteSocket = result.AsyncState as Socket;

                int bufSize = clienteSocket.EndReceive(result);

                byte[] buf = new byte[bufSize];
                Buffer.BlockCopy(buffer, 0, buf, 0, bufSize);

                //handle
                PacketHandler.Handle(buf);

                buffer = new byte[bufferSize];
                clienteSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallback, clienteSocket);
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
    }
}
