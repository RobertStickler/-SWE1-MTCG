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
            for (int j = 1; j < line.Length; j++)
            {
                request.header += line[j];                
                request.header += "\n";
            }
            //die Zeilen der message zusammenfassen
            
            for (int k = 6; k < line.Length; k++)
            {
                request.message += line[k];
                request.message = request.message.Trim(new char[] {'\r', '\n' });
                request.message += "\n";
            }
            if (request.message == "\n")
                request.message = "empty";

            //Console.WriteLine(header);        
            //die separierten Daten in unserem Objekt speichern
            request.method = tempfirstline[0];
            request.path = tempfirstline[1];
            request.version = tempfirstline[2].Trim(new char[] { ' ', '\r', '\n' });
            //Console.WriteLine(request.message);
            return request;
        }

//###################################################################################
        public static string GetAllMessages(List<RequestContext> Liste)
        {
            string temp = "";
            int number = 0;
            foreach (RequestContext aPart in Liste)
            {             
                Console.WriteLine("\n{0} uid: {1} \nmessage: {2}", number, aPart.unique_id, aPart.message);
                //Console.WriteLine(aPart.header);
                temp +=  "<br>";
                temp += aPart.message;
                temp += "\n";
                number++; ;
            }
            return temp;
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
        public static void UpdateMessage (string message, List<RequestContext> Liste, int number)
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
        //###################################################################################
        
        public static string GetResponse(RequestContext message, string allMessages)
        {
            ResponseContext response = new ResponseContext();

     
            response.status_message = "OK";
            response.status_code = "200";

            /*if (response.message != null)
            {
                response.header = MakeHeader(message);
                response.message = MakeMessage(message);

                return message.version + " "
                        + response.status_code + " "
                        + response.status_message
                        + response.header + "\n"
                        + response.message;
            }
            return message.version + " "
                        + response.status_code + " "
                        + response.status_message
                        + response.header + "\n"
                        + "Empty";*/

            response.message = allMessages;

            response.header = MakeHeader(message, response);
            

            return message.version + " " 
                    + response.status_code + " " 
                    + response.status_message 
                    + response.header + "\n"
                    + response.message;
;        } 
        public static string MakeHeader(RequestContext message, ResponseContext newMesasge)
        {
            string localDate = DateTime.Now.ToString();
            string temp = "\nDate: " + localDate
                        + "\nHost: Server Apache"
                        + "\nLast-Modified: " + localDate
                        + "\nERag: " + message.unique_id
                        + "\nAccept.Ranges: bytes"
                        + "\nContent-Length: " + (newMesasge.message.Length).ToString()
                        + "\nContent-Type: text/html\n";
            return temp;
        }
        public static string MakeMessage(RequestContext message)
        {
            string temp ="<html><header><title>" + "Test" + "?</title></header>"          + "<body>" + message.message + "</body></html>";

            return "Empty";
        }
    }
}
