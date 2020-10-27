using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class ClientFunctions
    {
        static readonly HttpClient client = new HttpClient();
        public void GetRequest(string ur)
        {

        }

        public async Task PostRequest(string uri)
        {
            try
            {
                Console.WriteLine("enter your message");
                string message = Console.ReadLine();
                HttpContent data = new StringContent(message, Encoding.UTF8, "text/plain");

                HttpResponseMessage postReturn = await client.PostAsync(uri, data);
                string result = postReturn.Content.ReadAsStringAsync().Result;
                var send = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));

                Console.WriteLine(postReturn);
                Console.WriteLine(result);
            }
            catch
            {
                throw new HttpRequestException();
            }
            
        }



    }
}
