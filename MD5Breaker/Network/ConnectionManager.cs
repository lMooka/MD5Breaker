using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Network
{
    public class ConnectionManager
    {
        Socket serverSocket;
        Socket clientSocket;

        List<Connection> Connections;

        public ConnectionManager()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Connections = new List<Connection>();
        }

        public void Start(int port, int backlog)
        {
            Bind(port);
            Listen(backlog);
            Accept();
        }

        public void Bind(int port)
        {
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        public void Listen(int backlog)
        {
            serverSocket.Listen(backlog);
        }

        public void Accept()
        {
            serverSocket.BeginAccept(new AsyncCallback(AcceptedCallback), serverSocket);
        }

        void AcceptedCallback(IAsyncResult result)
        {
            Socket connSocket = serverSocket.EndAccept(result);

            Console.WriteLine("Um cliente conectou-se: " + connSocket.GetHashCode());
            Connection conn = new Connection(connSocket);
            Connections.Add(conn);
            conn.Activate();
            Accept();
        }

        // Client-Side

        public void Connect(string ip, int port)
        {
            Console.WriteLine("Conectado a " + ip + ":" + port + ".");
            clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectedCallback, clientSocket);
        }

        void ConnectedCallback(IAsyncResult result)
        {
            Socket connSocket = result.AsyncState as Socket;

            Connection conn = new Connection(connSocket);
            Connections.Add(conn);
            conn.Activate();
        }
    }
}
