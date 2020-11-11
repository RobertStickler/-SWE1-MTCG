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

        public void SocketConnection(string userName, string password)
        {
            var request = new RequestContextClient();
            request.port = 6543;
            request.ip = "127.0.0.1";
            request.username = userName;
            request.pwd = password;
                        
            var client = new TcpClient(request.ip, request.port);
            var stream = client.GetStream();
            var msg = new Message();
            

            Console.WriteLine("1...Start Battle");
            Console.WriteLine("2...GET");
            Console.WriteLine("3...POST");
            Console.WriteLine("4...PUT");
            Console.WriteLine("5...DELETE");

            request.message_number = Console.ReadLine().Trim('\n');

            try
            {                
                string message = msg.GetMessage(request);

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
