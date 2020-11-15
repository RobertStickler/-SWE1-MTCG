using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using SWE1_MTCG;
using Cards;
using System.Runtime.Serialization.Formatters;

namespace Server
{
    public class DbFunctions
    {
        public static bool VerifyLogin(RequestContext request, NetworkStream stream)
        {            
            MySqlDataClass mysql = new MySqlDataClass();
            var user = mysql.GetOneUser(request.GetUsernameFromDict()); //erstellt ein DB User objekt
            Console.WriteLine(user.userName);
            if (request.GetPWDFromDict() == user.pwd)
            {
                string message = "Succsessful";
                ServerClientConnection.sendData(stream, message);
                return true;
            }
            Console.WriteLine("Wrong user or Pwd!");
            return false;
        }
        public static RequestContext makeAnotherRequest(TcpClient client, NetworkStream stream)
        {
            var request = new RequestContext();
            string message = "please try again";
            // Translate the Message into ASCII.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            return request;
        }

    }
}
