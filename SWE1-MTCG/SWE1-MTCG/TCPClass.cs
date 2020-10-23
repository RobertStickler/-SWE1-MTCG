using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SWE1_MTCG
{
    class TCPClass
    {
        public void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("<Option> <Path>");
            Console.WriteLine("<Text>");
        }

        public static RequestContext GetRequest(string data)
        {
            RequestContext request = new RequestContext();
            string[] line = data.Split("\n");
            string[] tempfirstline = line[0].Split(" ");

            for (int j = 1; j <= 4; j++)
            {
                request.header += line[j];
                request.header += "\n";
            }
            for (int k = 5; k < line.Length; k++)
            {
                request.message += line[k];
                request.message += "\n";
            }
            //Console.WriteLine(header);          
            request.method = tempfirstline[0];
            request.path = tempfirstline[1];
            request.version = tempfirstline[2];
            //Console.WriteLine(request.method);

            return request;
        }


        public static void GetAllMessages(List<RequestContext> Liste)
        {
            int number = 0;
            foreach (RequestContext aPart in Liste)
            {
                if (aPart.unique_id == "0")
                    continue;
                Console.WriteLine("\n{0} uid: {1} \nmessage: {2}", number, aPart.unique_id, aPart.message);               
                number++; ;
            }
        }

        public static void GetOneMessages(List<RequestContext> Liste, int number)
        {
            Console.WriteLine("\n{0} uid: {1} \nmessage: {2}", number, Liste[number].unique_id, Liste[number].message);
        }

        public static int GetNumber(string path)
        {
            string[] temp_path = path.Split("/");
            //Console.WriteLine(path);
            int number = Convert.ToInt32(temp_path[2]);
            return number;
        }

        public static RequestContext AddMessage(RequestContext request)
        {
            string teststring = string.Format("{0:N}", Guid.NewGuid());
            request.unique_id = teststring.Substring(0, 8);
            Console.WriteLine("message added");
            return request;
        }

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

        public static void UpdateMessage (string message, List<RequestContext> Liste, int number)
        {
            Liste[number].message = message;
        }

        public static void DeleteMessage(List<RequestContext> Liste, int number)
        {
            Liste[number].unique_id = "0";
        }


    }
}
