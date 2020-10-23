using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using SWE1_MTCG;

class MyTcpListener
{
    //static void muss angegeben werden

    public static void Main()
    {
        TCPClass tcpClass = new TCPClass();
        TcpListener server = null;
        List<RequestContext> Liste = new List<RequestContext>();

        try
        {
            // Set the TcpListener port.
            Int32 port = 6543;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            // Enter the listening loop.
            do
            {
                Console.Write("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                if ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    //Console.WriteLine("Received:\n{0}", data);

                    RequestContext request = TCPClass.GetRequest(data);


                    if (request.method == "GET")
                    {
                        if (request.path == "/messages")
                        {
                            //lists all messages
                            TCPClass.GetAllMessages(Liste);
                        }
                        else
                        {
                            //list messege with number
                            int number = TCPClass.GetNumber(request.path);
                            TCPClass.GetOneMessages(Liste, number);
                        }
                    }
                    else if (request.method == "POST")
                    {
                        //add message
                        if (request.path == "/messages")
                        {
                            request = TCPClass.AddMessage(request);
                            int index = TCPClass.IndexFinder(Liste);
                            Liste.Insert(index, request);
                        }
                        else
                        {
                            Console.WriteLine("Wrong path");
                        }
                    }
                    else if (request.method == "PUT")
                    {
                        //update the message
                        if (request.path != "/messages")
                        {                            
                            int number = TCPClass.GetNumber(request.path);
                            TCPClass.UpdateMessage(request.message, Liste, number);
                        }
                        else
                        {
                            Console.WriteLine("Wrong message number");
                        }

                    }
                    else if (request.method == "DELETE")
                    {
                        if (request.path != "/messages")
                        {
                            Console.WriteLine("you are in delete");
                            int number = TCPClass.GetNumber(request.path);
                            TCPClass.DeleteMessage(Liste, number);
                        }
                        else
                        {
                            Console.WriteLine("Wrong message number");
                        }
                    }
                    else
                    {
                        tcpClass.PrintUsage();
                    }
                    //Console.WriteLine(request.message);

                }

                // Shutdown and end connection
                client.Close();

                //Console.WriteLine("Press \"y\" to continue");
            }
            while (true);
            //while ((Console.ReadLine().ToUpper() == "Y")) ;
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            // Stop listening for new clients.
            server.Stop();
        }

        Console.WriteLine("\nHit enter to continue...");
        Console.Read();
    }
    
}