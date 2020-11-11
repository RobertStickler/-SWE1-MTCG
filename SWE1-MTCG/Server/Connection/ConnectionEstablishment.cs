using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using SWE1_MTCG;
using Cards;
using System.Runtime.Serialization.Formatters;

namespace Server
{
    public class ConnectionEstablishment
    {        
        public static string receiveData(TcpClient client, NetworkStream stream)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];
            stream.Read(bytes, 0, (int)client.ReceiveBufferSize);
            string returndata = Encoding.UTF8.GetString(bytes);
            return returndata;
        }
        public static void startServer()
        {
            List<RequestContext> clientList = new List<RequestContext>();

            int port = 6543;

            Console.Write("Waiting for a connection... ");
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            MessageHandler messageHandler = new MessageHandler();
            listener.Start();


            while (true)
            {              

                if (listener.Pending())
                {
                    TcpClient client = listener.AcceptTcpClient();
                    new Thread(() =>
                    {

                        Console.WriteLine("Connected!");
                        NetworkStream stream = client.GetStream();
                        string data = ConnectionEstablishment.receiveData(client, stream);
                        Console.WriteLine("SERVER RECEIVED:\n" + data);

                        //data verwalten und in ein Objekt speichern
                        RequestContext request = TCPClass.GetRequest(data);  
                        string sieger = "noOne";
            

                        if (request.message == "StartTheBattle")
                        {
                            request.cardDeck = BattleMaker.GetRandCards();
                            clientList.Add(request);

                            while (sieger == "noOne")
                            { 
                                sieger = BattleMaker.AddToBattleQueue(clientList);                                
                            }
                            Console.WriteLine("And the winner is: {0}", sieger);
                            clientList.RemoveAt(0);

                        }
                    }).Start();
                }
            }
        }
    }
}
