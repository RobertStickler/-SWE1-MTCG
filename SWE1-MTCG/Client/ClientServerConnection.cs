using System;
using System.Net.Sockets;
using System.Text;
using Org.BouncyCastle.Asn1.IsisMtt.Ocsp;

namespace Client
{
    public class ClientFunctions
    {

        public void SocketConnection()
        {
            Program.PrtinMenueZero();
            string incomeChoice = "";
            int counter4Demo = 3;



            while(incomeChoice != "y" && incomeChoice != "n")
            {                
                incomeChoice = Console.ReadLine().Trim('\n');
                if (incomeChoice == "0")
                    return;
            }

            string choiceWhenLoggedOut;
            Program.PrintMenueOne();

            //überall sachen adden für demo mode
            if(incomeChoice == "n")
            {
                choiceWhenLoggedOut = MyChoice1(); //inut error handling
            }
            else
            {
                choiceWhenLoggedOut = "1";
            }

            //0 beendet das Programm
            if (choiceWhenLoggedOut == "0")
                return;

            //für die Verbindung von Server und Client
            var request = new RequestContextClient();
            request.port = 6543;
            request.ip = "127.0.0.1";
            request.message_number = choiceWhenLoggedOut;

            var client = new TcpClient(request.ip, request.port);
            var stream = client.GetStream();
            
            bool loggedIn = false;

            try
            {
                loggedIn = NotLoggedIn(loggedIn, request, client, stream, incomeChoice);
                if(loggedIn == false)
                {
                    return;
                }

                LoggedInFunc(request, client, stream, incomeChoice, counter4Demo);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                stream.Close();
                client.Close();
            }

            Console.Read();
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
            //Console.Write("Sent:\n{0}", message);
            //Console.WriteLine("\n");
        }
        public static string MyChoice1()
        {
            string choiceWhenLoggedOut = "";
            while ((choiceWhenLoggedOut != "1") && (choiceWhenLoggedOut != "2") && (choiceWhenLoggedOut != "0"))
            {
                Console.Write("Enter your choice: ");
                choiceWhenLoggedOut = Console.ReadLine().Trim('\n');
            }
            return choiceWhenLoggedOut;
        }
        public static string MyChoiceBigMenue()
        {
            string choiceWhenLoggedIn = "-1";
            while (Int32.Parse(choiceWhenLoggedIn) < 3 || Int32.Parse(choiceWhenLoggedIn) > 10)
            {
                Console.Write("Enter your choice: ");
                choiceWhenLoggedIn = Console.ReadLine().Trim('\n');  
                
                if(Int32.Parse(choiceWhenLoggedIn) == 0)
                {
                    return choiceWhenLoggedIn;
                }
            }
            return choiceWhenLoggedIn;
        }

