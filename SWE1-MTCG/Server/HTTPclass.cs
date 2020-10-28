using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class MyTcpListener
    {

        static void receiveData(TcpClient client, NetworkStream stream)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];
            stream.Read(bytes, 0, (int)client.ReceiveBufferSize);
            string returndata = Encoding.UTF8.GetString(bytes);
            Console.WriteLine("SERVER RECEIVED: " + returndata);
        }

        public void startServer()
        {
            new Thread(() =>
            {
                TcpListener listener = new TcpListener(IPAddress.Any, 6543);
                listener.Start();

                while (true)
                {
                    if (listener.Pending())
                    {
                        new Thread(() =>
                        {
                            TcpClient client = listener.AcceptTcpClient();
                            NetworkStream stream = client.GetStream();
                            MyTcpListener.receiveData(client, stream);
                        }).Start();
                    }
                }
            }).Start();
        }
    }
}
