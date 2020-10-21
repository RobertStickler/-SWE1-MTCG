using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

//Im Beispiel laden wir ein Bild von der Website webcode.me herunter. Das Bild wird in den Ordner "Dokumente" des Benutzers geschrieben.
namespace HttpClientDownloadImage
{
    class Program
    {
        static async Task DownloadImage(string[] args)
        {
            using var httpClient = new HttpClient();
            var url = "http://webcode.me/favicon.ico";

            //The GetByteArrayAsync() returns the image as an array of bytes.
            byte[] imageBytes = await httpClient.GetByteArrayAsync(url);

            //We determine the Documents folder with the GetFolderPath() method.
            string documentsPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);

            string localFilename = "favicon.ico";
            string localPath = Path.Combine(documentsPath, localFilename);
            //The bytes are written to the disk with the File.WriteAllBytes() method.
            File.WriteAllBytes(localPath, imageBytes);
        }
    }
}