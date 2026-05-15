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

            while (true)
            {

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