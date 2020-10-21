using System;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

//The example sends credentials to the httpbin.org website.
namespace HttpClientAuth
{
    class Program
    {
        static async Task ClientAuth(string[] args)
        {
            var userName = "user7";
            var passwd = "passwd";
            //The URL contains authentication details because we test it with the httpbin.org website. 
            //This way we don't need to set up our own server. Authentication details are never put into the URL, of course.
            var url = "https://httpbin.org/basic-auth/user7/passwd";

            using var client = new HttpClient();

            //Here we build the authentication header.
            var authToken = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(authToken));

            var result = await client.GetAsync(url);

            var content = await result.Content.ReadAsStringAsync();
            Console.WriteLine(content);
        }
    }
}