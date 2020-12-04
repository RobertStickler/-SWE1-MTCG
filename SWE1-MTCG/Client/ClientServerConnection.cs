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
            Byte[] data = null;



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
                    if (response == "Succsessful") //you are logged in
                    {
                        loggedIn = true;
                        break;

                    }
                    else if (response == "AccessDenied")
                    {
                        Console.WriteLine("no more Attempts left!");
                        break;
                    }
                    else if (response == "TryAgain")
                    {
                        continue;
                    }
                    else if (response == "YouAreRegistred")
                    {
                        continue;
                    }
                }
                while(true) //wenn eingeloggt
                {
                    Program.PrintMenueTwo();
                    string choiceWhenLoggedIn = "-1";
                    //nur richtige eingabe zulassen
                    while ((choiceWhenLoggedIn != "3") && (choiceWhenLoggedIn != "4") && (choiceWhenLoggedIn != "5") && (choiceWhenLoggedIn != "6") && (choiceWhenLoggedIn != "0"))
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

                    Console.Write("Received:\n{0}", response);
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
