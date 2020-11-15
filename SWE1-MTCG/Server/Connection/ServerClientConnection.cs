﻿using System;
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
    public class ServerClientConnection
    {
        public static string receiveData(TcpClient client, NetworkStream stream)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];
            stream.Read(bytes, 0, (int)client.ReceiveBufferSize);
            string returndata = Encoding.UTF8.GetString(bytes);
            return returndata;
        }
        public static void sendData(NetworkStream stream, string message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.Write("Sent:\n{0}", message);
        }


        public static void startServer()
        {
            List<RequestContext> clientList = new List<RequestContext>();

            int port = 6543;

            Console.Write("Waiting for a connection... ");
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            MessageHandler messageHandler = new MessageHandler();
            listener.Start();

            try
            {
                while (true)
                {

                    if (listener.Pending())
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        new Thread(() =>
                        {

                            Console.WriteLine("Connected!");
                            NetworkStream stream = client.GetStream();
                            string data = ServerClientConnection.receiveData(client, stream);
                            Console.WriteLine("SERVER RECEIVED:\n" + data);

                            //data verwalten und in ein Objekt speichern
                            RequestContext request = TCPClass.GetRequest(data);
                            string sieger = "noOne";


                            if (request.message == "Login")
                            {
                                //check if login   
                                int attempt = 3; //muss ich noch hinzufügen
                                bool temp = false;
                                do
                                {
                                    temp = DbFunctions.VerifyLogin(request, stream);
                                    attempt--;
                                    if(temp == false)
                                    {
                                        //client antworten und pw und user neu eingeben
                                    }

                                }
                                while ((attempt > 0 && temp == false));
                                if (attempt == 0)
                                {
                                    throw new ArgumentException("Password entered incorrectly too often");
                                }
                            }
                            else if (request.message == "Register")
                            {
                                //setup for register
                            }

                            //brauch ich dann auch für später
                            if (request.message == "StartTheBattle")
                            {
                                request.cardDeck = BattleMaker.GetRandCards();
                                clientList.Add(request);

                                //noch lock hinzufügen
                                while (sieger == "noOne")
                                {
                                    sieger = BattleMaker.AddToBattleQueue(clientList);
                                    Thread.Sleep(1000);
                                }
                                Console.WriteLine("And the winner is: {0}", sieger);


                            }
                        }).Start();
                    }
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("try another login!");
            }
        }


    }
}
