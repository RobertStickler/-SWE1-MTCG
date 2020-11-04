using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 6543;
            IPAddress ip = IPAddress.Parse("127.0.0.1");

            Socket clientSock = new Socket
            (
               AddressFamily.InterNetwork,     
               SocketType.Stream,              
                                               
               ProtocolType.Tcp
            );

            clientSock.Connect(new IPEndPoint(ip, port));
            Console.WriteLine("Client connected!");

            while(true)
            {
                Console.WriteLine("Enter message:");
                string msgClient = Console.ReadLine();

                byte[] msgClientByte = new byte[1024];
                //msg = Encoding.ASCII.GetString(messageClient);
                msgClientByte = Encoding.ASCII.GetBytes(msgClient);

                //clientSock.Send(System.Text.Encoding.ASCII.GetBytes(msgClient), 0, msgClient.Length, SocketFlags.None);
                clientSock.Send(msgClientByte, 0, msgClientByte.Length, SocketFlags.None);

                byte[] msgServerByte = new byte[1024];
                int size = clientSock.Receive(msgServerByte);
                string msgServer = Encoding.ASCII.GetString(msgServerByte, 0, size);
                Console.WriteLine("Server response:\n" + msgServer + "\n");
                //Console.WriteLine("Server:\n", System.Text.Encoding.ASCII.GetString(msgServerByte,0,size));

            }



        }

    }
}
