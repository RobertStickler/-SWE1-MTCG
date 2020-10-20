using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientDownloadImage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var httpClient = new HttpClient();
            var url = "http://webcode.me/favicon.ico";
            byte[] imageBytes = await httpClient.GetByteArrayAsync(url);

            string documentsPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);

            string localFilename = "favicon.ico";
            string localPath = Path.Combine(documentsPath, localFilename);
            File.WriteAllBytes(localPath, imageBytes);
        }
    }
}