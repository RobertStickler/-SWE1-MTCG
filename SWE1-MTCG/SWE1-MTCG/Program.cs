using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SWE1_MTCG;

class MyTcpListener
{
    public void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("<Option> <Path>");
        Console.WriteLine("<Text>");
    }
    public static void Main()
    {
        TcpListener server = null;
        RequestContext request = new RequestContext();
        ResponseContext resonse = new ResponseContext();

        try
        {
            // Set the TcpListener on port 13000.
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
                    Console.WriteLine("Received:\n{0}", data);

                    string[] line = data.Split("\n");
                    string temp_header = null;
                    string[] tempfirstline = line[0].Split(" ");

                    for (i = 1; i < line.Length; i++)
                    {
                        temp_header += line[i];
                        temp_header += "\n";
                    }
                    //Console.WriteLine(header);
                    request.header = temp_header;
                    request.method = tempfirstline[0];
                    request.path = tempfirstline[1];
                    request.version = tempfirstline[2];
                    //Console.WriteLine(request.method);

                    if(request.method == "GET")
                    {
                        //lists all messages
                        if(request.path == "/messages")
                        {

                        }
                        else
                        {
                            //list messege with number
                        }
                    }
                    else if((request.method == "POST")
                    {
                        //add message
                        if(request.path == "/messages")
                        {

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

                        }
                        else
                        {
                            Console.WriteLine("Wrong message number");
                        }
                    }
                    else
                    {
                        //PrintUsage();
                    }

                }

                // Shutdown and end connection
                client.Close();

                Console.WriteLine("Press \"y\" to continue");
            }
            while ((Console.ReadLine().ToUpper() == "Y"));
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