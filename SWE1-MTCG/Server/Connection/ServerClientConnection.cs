using Cards;
using SWE1_MTCG;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Npgsql;
using Org.BouncyCastle.Asn1.Crmf;

namespace Server
{
    public class ServerClientConnection
    {
        private static Mutex _mut = new Mutex();
        public static void StartServer()
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
                            try
                            {
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
                                    data = ServerClientConnection.ReceiveData(client, stream);
                                    Console.WriteLine("SERVER RECEIVED:\n" + data);

                                    //data verwalten und in ein Objekt speichern
                                    request = MessageHandler.GetRequest(data);

                                    if (request.message.Trim('\n') == "Login")
                                    {
                                        //check if login   
                                        if (attempt == 0)
                                        {
                                            string tempMessage = "AccessDenied";
                                            SendData(stream, tempMessage);
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
                                            SendData(stream, message);
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
                                            ServerClientConnection.SendData(stream, tempMessage);
                                            Console.WriteLine("Du hast keine Versuche mehr");
                                            Console.ReadLine();
                                            break;
                                        }
                                        else
                                        {
                                            registered = DbFunctions.RegisterAtDb(request, stream);

                                            if (registered == true)
                                            {
                                                string tempMessage = "YouAreRegistred\n";
                                                ServerClientConnection.SendData(stream, tempMessage);
                                            }


                                        }
                                        //setup for register
                                        if (registered == false)
                                        {
                                            attempt--;
                                            string tempMessage = "TryAgain\n";
                                            ServerClientConnection.SendData(stream, tempMessage);
                                        }
                                    }
                                }
                                while (true)
                                {
                                    string sieger = "noOne";

                                    data = "";
                                    //request.message = "empty";
                                    data = ServerClientConnection.ReceiveData(client, stream);
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
                                        request.cardCollection = mypostgresDataClass.GetCardsFromDb(request.GetUsernameFromDict());

                                        //wenn er zu weinige Karten besitzt
                                        if (request.cardCollection.Count < 3)
                                        {
                                            SendData(stream, "Du musst zuerst karten kaufen");
                                            continue;
                                        }

                                        //standardmäßig mal das auswählen
                                        if (request.cardDeck == null)
                                        {
                                            request.cardDeck = BattleMaker.The4BestCards(request.cardCollection);
                                        }


                                        clientList.Add(request);

                                        //noch lock hinzufügen
                                        while (sieger.Trim('\n') == "noOne")
                                        {
                                            if (!clientList.Contains(request))
                                            {
                                                break;
                                            }
                                            //Console.WriteLine(clientList.Count);
                                            _mut.WaitOne();
                                            sieger = BattleMaker.AddToBattleQueue(clientList);
                                            Thread.Sleep(1000);
                                            _mut.ReleaseMutex();
                                        }
                                        if (sieger == request.GetUsernameFromDict())
                                        {
                                            //elo points erhöhen
                                            Console.WriteLine(sieger);


                                        }
                                        else
                                        {
                                            //elo points minus
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
                                        if (tempListForAnswerToClient == null)
                                        {
                                            SendData(stream, "ZuWenigeCoins");
                                        }
                                        else
                                        {
                                            string tempStringForAnswerToClient = GetAllNames(tempListForAnswerToClient);
                                            SendData(stream, tempStringForAnswerToClient);
                                        }


                                    }
                                    else
                                    if (request.message.Trim('\n') == "ShowDeck")
                                    {
                                        userFromDb.cardCollection = mypostgresDataClass.GetCardsFromDb(userFromDb.userName);
                                        string answer;
                                        if (userFromDb.cardDeck.Count == 0)
                                        {
                                            if (userFromDb.cardCollection == null)
                                            {
                                                answer = "NoCards";
                                            }
                                            else
                                            {
                                                userFromDb.cardDeck = BattleMaker.The4BestCards(userFromDb.cardCollection);
                                                answer = GetAllNames(userFromDb.cardDeck);
                                            }

                                        }
                                        else
                                        {
                                            answer = GetAllNames(userFromDb.cardDeck);
                                        }
                                        SendData(stream, answer);

                                    }
                                    else if (request.message.Trim('\n') == "ShowCardCollection")
                                    {
                                        userFromDb.cardCollection = mypostgresDataClass.GetCardsFromDb(userFromDb.userName);
                                        string answer = String4ShowCardCollection(userFromDb.cardCollection);
                                        SendData(stream, answer);
                                    }
                                    else if (request.message.Trim('\n') == "Trade4Coins")
                                    {
                                        while (true)
                                        {
                                            //Console.WriteLine("ready to trade");
                                            userFromDb.cardCollection = mypostgresDataClass.GetCardsFromDb(userFromDb.userName);
                                            string answer = GetAllNames(userFromDb.cardCollection);
                                            SendData(stream, answer);

                                            data = ReceiveData(client, stream);
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
                                                if (cardToTrade > userFromDb.cardCollection.Count)
                                                {
                                                    SendData(stream, "Wrong input\n do you want to continue?");
                                                    data = ReceiveData(client, stream);
                                                    request = MessageHandler.GetRequest(data);
                                                    if (request.message.Trim('\n') == "YES")
                                                    {
                                                        continue;
                                                    }
                                                    break;
                                                }

                                                int preis = CalcPreis(userFromDb.cardCollection[cardToTrade - 1]);
                                                //answer ob to sell
                                                if(preis == 0)
                                                {
                                                    preis = 1;
                                                }
                                                string message = MakeMessageToSellCoinsAsk(preis);
                                                SendData(stream, message);

                                                data = ReceiveData(client, stream);
                                                request = MessageHandler.GetRequest(data);

                                                if (request.message.Trim('\n') == "YES")
                                                {
                                                    message = MakeMessageTradCoinsDelete(userFromDb, userFromDb.cardCollection[cardToTrade - 1]);
                                                    bool successQueryExecute = DbFunctions.PassQuery(message);

                                                    //coins hochzählen
                                                    userFromDb.coins += preis;
                                                    string makeQuery4UpdateCoins = DbFunctions.MakeQueryForUpdateCoins(userFromDb);
                                                    successQueryExecute = DbFunctions.PassQuery(makeQuery4UpdateCoins);
                                                }
                                                else if (request.message.Trim('\n') == "NO")
                                                {
                                                    break;
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

                                        string input = "1";

                                        do
                                        {
                                            

                                            if (input != "0")
                                            {
                                                //choose a card 
                                                userFromDb.cardDeck = new List<BaseCards>();

                                                BaseCards tempCard = null;
                                                //hier kann man die Karten auswählen, die im deck sein sollen
                                                userFromDb.cardCollection = mypostgresDataClass.GetCardsFromDb(userFromDb.userName);
                                                string answer = String4ShowCardCollection(userFromDb.cardCollection);
                                                SendData(stream, answer); //schickt die possible karten
                                                int number;

                                                //die zahl kommt als antwort und die will ich hochladen
                                                while (true)
                                                {
                                                    data = ReceiveData(client, stream);
                                                    request = MessageHandler.GetRequest(data);
                                                    number = Int32.Parse(request.message);

                                                    if (number < userFromDb.cardCollection.Count)
                                                    {
                                                        SendData(stream, "OK");
                                                        data = ReceiveData(client, stream);
                                                        request = MessageHandler.GetRequest(data);
                                                        input = request.message;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        SendData(stream, "False");
                                                        data = ReceiveData(client, stream);
                                                        request = MessageHandler.GetRequest(data);
                                                    }
                                                }


                                                data = ReceiveData(client, stream);
                                                request = MessageHandler.GetRequest(data);
                                                string spellOrMonster = request.message;

                                                data = ReceiveData(client, stream);
                                                request = MessageHandler.GetRequest(data);
                                                string requiredDamage = request.message;

                                                mypostgresDataClass.AddCardsToTrade(userFromDb, number, spellOrMonster, requiredDamage);
                                            }

                                        }
                                        while (input != "0");


                                    }
                                    else if (request.message.Trim('\n') == "ChangeTheDeck")
                                    {
                                        userFromDb.cardDeck = new List<BaseCards>();

                                        BaseCards tempCard = null;
                                        //hier kann man die Karten auswählen, die im deck sein sollen
                                        userFromDb.cardCollection = mypostgresDataClass.GetCardsFromDb(userFromDb.userName);
                                        string answer = String4ShowCardCollection(userFromDb.cardCollection);
                                        SendData(stream, answer);
                                        while (userFromDb.cardDeck.Count < 4)
                                        {
                                            data = ReceiveData(client, stream);
                                            request = MessageHandler.GetRequest(data);
                                            int number = Int32.Parse(request.message) -1; //wwil bei 0 zu zählen beginnen
                                            
                                            if(userFromDb.cardCollection.Count < number)
                                            {
                                                SendData(stream, "NumberToHigh");
                                                continue;
                                            }

                                            tempCard = userFromDb.cardCollection[number];
                                            //tscheck if falide
                                            //eine karte darf z.b nur einmal drinnen sein
                                            if (CheckIfAddToDeckIsValide(tempCard, userFromDb))
                                            {
                                                userFromDb.cardDeck.Add(tempCard);
                                                SendData(stream, GetAllNames(userFromDb.cardDeck));
                                            }
                                            else
                                            {
                                                SendData(stream, "cardAlreadyUsed");
                                                continue;
                                            }
                                        }
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
            catch (Exception e)
            {
                Console.WriteLine("Error {0}!", e);
            }
        }

        public static bool CheckIfAddToDeckIsValide(BaseCards tempCard, DbUser userFromDb)
        {
            //nun kann ein benutzer eine karte leider auch mehrmals haben
            int numbeCardcollection = WieOftHatErDieKarte(userFromDb.cardCollection, tempCard.getUID()); //so oft darf er eine bestimmte karte verwenden
            int numbeDeck = WieOftHatErDieKarte(userFromDb.cardDeck, tempCard.getUID()); //so oft hat er die karte bereits im deck

            if (numbeCardcollection > numbeDeck)
            {
                return true;
            }

            return false;

        }

        public static int WieOftHatErDieKarte(List<BaseCards> cardList, string cardId)
        {
            int counter = 0;
            if (cardList == null)
            {
                return 0;
            }
            foreach (BaseCards part in cardList)
            {
                if (part.getUID() == cardId)
                {
                    counter++;
                }
            }

            return counter;
        }

        public static string String4ShowCardCollection(List<BaseCards> cardDeck)
        {
            string answer = "";
            if (cardDeck == null)
            {
                answer = "NoCards";
            }
            else
            {
                answer = GetAllNames(cardDeck);
            }
            return answer;
        }
        public static string ReceiveData(TcpClient client, NetworkStream stream)
        {
            byte[] bytes = new byte[client.ReceiveBufferSize];
            stream.Read(bytes, 0, (int)client.ReceiveBufferSize);
            string returndata = Encoding.UTF8.GetString(bytes);
            return returndata.Trim('\0');
        }
        public static void SendData(NetworkStream stream, string message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.Write("Sent:\n{0}", message);
            Console.WriteLine("\n");
        }
        public static string GetAllNames(List<BaseCards> tempListForAnswerToClient)
        {
            string temp = "";
            int counter = 1;
            foreach (var part in tempListForAnswerToClient)
            {
                if (part == null)
                {
                    throw new ArgumentException("Spieler hat keine Karten");
                }
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

            temp += "fk_user_uid = '" + userFromDb.uid + "' AND fk_card_uid = '" + card.getUID() + "';";

            return temp;
        }
        public static int CalcPreis(BaseCards card)
        {
            float temp = 0;

            temp = card.getCardDamage() * 0.25f - 1f; //weil man in einem Pack 4 Karten bekommt, ein pack 25 coins kostet und der damage von 0 bis 50 sein kann
            if (temp == 0)
            {
                temp = 1;
            }
            return Convert.ToInt32(temp);
        }
        public static string MakeMessageToSellCoinsAsk(int preis)
        {
            string temp = "";
            temp += "Du bekommst für deine Karte " + preis + " coins";

            return temp;
        }
    }
}
