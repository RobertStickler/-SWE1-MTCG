using System;
using System.Net.Http;
using System.Threading.Tasks;

//HTTP Client Status Code
namespace HttpClientStatus
{
    class Program
    {
        static async Task GetStatus(string[] args)
        {
            using var client = new HttpClient(); //A new HttpClient is created.

            //When the asynchronous operation completes, the await operator returns the result of the operation, if any.
            var result = await client.GetAsync("http://webcode.me");
            Console.WriteLine(result.StatusCode);
        }
    }
}


