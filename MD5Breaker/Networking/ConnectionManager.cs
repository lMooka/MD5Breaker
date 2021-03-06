﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MD5Breaker.Networking.Packets;
using System.Diagnostics;

namespace MD5Breaker.Networking
{
    public delegate void ProblemReporter(Exception e);
    public delegate void ClientConnectedEvent(Connection connection);

    public class ConnectionManager
    {
        public int ClientID;
        public ushort ListenPort;

        // Singleton
        private static ConnectionManager _instance;
        public static ConnectionManager Instance
        {
            get
            {
                return (_instance ?? (_instance = new ConnectionManager()));
            }
        }

        // Events
        public event ProblemReporter ProblemReportEvent;
        public event ClientConnectedEvent ClientConnected;

        private List<Connection> Connections;
        private Socket serverSocket;
        private Socket clientSocket;

        public ConnectionManager()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Connections = new List<Connection>();

            ClientID = (Environment.MachineName + Process.GetCurrentProcess().Id).GetHashCode();
        }

        public void Start(int port, int backlog)
        {
            try
            {
                Bind(port);
                Listen(backlog);
                Accept();
            }
            catch (Exception e)
            {
                ProblemReportEvent(e);
            }
        }

        public void Bind(int port)
        {
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            this.ListenPort = (ushort)port;
        }

        public void Listen(int backlog)
        {
            serverSocket.Listen(backlog);
        }

        public void Accept()
        {
            try
            {
                serverSocket.BeginAccept(new AsyncCallback(AcceptedCallback), serverSocket);
            }
            catch (Exception e)
            {
                ProblemReportEvent(e);
            }
        }

        void AcceptedCallback(IAsyncResult result)
        {
            Socket connSocket = serverSocket.EndAccept(result);

            Connection conn = new Connection(connSocket);
            conn.Activate();

            AddConnection(conn);
            Accept();
        }

        void AddConnection(Connection conn)
        {
            Connections.Add(conn);
            conn.ConnectionClosed += ConnectionProblem;

            conn.Send(new ClientInfoPacket(this.ClientID, this.ListenPort));

            ClientConnected(conn);
        }

        void RemoveConncection(Connection conn)
        {
            Connections.Remove(conn);
        }

        // cliente-side
        public void Connect(string ip, int port)
        {
            try
            {
                //Console.WriteLine("Conectado a " + ip + ":" + port + ".");
                clientSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectedCallback, clientSocket);
            }
            catch (Exception e)
            {
                ProblemReportEvent(e);
            }
        }

        void ConnectedCallback(IAsyncResult result)
        {
            Socket connSocket = result.AsyncState as Socket;
            Connection conn = new Connection(connSocket);

            try
            {
                conn.Activate();
                AddConnection(conn);
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            catch (Exception ex)
            {
                ProblemReportEvent(ex);
                //conn.Dispose();
            }
        }

        public Connection GetConnection(int clientID)
        {
            foreach (Connection c in Connections)
            {
                if (c.RemoteClientID == clientID)
                    return c;
            }

            return null;
        }

        private void ConnectionProblem(Connection connection, Exception e)
        {
            Connections.Remove(connection);
            //connection.Dispose();
            ProblemReportEvent(e);
        }

        public void Broadcast(Packet packet)
        {
            foreach (Connection conn in Connections.ToList())
            {
                if (conn.socket.Connected)
                    conn.socket.Send(packet.Data);
            }
        }

        public void SendPacket(Connection conn, Packet packet)
        {
            if (conn.socket.Connected)
                conn.socket.Send(packet.Data);
        }
    }
}
