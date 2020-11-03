using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

class Server
{
    static void Main(string[] args)
    {
        Server.startServer();
    }

    static void receiveData(TcpClient client, NetworkStream stream)
    {
        byte[] bytes = new byte[client.ReceiveBufferSize];
        stream.Read(bytes, 0, (int)client.ReceiveBufferSize);
        string returndata = Encoding.UTF8.GetString(bytes);
        Console.WriteLine("SERVER RECEIVED:\n" + returndata);
    }

    static void startServer()
    {
        new Thread(() =>
        {
            Console.Write("Waiting for a connection... ");
            TcpListener listener = new TcpListener(IPAddress.Any, 6543);
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
                        Server.receiveData(client, stream);
                    }).Start();
                }
            }
        }).Start();
    }


}


