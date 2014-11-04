using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MD5Breaker.Networking;
using MD5Breaker.Networking.Packets;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var cm = ConnectionManager.Instance;

            cm.ProblemReportEvent += cm_ProblemReportEvent;
            cm.ClientConnected += cm_ClientConnected;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("-- @help para lista de commandos\n\n");

            while (true)
            {
                Color(ConsoleColor.Gray);
                Console.Write("@");
                Color(ConsoleColor.Green);

                string command = Console.ReadLine();

                if (command.Contains("connect"))
                {
                    string[] param = command.Split(' ');

                    try
                    {
                        cm.Connect(param[1], Convert.ToInt32(param[2]));
                        ColorMessage("Connectando em " + param[1] + ".", ConsoleColor.DarkYellow);
                    }
                    catch (SystemException e)
                    {
                        Color(ConsoleColor.Red);
                        Console.WriteLine("Use: connect <IP> <Porta>\n" + e.Message);
                        continue;
                    }
                }
                else if (command.Contains("send"))
                {
                    command += "\\";
                    string[] param = command.Split('\\');

                    try
                    {
                        cm.Broadcast(new MessagePacket(param[1]));
                    }
                    catch (SystemException e)
                    {
                        Console.WriteLine("Use: send <msg>\n" + e.Message);
                        continue;
                    }
                }
                else if (command.Contains("listen"))
                {
                    string[] param = command.Split(' ');

                    try
                    {
                        cm.Start(Convert.ToInt32(param[1]), 100);
                        ColorMessage("Escutando conexões na porta " + param[1], ConsoleColor.DarkYellow);
                    }
                    catch (SystemException e)
                    {
                        ColorMessage("Use: listen <Porta>\n" + e.Message, ConsoleColor.Red);
                        continue;
                    }
                }
                else
                {
                    ColorMessage("Comando Inválido", ConsoleColor.Red);
                }
            }
        }

        static void cm_ClientConnected(Connection connection)
        {
            ColorMessage(connection.socket.RemoteEndPoint.ToString() + " conectou-se.", ConsoleColor.DarkYellow);
            Color(ConsoleColor.Gray);
            Console.Write("@");
        }

        static void cm_ProblemReportEvent(Exception e)
        {
            ColorMessage("Ocorreu um erro: " + e.Message, ConsoleColor.Red);
            Color(ConsoleColor.Gray);
            Console.Write("@");

        }

        static void Color(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        static void ColorMessage(string msg, ConsoleColor color)
        {
            ConsoleColor currentColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.Write(msg + "\n");
            Console.ForegroundColor = currentColor;
        }
    }
}
