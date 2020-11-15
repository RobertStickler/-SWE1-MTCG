using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SWE1_MTCG
{
    public class MessageHandler
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
            int tempcount = 1;

            //zuerst die erste zeile einlesen
            string[] tempfirstline = line[0].Split(" "); //die erste Zeile an den Leerzeichen trennen
            request.KeyValues.Add("method", tempfirstline[0]);
            request.KeyValues.Add("path", tempfirstline[1]);
            request.KeyValues.Add("version", tempfirstline[2]);

            foreach (string oneLine in line)
            {
                if(tempcount == 1)
                {
                    tempcount = 2;
                    continue;
                }
                if (oneLine == "")
                {
                    tempcount = 3;
                    continue;
                }

                if(tempcount == 2)
                {
                    string[] temp = oneLine.Split(":");
                    request.KeyValues.Add(temp[0], temp[1].Trim(' '));
                }
                if(tempcount == 3)
                {
                    request.message += oneLine;
                    request.message += "\n";
                }
                
            }
            request.message = request.message.Trim('\n', '\0');

            //to output the dict 
            foreach ( KeyValuePair<string, string> kvp in request.KeyValues)
            {
                Console.WriteLine("{0}: {1}",kvp.Key, kvp.Value);
            }
            return request;
        }       
    }
}