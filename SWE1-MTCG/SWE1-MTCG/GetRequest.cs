using System;
using System.Net.Http;
using System.Threading.Tasks;

//Die GET-Methode fordert eine Darstellung der angegebenen Ressource an.
namespace HttpClientEx
{
    class Program
    {
        static async Task GetRequest(string[] args)
        {
            using var client = new HttpClient();

            //The GetStringAsync() sends a GET request to the specified Uri and returns the response body as a string in an asynchronous operation.
            var content = await client.GetStringAsync("http://webcode.me");

            Console.WriteLine(content);
        }
    }
}