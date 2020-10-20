using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace HttpClientJson
{
    class Contributor
    {
        public string Login { get; set; }
        public short Contributions { get; set; }

        public override string ToString()
        {
            return $"{Login,20}: {Contributions} contributions";
        }
    }

    class Program
    {
        private static async Task JsonRequest()
        {
            using var client = new HttpClient(); //In the request header, we specify the user agent.

            client.BaseAddress = new Uri("https://api.github.com");
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");

            //In the accept header value, we tell that JSON is an acceptable response type.
            client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

            //We generate a request and read the content asynchronously.
            var url = "repos/symfony/symfony/contributors";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();

            //We transform the JSON response into a list of Contributor objects with the JsonConvert.DeserializeObject() method.
            List<Contributor> contributors = JsonConvert.DeserializeObject<List<Contributor>>(resp);
            contributors.ForEach(Console.WriteLine);
        }
    }
}