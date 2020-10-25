using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using.Threading.Task;

namespace SWE1_MTCG
{
    static async Task Main(string[] args)
    {
        string adress = "127.0.0.1";
        string port = ":6543/*";
        string uri = "http://*" + adress + port;
        HttpClient client = new HttpClient();

        string message = Console.ReadLine();
        var data = new StringContent(message, Encoding.UTF8, "test/plain");
        var content = await client.PostAsync(uri, data);
        var send = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, uri));
        Console.WriteLine("irgwas");
    }
}
