using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();
        
        static async Task Main()
        {
            string port = "6543";
            string ipAdress = "127.0.0.1";
            string advice = "/messages";
            string messageNumber = "/1";

            string uri = "http://" + ipAdress + ":" + port + advice; //das von insomnia hald

            Console.WriteLine("1...GET");
            Console.WriteLine("2...POST");
            Console.WriteLine("3...PUT");
            Console.WriteLine("4...DELETE");

            ClientFunctions clientFunc = new ClientFunctions();

            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                int choice = Convert.ToInt32(Console.ReadLine());
                if (choice >= 5)
                {
                    Console.WriteLine("error");
                }
                else if(choice < 1)
                {
                    Console.WriteLine("error");
                }

                switch(choice)
                {
                    case 1: //Get
                        {
                            clientFunc.GetRequest(uri);
                            break;
                        }
                    case 2: //Post
                        {
                            clientFunc.PostRequest(uri);
                            break;
                        }
                }
                

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            Console.Read();
        }
    }
}

