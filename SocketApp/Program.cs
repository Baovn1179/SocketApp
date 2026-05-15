using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocketApp
{
    internal class ServerApp
    {
        static private List<Socket> Clients = null;
        static IPEndPoint ServerEP = null;
        static Socket Server = null;
        private static async Task Main(string[] args)
        {
            Clients = new List<Socket>();
            ServerEP = new IPEndPoint(IPAddress.Any, 8000);
            Server = new Socket(ServerEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            Server.Bind(ServerEP);
            Server.Listen(100);

            while (true)
            {
                Socket Client = await Server.AcceptAsync();
                new Thread(async () =>
                {
                    await HandleClient(Client);
                }).Start();
            }
        }

        private static async Task HandleClient(Socket Client)
        {
            Clients.Add(Client);
            Log($"{Client.RemoteEndPoint} has joined to the server.");
            await Broadcast($"{Client.RemoteEndPoint} has joined to the server.");


            await SendMsg(Client, $"Server[{Server.LocalEndPoint}] >>> Chao mung ban da den voi server");

            byte[] buffer = new byte[2048];

            while (true)
            {
                int r = await Client.ReceiveAsync(buffer);
                Console.WriteLine(r);
                string msg = Encoding.UTF8.GetString(buffer);
                if (r == 0)
                {
                    await Broadcast($"Nguoi dung {Client.RemoteEndPoint} da roi khoi server.");
                    for (int idx = 0; idx < Clients.Count; idx++)
                    {
                        Socket c = Clients[idx];
                        if (c.RemoteEndPoint == Client.RemoteEndPoint)
                        {
                            await SendMsg(Client, "out");
                            Clients.RemoveAt(idx);
                            break;
                        }
                    }
                } 

                    
            }
        } 

        private static async Task Broadcast(string msg)
        {
            if (Clients.Count > 0)
            {
                foreach (Socket Client in Clients)
                {
                    await Client.SendAsync(Encoding.UTF8.GetBytes(msg));
                }
            }
        }

        private static async Task SendMsg(Socket Client, string msg)
        {
            await Client.SendAsync(Encoding.UTF8.GetBytes(msg));
        }

        private static void Log(string msg)
        {
            Console.WriteLine($"System[{Server.LocalEndPoint}] >>> {msg}");
        }

    }
}