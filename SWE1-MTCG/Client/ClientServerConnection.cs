using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientFunctions
    {

        public void SocketConnection()
        {

            Program.PrintMenueOne();
            string choiceWhenLoggedOut = "-1";
            //inut error handling
            while ((choiceWhenLoggedOut != "1") && (choiceWhenLoggedOut != "2") && (choiceWhenLoggedOut != "0"))
            {
                Console.Write("Enter your choice: ");
                choiceWhenLoggedOut = Console.ReadLine().Trim('\n');
            }
            //0 beendet das Programm
            if (choiceWhenLoggedOut == "0")
                return;

            var request = new RequestContextClient();
            request.port = 6543;
            request.ip = "127.0.0.1";
            request.message_number = choiceWhenLoggedOut;


            var client = new TcpClient(request.ip, request.port);
            var stream = client.GetStream();
            var msg = new Message();
            
            bool loggedIn = false;

            try
            {
                string message = "empty";
                string response = "empty";

                while (loggedIn == false)
                {
                    
                    message = msg.CreateMessageForSend(request); //da wird die message erstellt
                    Console.WriteLine();

                    sendData(stream, message);
                    Console.Write("Sent:\n{0}", message);

                    response = receiveData(client, stream);

                    Console.Write("Received:\n{0}", response);
                    //Console.WriteLine("\ndu bist bis hier gekommen!");
                    if (response.Trim('\n') == "Succsessful") //you are logged in
                    {
                        loggedIn = true;
                        break;

                    }
                    else if (response.Trim('\n') == "AccessDenied")
                    {
                        Console.WriteLine("no more Attempts left!");
                        return;
                    }
                    else if (response.Trim('\n') == "TryAgain")
                    {
                        continue;
                    }
                    if (response.Trim('\n') == "YouAreRegistred")
                    {
                        response = "Succsessful";
                        loggedIn = true;
                        break;
                    }

                }
                while(true) //wenn eingeloggt
                {
                    Program.PrintMenueTwo();
                    string choiceWhenLoggedIn = "-1";
                    //nur richtige eingabe zulassen
                    while ((choiceWhenLoggedIn != "3") && (choiceWhenLoggedIn != "4") && (choiceWhenLoggedIn != "5") && (choiceWhenLoggedIn != "6") 
                        && (choiceWhenLoggedIn != "0") && (choiceWhenLoggedIn != "7") && (choiceWhenLoggedIn != "8"))
                    {
                        Console.Write("Enter your choice: ");
                        choiceWhenLoggedIn = Console.ReadLine().Trim('\n');
                    }
                    //0 beendet das Programm
                    if (choiceWhenLoggedIn == "0")
                        return;

                    request.message_number = choiceWhenLoggedIn;
                    message = msg.CreateMessageForSend(request); //verwaltet wieder die nachricht
                    sendData(stream, message);
                    Console.Write("Sent:\n{0}", message);

                    response = receiveData(client, stream);

                    if(choiceWhenLoggedIn == "7")
                    {
                        while(true)
                        {  
                            Console.WriteLine(response);    
                            string cardToTrade = "";
                            int parsedNumber;

                            while (true)
                            {
                                Console.Write("choose a card to trade: ");
                                cardToTrade = Console.ReadLine();
                                if (Int32.TryParse(cardToTrade, out parsedNumber) == true)
                                {
                                    break;
                                }
                            }                           


                            if(cardToTrade == "0")
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
                            string choice = Console.ReadLine();

                            if(choice.Trim('\n') == "y")
                            {
                                message = msg.MakeRequest(request, "YES");
                                sendData(stream, message);
                            }
                            else
                            {
                                message = msg.MakeRequest(request, "NO");
                                sendData(stream, message);
                            }


                            //neue liste empfangen
                            response = receiveData(client, stream);
                        }
                        
                    }


                    if(response == "TradeWithPlayer")
                    {
                        Console.WriteLine("coming soon");
                    }
                    if (response == "ZuWenigeCoins")
                    {
                        Console.WriteLine("Bro, kauf dir Münzen");
                    }
                    Console.WriteLine("");
                    Console.Write("Received:\n{0}\n\n", response);
                }

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
            Console.Write("Sent:\n{0}", message);
            Console.WriteLine("\n");
        }
    }

}
