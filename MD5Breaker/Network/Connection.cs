using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Network
{
    public class Connection
    {
        Socket socket;

        public static int bufferSize = 512;
        byte[] buffer;

        public Connection(Socket socket)
        {
            this.socket = socket;
            buffer = new byte[bufferSize];
        }

        void ReceivedCallback(IAsyncResult result)
        {
            Socket clienteSocket = result.AsyncState as Socket;

            int bufSize = clienteSocket.EndReceive(result);

            byte[] buf = new byte[bufSize];
            Buffer.BlockCopy(buffer, 0, buf, 0, buffer.Length);

            //handle

            buffer = new byte[bufferSize];
            clienteSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallback, clienteSocket);
        }

        public void Activate()
        {
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceivedCallback, socket);
        }
    }
    

}
