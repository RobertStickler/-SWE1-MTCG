using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SWE1_MTCG
{
    public class TCPClass
    {
        public void PrintUsage()
        {
            Console.WriteLine("Usage Example Insomnia:");
            Console.WriteLine("<ip-Adress>:<Port></Path>");
            Console.WriteLine("<For Example: \nlocalhost:6543/messages/1>");
        }
        //###################################################################################
        public static RequestContext GetRequest(string data)
        {
            //Console.WriteLine(data);
            RequestContext request = new RequestContext();

            //die daten aus dem HTTP-Request extrahieren
            string[] line = data.Split("\n"); //Bei einem Enter trennen
            string[] tempfirstline = line[0].Split(" "); //die erste Zeile an den Leerzeichen trennen
            //die Zeilen für den Header zusammenfassen
            for (int j = 1; j <= 4; j++)
            {
                request.header += line[j];
                request.header += "\n";
            }
            //die Zeilen der message zusammenfassen
            for (int k = 5; k < line.Length; k++)
            {
                request.message += line[k];
                request.message += "\n";
            }
            //Console.WriteLine(header);        
            //die separierten Daten in unserem Objekt speichern
            request.method = tempfirstline[0];
            request.path = tempfirstline[1];
            request.version = tempfirstline[2];
            //Console.WriteLine(request.message);
            return request;
        }

        //###################################################################################
        public static void GetAllMessages(List<RequestContext> Liste)
        {
            int number = 0;
            foreach (RequestContext aPart in Liste)
            {
                Console.WriteLine("\n{0} uid: {1} \nmessage: {2}", number, aPart.unique_id, aPart.message);
                number++; ;
            }
        }
        //###################################################################################
        public static void GetOneMessages(List<RequestContext> Liste, int number)
        {
            try
            {
                Console.WriteLine("\n{0} uid: {1} \nmessage: {2}", number, Liste[number].unique_id, Liste[number].message);
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
                Console.WriteLine("Error: {0}", outOfRange.Message);
                //throw new System.ArgumentOutOfRangeException("index parameter is out of range.", outOfRange);
            }
        }
        //###################################################################################
        public static int GetNumber(string path)
        {
            //der Pfad wird an den / getrennt
            string[] temp_path = path.Split("/");
            //Console.WriteLine(path);
            int number = Convert.ToInt32(temp_path[2]);
            return number;
        }
        //###################################################################################
        public static RequestContext AddMessage(RequestContext request)
        {
            string teststring = string.Format("{0:N}", Guid.NewGuid()); //erstellt eine unique Id 
            request.unique_id = teststring.Substring(0, 8); //kürzt die ID auf 8 stellen
            return request;
        }
        //###################################################################################
        public static int IndexFinder(List<RequestContext> Liste)
        {
            int index = 0;

            foreach (RequestContext aPart in Liste)
            {
                if (aPart.unique_id == "0")
                {
                    break;
                }
                index++;
            }
            return index;
        }
        //###################################################################################
        public static string GetPath(string path)
        {
            //der Pfad wird an den / getrennt
            string[] temp_path = path.Split("/");
            //Console.WriteLine(path);
            string origPath = (temp_path[1]);
            return origPath;
        }
        //###################################################################################
        public static void UpdateMessage(string message, List<RequestContext> Liste, int number)
        {
            try
            {
                Liste[number].message = message;
                Console.WriteLine("message updated");
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
                Console.WriteLine("Error: {0}", outOfRange.Message);
            }
        }
        //###################################################################################
        public static void DeleteMessage(List<RequestContext> Liste, int number)
        {
            try
            {
                //Liste[number].unique_id = "0";
                Liste.RemoveAt(number);
                Console.WriteLine("message deleted");
            }
            catch (ArgumentOutOfRangeException outOfRange)
            {
                Console.WriteLine("Error: {0}", outOfRange.Message);
            }
        }
    }
}