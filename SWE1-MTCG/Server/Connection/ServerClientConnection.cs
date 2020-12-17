using Cards;
using SWE1_MTCG;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Npgsql;

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
                            try { 
                                Console.WriteLine("\nClient Connected!");                            
                                bool loggedIn = false;
                                DbUser userFromDb = new DbUser();
                                NetworkStream stream = null;
                                RequestContext request = new RequestContext();
                                ServerDbCOnnection mypostgresDataClass = new ServerDbCOnnection();
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
                                            Console.WriteLine("Du hast keine Versuche mehr");
                                            Console.ReadLine();
                                            break;
                                        }
                                        else
                                        {
                                            string tempMessage = "YouAreRegistred\n";
                                            registered = DbFunctions.RegisterAtDB(request, stream);
                                            ServerClientConnection.sendData(stream, tempMessage);
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
                                        request.cardCollection = mypostgresDataClass.GetCardsFromDB(request.GetUsernameFromDict());
                                        

                                        if (request.cardCollection.Count < 3)
                                        {
                                            sendData(stream, "Du musst zuerst karten kaufen");
                                            continue;
                                        }

                                        request.cardDeck = BattleMaker.The4BestCards(request.cardCollection);

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
                                        if(sieger == request.GetUsernameFromDict())
                                        {
                                            Console.WriteLine(sieger);
                                            //getcards from
                                        }
                                        else
                                        {
                                            Console.WriteLine(request.GetUsernameFromDict());

                                        }
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
                                        if(tempListForAnswerToClient == null)
                                        {
                                            sendData(stream, "ZuWenigeCoins");
                                        }
                                        else
                                        {
                                            string tempStringForAnswerToClient = getAllNames(tempListForAnswerToClient);
                                            sendData(stream, tempStringForAnswerToClient);
                                        }
                                        

                                    }
                                    else 
                                    if (request.message.Trim('\n') == "ShowDeck")
                                    {
                                        //coming soon
                                        //will alle karten anzeigen
                                        userFromDb.cardCollection = mypostgresDataClass.GetCardsFromDB(userFromDb.userName);
                                        userFromDb.cardDeck = BattleMaker.The4BestCards(userFromDb.cardCollection);

                                        string answer = getAllNames(userFromDb.cardDeck);
                                        sendData(stream, answer);

                                    }
                                    else if (request.message.Trim('\n') == "ShowCardCollection")
                                    {
                                        userFromDb.cardCollection = mypostgresDataClass.GetCardsFromDB(userFromDb.userName);
                                        userFromDb.cardDeck = BattleMaker.The4BestCards(userFromDb.cardCollection);

                                        string answer = getAllNames(userFromDb.cardCollection);
                                        sendData(stream, answer);
                                    }
                                    else if (request.message.Trim('\n') == "Trade4Coins")
                                    {
                                        while(true)
                                        {
                                            //Console.WriteLine("ready to trade");
                                            userFromDb.cardCollection = mypostgresDataClass.GetCardsFromDB(userFromDb.userName);
                                            string answer = getAllNames(userFromDb.cardCollection);
                                            sendData(stream, answer);

                                            data = receiveData(client, stream);
                                            request = MessageHandler.GetRequest(data);

                                            if (request.message.Trim('\n') == "END")
                                            {
                                            
                                                break;
                                            }
                                            else
                                            {
                                                //die karte an der stelle löschen, coins hochzählen, aus der datenbank löschen
                                                //1. aus der datenbank löschen, dann kann man nur die karten neu laden


                                                int cardToTrade = Int32.Parse(request.message);
                                                //Console.WriteLine(userFromDb.cardCollection[cardToTrade - 1].getCardName()); //eins abziehen, weil die client eingabe bei 1 startet
                                                //Console.WriteLine(" ");
                                                //noch die coins anzeigen
                                                if(cardToTrade > userFromDb.cardCollection.Count)
                                                {
                                                    sendData(stream, "Wrong input\n do you want to continue?");
                                                    data = receiveData(client, stream);
                                                    request = MessageHandler.GetRequest(data);
                                                    if (request.message.Trim('\n') == "YES")
                                                    {
                                                        continue;
                                                    }
                                                    break;
                                                }

                                                int preis = CalcPreis(userFromDb.cardCollection[cardToTrade - 1]);
                                                //answer ob to sell
                                                string message = MakeMessageToSellCoinsAsk(preis);
                                                sendData(stream, message);

                                                data = receiveData(client, stream);
                                                request = MessageHandler.GetRequest(data);

                                                if(request.message.Trim('\n') =="YES")
                                                {
                                                    message = MakeMessageTradCoinsDelete(userFromDb, userFromDb.cardCollection[cardToTrade - 1]);
                                                    bool successQueryExecute = DbFunctions.PassQuery(message);

                                                    //coins hochzählen
                                                    userFromDb.coins += preis;
                                                    string MakeQuery4UpdateCoins = DbFunctions.MakeQueryForUpdateCoins(userFromDb);
                                                    successQueryExecute = DbFunctions.PassQuery(MakeQuery4UpdateCoins);
                                                }


                                            }
                                        }

                                    }
                                    else if (request.message.Trim('\n') == "TradeWithPlayer")
                                    {
                                        //ähnlich wie battle logic
                                        //eine karte auswählen, in queue hinzufügen
                                        //zweiten spieler hinzufügen, queue is nicht leer
                                        //zweiter spieler wählt eine karte aus, die er tauschen will
                                        //datenbank wird aktualisiert
                                        //beide werden aus der queue gelöscht
                                    }
                                    else
                                    {
                                        Console.WriteLine("Some unknown error!");
                                        Console.ReadLine();
                                        return;
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error {0}!", e);
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
            int counter = 1;
            foreach(var part in tempListForAnswerToClient)
            {
                temp += $"{counter}. {part.getCardName()}, {part.getCardType()}, {part.getElementTypes()}, ";
                //temp += counter.ToString() + ". ";
                //temp += part.getCardName() +", "+ part.getCardType() + ", " + part.getElementTypes() + ", ";
                    if (part.getCardType() == MyEnum.cardTypes.Monster)
                        temp += part.getCardProperty() + ", ";
                temp += part.getCardDamage();
                temp += "\n";

                counter++;
            }
            temp += "\n";
            return temp;
        }
        public static string MakeMessageTradCoinsDelete(DbUser userFromDb, BaseCards card)
        {
            string temp = "DELETE FROM userdata_cardcollection WHERE ";

            temp += "fk_user_uid = '" + userFromDb.uid + "' AND fk_card_uid = '" + card.getUID() + "';" ;

            return temp;
        }
        public static int CalcPreis(BaseCards card)
        {
            float temp = 0;

            temp = card.getCardDamage() * 0.25f - 1f; //weil man in einem Pack 4 Karten bekommt, ein pack 25 coins kostet und der damage von 0 bis 50 sein kann
            if(temp == 0)
            {
                temp = 1;
            }
            return Convert.ToInt32(temp);
        }
        public static string MakeMessageToSellCoinsAsk(int preis)
        {
            string temp = "";
            temp += "Du bekommst für deine Karte " + preis +  " coins";      

            return temp;
        }
    }
}
