using SWE1_MTCG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Server
{
    class Server
    {
        static void Main(string[] args)
        {
            Server.startServer();
        }
        static string receiveData(TcpClient client, NetworkStream stream)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];
            stream.Read(bytes, 0, (int)client.ReceiveBufferSize);
            string returndata = Encoding.UTF8.GetString(bytes);
            return returndata;
        }

        static void startServer()
        {
            new Thread(() =>
            {
                int port = 6543;

                Console.Write("Waiting for a connection... ");
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                MessageHandler messageHandler = new MessageHandler();
                listener.Start();

                while (true)
                {
                    if (listener.Pending())
                    {
                        new Thread(() =>
                        {
                            TcpClient client = listener.AcceptTcpClient();
                            Console.WriteLine("Connected!");
                            NetworkStream stream = client.GetStream();
                            string data = Server.receiveData(client, stream);
                            Console.WriteLine("SERVER RECEIVED:\n" + data);

                            //data verwalten und in ein Objekt speichern
                            RequestContext request = TCPClass.GetRequest(data);

                            //generelle eingabe überprüfen; vl noch entfernen
                            if ((TCPClass.GetPath(request.path) != "messages"))
                            {
                                Console.WriteLine("Wrong Path");
                                return; //noch int hinzu oderso
                            }

                            messageHandler.ManageMessages(request);
                        }).Start();
                    }
                }
            }).Start();
        }
    }
}


