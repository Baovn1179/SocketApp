using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocketApp
{
    internal class ClientApp
    {
       
        private static async Task Main(string[] args)
        {
            IPEndPoint ServerEP = new IPEndPoint(IPAddress.Loopback, 8000);
            Socket Client = new Socket(ServerEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            await Client.ConnectAsync(ServerEP);
            byte[] buffer = new byte[2048];
            while (true)
            {
                Console.Write("Chat >>> ");
                string sendmsg = Console.ReadLine();


                await Client.SendAsync(Encoding.UTF8.GetBytes(sendmsg));

                await Client.ReceiveAsync(buffer);
                string msg = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(msg);

                if (msg == "out")
                {
                    Console.WriteLine("Out khoi server");
                    Client.Shutdown(SocketShutdown.Both);
                    Client.Close();
                }
            }
        }

    }
}