using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

//Die HTTP-POST-Methode sendet Daten an den Server. Der Typ des Hauptteils der Anforderung wird durch den Content-Type-Header angegeben.
namespace HttpClientPost
{
    class Person
    {
        public string Name { get; set; }
        public string Occupation { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Occupation}";
        }
    }

    class Program
    {
        static async Task PostRequest(string[] args)
        {
            //We turn an object into a JSON data with the help of the Newtonsoft.Json package.
            var person = new Person();
            person.Name = "John Doe";
            person.Occupation = "gardener";

            var json = JsonConvert.SerializeObject(person);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "https://httpbin.org/post";
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);  //We send an asynchronous POST request with the PostAsync() method.

            string result = response.Content.ReadAsStringAsync().Result; //We read the returned data and print it to the console.
            Console.WriteLine(result);
        }
    }
}