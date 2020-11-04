using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 6543;
            IPAddress ip = IPAddress.Parse("127.0.0.1");

            Socket serverSock = new Socket
            (
                AddressFamily.InterNetwork,     // IPv4
                SocketType.Stream,              // Stream sends byte array based data
                                                // and supports reliable 2way communication
                ProtocolType.Tcp                // TCP
            );              


            //IPEndPoint endPoint = new IPEndPoint(ip, port);
            serverSock.Bind(new IPEndPoint(ip, port));
            serverSock.Listen(5);

            Console.WriteLine("Server is listening.....\n");

            Socket clientSock = default(Socket);
            int counter = 0;
            Program p = new Program();

            while(true)
            {
                counter++;
                clientSock = serverSock.Accept();
                Console.WriteLine(counter + " Clients connected");
                Thread UserThread = new Thread(new ThreadStart(() => p.User(clientSock)));
                UserThread.Start();
            }

           
        }
        public void User(Socket client)
        {
            while(true)
            {
                byte[] msg = new byte[1024];
                int size = client.Receive(msg);
                client.Send(msg, 0, size, SocketFlags.None);
            }
        }
    }
}