        public static void LoggedInFunc(RequestContextClient request, TcpClient client, NetworkStream stream, string incomeChoice, int counter4Demo)
        {
            while (true) //wenn eingeloggt
            {
                Program.PrintMenueTwo();
                var msg = new Message();
                string response, message, choiceWhenLoggedIn;

                //nur richtige eingabe zulassen
                if (incomeChoice == "n")
                {
                     choiceWhenLoggedIn = MyChoiceBigMenue();
                }
                else
                {
                    choiceWhenLoggedIn = counter4Demo.ToString(); //wieso so kompliziert hochzählen haha
                    Console.WriteLine("The coice was {0}", choiceWhenLoggedIn);
                }
                //0 beendet das Programm
                if (choiceWhenLoggedIn == "0")
                    return;

                //handelt eig alles, wo man keine zusätzliche user eingabe machen muss
                request.message_number = choiceWhenLoggedIn; //die auswahl in die mesasge speichern
                message = msg.CreateMessageForSend(request, incomeChoice); //verwaltet wieder die nachricht
                sendData(stream, message);
                response = receiveData(client, stream);

                //alles was zusätzliche user eingabe benötigt
                if (choiceWhenLoggedIn == "7")
                {
                    Trade4CoinsFunc(response, request, client, stream, incomeChoice);
                }

                if (choiceWhenLoggedIn == "8")
                {
                    TradeWithPeopleFunc(response, request, client, stream, incomeChoice);
                }

                if (choiceWhenLoggedIn == "9")
                {
                    EditYourDeckFunc(response, request, client, stream, incomeChoice);                    
                }

                //einige spezielle antworten des servers abfragen und auch ausgeben
                if (response == "TradeWithPlayer")
                {
                    Console.WriteLine("coming soon");
                }
                if (response == "ZuWenigeCoins")
                {
                    Console.WriteLine("Bro, kauf dir Münzen");
                }
                if (response == "NoCards")
                {
                    Console.WriteLine("Du hast keine Karten");
                }
                Console.WriteLine("");
                Console.Write("Received:\n{0}\n\n", response);

                counter4Demo++; //hochzählen nicht vergessen

                if (counter4Demo == 11)
                {
                    counter4Demo = 0;
                    Console.WriteLine("you have rached the end");
                }
            }
        }
        public static void EditYourDeckFunc(string response, RequestContextClient request, TcpClient client, NetworkStream stream, string incomeChoice)
        {
            Console.WriteLine(response);
            Console.WriteLine("Enter Cards for Deck (exact 4)");
            string number, message;
            var msg = new Message();
            int counter = 1;

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(": ");
                if (incomeChoice == "n")
                {
                    number = Console.ReadLine();
                }
                else
                {
                    number = counter.ToString();
                    Console.WriteLine("Choice was {0}", counter);
                }

                message = msg.MakeRequest(request, number);
                sendData(stream, message);

                Console.WriteLine("Your current Deck");
                response = receiveData(client, stream);
                Console.WriteLine(response);

                if (response == "cardAlreadyUsed")
                {
                    Console.WriteLine("Card is already in the Deck");
                    i--;
                }
                if (response == "NumberToHigh")
                {
                    Console.WriteLine("Card does not exist");
                    i--;
                }

                counter++;

            }
        }
        public static void TradeWithPeopleFunc(string response, RequestContextClient request, TcpClient client, NetworkStream stream, string incomeChoice)
        
