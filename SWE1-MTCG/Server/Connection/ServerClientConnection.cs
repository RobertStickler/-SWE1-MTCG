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
    public class ServerClientConnection
    {
        private static Mutex mut = new Mutex();
        public static void startServer()
        {
            List<RequestContext> clientList = new List<RequestContext>();
            int port = 6543;
            Console.Write("Waiting for a connection... ");
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            try
            {
                while (true) //akzeptiert alle clients die kommen
                {

                    if (listener.Pending())
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        string data = "";


                        new Thread(() =>
                        {
                            Console.WriteLine("\n Client Connected!");
                            string sieger = "noOne";
                            bool loggedIn = false;
                            DbUser userFromDb = new DbUser();
                            NetworkStream stream = null;
                            RequestContext request = new RequestContext();
                            MySqlDataClass mySqlDataClass = new MySqlDataClass();

                            while (loggedIn == false)
                            {
                                stream = client.GetStream();
                                data = ServerClientConnection.receiveData(client, stream);
                                Console.WriteLine("SERVER RECEIVED:\n" + data);

                                //data verwalten und in ein Objekt speichern
                               request = MessageHandler.GetRequest(data);

                                if (request.message == "Login")
                                {
                                    //check if login   
                                    int attempt = 3; //muss ich noch hinzufügen
                                    do
                                    {
                                        userFromDb = DbFunctions.VerifyLogin(request, stream);                                        
                                        if (userFromDb == null)
                                        {
                                            attempt--;
                                            //client antworten und pw und user neu eingeben
                                            string message = "please try again";
                                            // Translate the Message into ASCII.
                                            Byte[] response = System.Text.Encoding.ASCII.GetBytes(message);
                                            // Send the message to the connected TcpServer. 
                                            stream.Write(response, 0, response.Length);
                                        }
                                        else
                                        {
                                            loggedIn = true;
                                        }

                                    }
                                    while ((attempt > 0) && (loggedIn == false));                                    
                                    if (attempt == 0)
                                    {
                                        throw new ArgumentException("Password entered incorrectly too often");
                                    }

                                    //wieder auf nachricht warten
                                    //er ist nun eingeloggt
                                }
                                else if (request.message == "Register")
                                {
                                    //setup for register
                                    DbFunctions.RegisterAtDB(request, stream);
                                }
                            }

                            data = ServerClientConnection.receiveData(client, stream);
                            Console.WriteLine("SERVER RECEIVED:\n" + data);
                            //daten wieder einlesen
                            request = MessageHandler.GetRequest(data);

                            //also nach dem einloggen, kann ein client man hier her
                            //brauch ich dann auch für später
                            if (request.message == "StartTheBattle")
                            {
                                Console.WriteLine("Das battle beginnt in kürze");
                                //statt den rand card muss ich jz die von einem user abfragen
                                //request.cardDeck = BattleMaker.GetRandCards();

                                request.cardCollection = mySqlDataClass.GetCardsFromDB(request.GetUsernameFromDict());
                                request.cardDeck = BattleMaker.The4BestCards(request.cardCollection);
                                clientList.Add(request);

                                //noch lock hinzufügen
                                while (sieger == "noOne")
                                {
                                    mut.WaitOne();
                                    sieger = BattleMaker.AddToBattleQueue(clientList);
                                    Thread.Sleep(1000);
                                    mut.ReleaseMutex();
                                }
                                Console.WriteLine("And the winner is: {0}", sieger);


                            }
                            else if(request.message == "OptainNewCards")
                            {
                                Console.WriteLine("Welcome to the Shop!");
                                Console.WriteLine("0...quit");
                                Console.WriteLine("1...4 cards cost 25 Coins"!);
                                string choiceCardShop = Console.ReadLine().Trim(' ', '\n');

                                if (choiceCardShop == "1") ;
                                {
                                    DbFunctions.OptainNewCards(userFromDb);
                                }
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
            Console.WriteLine("\n");
        }

    }
}
