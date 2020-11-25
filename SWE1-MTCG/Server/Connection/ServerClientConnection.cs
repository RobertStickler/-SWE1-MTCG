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
                            Console.WriteLine("\nClient Connected!");                            
                            bool loggedIn = false;
                            DbUser userFromDb = new DbUser();
                            NetworkStream stream = null;
                            RequestContext request = new RequestContext();
                            MySqlDataClass mySqlDataClass = new MySqlDataClass();
                            int attempt = 3; //muss ich noch hinzufügen
                            bool registered = false;

                            while (loggedIn == false)
                            {
                                stream = client.GetStream();
                                data = ServerClientConnection.receiveData(client, stream);
                                Console.WriteLine("SERVER RECEIVED:\n" + data);

                                //data verwalten und in ein Objekt speichern
                                request = MessageHandler.GetRequest(data);

                                if (request.message.Trim('\n') == "Login")
                                {
                                    //check if login   
                                    if (attempt == 0)
                                    {
                                        string tempMessage = "AccessDenied";
                                        sendData(stream, tempMessage);
                                        //string gabadge = ServerClientConnection.receiveData(client, stream);
                                        break;
                                    }
                                    else
                                    {
                                        userFromDb = DbFunctions.VerifyLogin(request, stream);
                                    }

                                    
                                    if ((userFromDb == null))
                                    {
                                        attempt--;
                                        //client antworten und pw und user neu eingeben
                                        string message = "please try again\n";
                                        sendData(stream, message);
                                    }
                                    else
                                    {
                                        loggedIn = true;
                                    }

                                   

                                    //wieder auf nachricht warten
                                    //er ist nun eingeloggt
                                }
                                else if (request.message.Trim('\n') == "Register")
                                {
                                    if (attempt == 0)
                                    {
                                        string tempMessage = "AccessDenied";
                                        ServerClientConnection.sendData(stream, tempMessage);                                        
                                        break;
                                    }
                                    else
                                    {
                                        registered = DbFunctions.RegisterAtDB(request, stream);
                                    }
                                    //setup for register
                                    
                                    if(registered == false)
                                    {
                                        attempt--;
                                        string tempMessage = "TryAgain\n";
                                        ServerClientConnection.sendData(stream, tempMessage);
                                    }
                                }
                            }
                            while (true)
                            {
                                string sieger = "noOne";

                                data = "";
                                //request.message = "empty";
                                data = ServerClientConnection.receiveData(client, stream);
                                Console.WriteLine("SERVER RECEIVED:\n" + data);
                                //daten wieder einlesen
                                request = MessageHandler.GetRequest(data);
                                

                                //also nach dem einloggen, kann ein client man hier her
                                //brauch ich dann auch für später
                                if (request.message.Trim('\n') == "StartTheBattle")
                                {
                                    Console.WriteLine("Das battle beginnt in kürze");
                                    //statt den rand card muss ich jz die von einem user abfragen
                                    //request.cardDeck = BattleMaker.GetRandCards();
                                    request.stream = stream;
                                    request.cardCollection = mySqlDataClass.GetCardsFromDB(request.GetUsernameFromDict());
                                    request.cardDeck = BattleMaker.The4BestCards(request.cardCollection);
                                    //die besten 4 karten augeben an client

                                    clientList.Add(request);

                                    //noch lock hinzufügen
                                    while (sieger.Trim('\n') == "noOne")
                                    {
                                        if(!clientList.Contains(request))
                                        {
                                            break;
                                        }
                                        //Console.WriteLine(clientList.Count);
                                        mut.WaitOne();
                                        sieger = BattleMaker.AddToBattleQueue(clientList);
                                        Thread.Sleep(1000);
                                        mut.ReleaseMutex();
                                    }
                                    Console.WriteLine("And the winner is: {0}", sieger);
                                    //clientList.RemoveAt(0);
                                    clientList.Remove(request);
                                    //string message = "And the winner is: " + sieger + "\n";
                                    //sendData(stream, message);
                                }
                                else if (request.message.Trim('\n') == "OptainNewCards")
                                {
                                    List<BaseCards> tempList = new List<BaseCards>();
                                    Console.WriteLine("4 cards cost 25 Coins"!);
                                    //string choiceCardShop = Console.ReadLine().Trim(' ', '\n');

                                    var tempListForAnswerToClient = DbFunctions.OptainNewCards(userFromDb);
                                    string tempStringForAnswerToClient = getAllNames(tempListForAnswerToClient);
                                    sendData(stream, tempStringForAnswerToClient);

                                }
                                else if (request.message.Trim('\n') == "ShowDeck")
                                {
                                    //coming soon
                                    //will alle karten anzeigen

                                }
                                else
                                {
                                    Console.WriteLine("Some unknown error!");
                                    Console.ReadLine();
                                    return;
                                }
                            }
                        }).Start();
                    }
                }
            }
            catch (System.ArgumentException e)
            {
                Console.WriteLine(e);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error {0}!", e);
            }
        }
       

        public static string receiveData(TcpClient client, NetworkStream stream)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];
            stream.Read(bytes, 0, (int)client.ReceiveBufferSize);
            string returndata = Encoding.UTF8.GetString(bytes);
            return returndata.Trim('\0');
        }
        public static void sendData(NetworkStream stream, string message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.Write("Sent:\n{0}", message);
            Console.WriteLine("\n");
        }
        public static string getAllNames(List<BaseCards> tempListForAnswerToClient)
        {
            string temp = "";
            foreach(var part in tempListForAnswerToClient)
            {
                temp += part.getCardName() +", "+ part.getCardType() + ", " + part.getElementTypes() + ", ";
                    if(part.getCardType() == MyEnum.cardTypes.Monster)
                        temp += part.getCardProperty() + ", ";
                temp += part.getCardDamage();
                temp += "\n";
            }
            return temp;
        }
    }
}
