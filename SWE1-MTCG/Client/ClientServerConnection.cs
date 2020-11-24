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
            string choiceWhenLoggedIn = "-1";
            bool loggedIn = false;



            try
            {
                string message = "empty";

                while (loggedIn == false)
                {
                    message = msg.CreateMessageForSend(request); //da wird die message erstellt
                    Console.WriteLine();

                    // Translate the Message into ASCII.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    // Send the message to the connected TcpServer. 
                    stream.Write(data, 0, data.Length);
                    Console.Write("Sent:\n{0}", message);
                    // Bytes Array to receive Server Response.
                    data = new Byte[256];
                    String response = String.Empty;
                    // Read the Tcp Server Response Bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                    Console.Write("Received:\n{0}", response);
                    //Console.WriteLine("\ndu bist bis hier gekommen!");
                    if (response == "Succsessful") //you are logged in
                    {
                        loggedIn = true;

                        Program.PrintMenueTwo();
                        choiceWhenLoggedIn = Console.ReadLine(); 
                        //nur richtige eingabe zulassen
                        while ((choiceWhenLoggedIn != "3") && (choiceWhenLoggedIn != "4") && (choiceWhenLoggedIn != "5") && (choiceWhenLoggedIn != "0"))
                        {
                            Console.Write("Enter your choice: ");
                            choiceWhenLoggedIn = Console.ReadLine().Trim('\n');
                        }
                        //0 beendet das Programm
                        if (choiceWhenLoggedIn == "0")
                            return;

                        request.message_number = choiceWhenLoggedIn;
                        message = msg.CreateMessageForSend(request); //verwaltet wieder die nachricht
                        data = System.Text.Encoding.ASCII.GetBytes(message);
                        // Send the message to the connected TcpServer. 
                        stream.Write(data, 0, data.Length);
                        Console.Write("Sent:\n{0}", message);

                    }
                    else if(response == "AccessDenied")
                    {
                        Console.WriteLine("no more Attempts left!");
                        break;
                    }
                    else if(response == "TryAgain")
                    {
                        continue;
                    }
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
    }
}