        {
            var msg = new Message();
            string message;
            string answerMessage, cardToTrade;
            

            Console.WriteLine("0 .. to exit");
            Console.WriteLine("1 .. to add a card");
            Console.WriteLine("2 .. to see other cards");
            Console.WriteLine(": ");

            if (incomeChoice == "n")
            {
                cardToTrade = Console.ReadLine(); //eingabe prüfen
            }
            else
            {
                cardToTrade = "2";
                Console.WriteLine("Choice was 2");
            }
                       
            Console.WriteLine(response);
            message = msg.MakeRequest(request, cardToTrade);
            sendData(stream, message);

            if(cardToTrade == "1")
            {
                do
                {
                    Console.Write("choose a card to trade: ");
                    cardToTrade = MyChoice3();
                    message = msg.MakeRequest(request, cardToTrade);
                    sendData(stream, message);
                    answerMessage = receiveData(client, stream);
                }
                while (answerMessage != "OK");

                answerMessage = SpellOrMonsterFunc(answerMessage);
                message = msg.MakeRequest(request, answerMessage);
                sendData(stream, message);

                answerMessage = DamageValue();

                message = msg.MakeRequest(request, answerMessage);
                sendData(stream, message);
            }
            else if(cardToTrade =="2")
            {
                answerMessage = receiveData(client, stream);
                Console.WriteLine(answerMessage);

                Console.WriteLine("Choose a number from a traiding offer:");
                if (incomeChoice == "n")
                {
                    cardToTrade = Console.ReadLine();
                }
                else
                {
                    cardToTrade = "2";
                    Console.WriteLine("Choice was 2");
                }

                message = msg.MakeRequest(request, cardToTrade);
                sendData(stream, message);
                answerMessage = receiveData(client, stream);
                //Console.WriteLine(answerMessage);

                Console.Write("choose one of your cards to trade: ");
                if (incomeChoice == "n")
                {
                    cardToTrade = Console.ReadLine();
                }
                else
                {
                    cardToTrade = "10";
                    Console.WriteLine("Choice was 10");
                }
                message = msg.MakeRequest(request, cardToTrade);
                sendData(stream, message);

                answerMessage = receiveData(client, stream);
                Console.WriteLine(answerMessage);
            }        

        }
        public static string DamageValue()
        {
            string answerMessage = Int32.MaxValue.ToString();
            while (Int32.Parse(answerMessage) < 0 || Int32.Parse(answerMessage) > 50)
            {
                Console.WriteLine("\nWhich damage value do you want?\n: ");
                answerMessage = Console.ReadLine().Trim('\n');
            }
            return answerMessage;
        }
        public static string SpellOrMonsterFunc(string answerMessage)
        {
            Console.WriteLine("1 .. monster");
            Console.WriteLine("2 .. spell");

            while (answerMessage != "1" && answerMessage != "2")
            {
                Console.WriteLine("\nDo you want a spell or a monster?\n: ");
                answerMessage = Console.ReadLine().Trim('\n');
            }
            return answerMessage;
        }
        public static string MyChoice3()
        {
            int parsedNumber;
            string cardToTrade;
            while (true)
            {
                Console.Write("choose a card to trade: ");
                cardToTrade = Console.ReadLine();
                if (Int32.TryParse(cardToTrade, out parsedNumber) == true)
                {
                    break;
                }
            }
            //damit auch eine kombination aus zahlen und buchstaben richtig erkannt wird
            return parsedNumber.ToString();
        }
        public static void Trade4CoinsFunc(string response, RequestContextClient request, TcpClient client, NetworkStream stream, string incomeChoice)
        {
            var msg = new Message();
            string message;
            int counterForDemo = 0;
            Console.WriteLine(response);
            
            while (true)
            {
                string cardToTrade;

                //überall sachen adden für demo mode
                if (incomeChoice == "n")
                {
                    cardToTrade = MyChoice3();
                }
                else
                {
                    if (counterForDemo == 0)
                    {
                        cardToTrade = "7";
                        Console.WriteLine("Choosen card = 7");
                    }
                    else
                    {
                        cardToTrade = "0";
                        Console.WriteLine("entered 0 to leave");
                    }
                }

                if (cardToTrade == "0")
                {
                    message = msg.MakeRequest(request, "END");
                    sendData(stream, message);
                    break;
                }

                message = msg.MakeRequest(request, cardToTrade);
                sendData(stream, message);

                response = receiveData(client, stream);
                Console.WriteLine(response);
                Console.WriteLine("y/n ?");
                Console.Write(": ");
                string choice;

                if (incomeChoice == "n")
                {
                    choice = Console.ReadLine();
                }
                else
                {
                    choice = "y";
                    Console.WriteLine("Choice is y");
                }
                //wenn man die karte tauschen will
                if (choice.Trim('\n') == "y") 
                {
                    message = msg.MakeRequest(request, "YES");
                    sendData(stream, message);
                    //neue liste empfangen
                    response = receiveData(client, stream);
                }
                else
                {
                    message = msg.MakeRequest(request, "NO");
                    sendData(stream, message);
                    break;
                }

                counterForDemo++;
            }
        }
        
        public static bool NotLoggedIn(bool loggedIn, RequestContextClient request, TcpClient client, NetworkStream stream, string incomeChoice)
        {
            while (loggedIn == false)
            {
                var msg = new Message();
                string response, message;
                message = msg.CreateMessageForSend(request, incomeChoice); //da wird die message zum anmelden erstellt
                //Console.WriteLine();

                sendData(stream, message);
                //Console.Write("Sent:\n{0}", message);

                response = receiveData(client, stream);


                //Console.Write("Received:\n{0}", response);
                //Console.WriteLine("\ndu bist bis hier gekommen!");
                if (response.Trim('\n') == "Succsessful") //you are logged in
                {
                    return true;
                }
                else if (response.Trim('\n') == "AccessDenied")
                {
                    Console.WriteLine("no more Attempts left!");
                    return false;
                }
                else if (response.Trim('\n') == "TryAgain")
                {
                    continue;
                }
                if (response.Trim('\n') == "YouAreRegistred")
                {
                    response = "Succsessful";
                    return true;
                }
            }
            return false;
        }
    }

}
