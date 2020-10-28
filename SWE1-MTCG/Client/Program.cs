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

        static void Main(string[] args)
        {
            int port = 6543;
            string ipAdress = "127.0.0.1";
            string advice = "/messages";
            string message_umber = "/1";

            TcpClient client = new TcpClient(ipAdress, port);
            NetworkStream stream = client.GetStream();
            Message msg = new Message();

            Console.WriteLine("1...GET");
            Console.WriteLine("2...POST");
            Console.WriteLine("3...PUT");
            Console.WriteLine("4...DELETE");

            try
            {
                string message = msg.GetMessage(client, ipAdress, port.ToString(), "POST", message_umber);
                Console.WriteLine("message \n\n");
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


