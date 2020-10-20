using System;
using System.Net.Http;
using System.Threading.Tasks;

//Die HTTP-HEAD-Methode fordert die Header an, die zurückgegeben werden, wenn die angegebene Ressource mit einer HTTP-GET-Methode angefordert wird.
namespace HttpClientHead
{
    class Program
    {
        static async Task HeadRequest(string[] args)
        {
            var url = "http://webcode.me";
            using var client = new HttpClient();

            var result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Head, url));

            Console.WriteLine(result);
        }
    }
}