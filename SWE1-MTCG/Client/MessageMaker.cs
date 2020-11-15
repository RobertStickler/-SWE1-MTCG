using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Client
{
    public class Message
    {
        readonly string content_type = "text/plain; charset=utf-8\n";
        readonly string path = "/messages ";
        readonly string http_version = "HTTP/1.1\n";
        readonly string advice = "POST ";


        public string GetMessage(RequestContextClient request)
        {         
            string temp_msg = null;
            string message = "";

            /*
            while ((temp = Console.ReadLine()) != ".")
            {                
                temp_msg += temp + "\n";
            }
            */

            if (request.message_number == "1")
            {

                Console.Write("Username: ");
                request.username = Console.ReadLine();
                Console.WriteLine("Password: ");
                request.pwd = Console.ReadLine();
                temp_msg = "Login";
                message = MakeRequest(request, temp_msg);

            }
            else if (request.message_number == "2")
            {
                //prepare for register
                temp_msg = "Register";
                Console.Write("Username: ");
                request.username = Console.ReadLine();
                Console.WriteLine("Password: ");
                request.pwd = Console.ReadLine();
                Console.WriteLine("email: ");
                request.email = Console.ReadLine();
                message = MakeRegisterRequest(request, temp_msg);
            }
            else if (request.message_number == "3")
            {
                //kommt erst, wenn eingeloggt
                temp_msg = "StartTheBattle";
                message = MakeRequest(request, temp_msg);
            }            

            return message;
        }
        public string MakeRequest(RequestContextClient request, string temp_msg)
        {
            string message = advice + path + http_version;
            message += "Content-Type: " + content_type;
            message += "Content-Lenght: " + (temp_msg.Length).ToString() + "\n";
            message += "Host: " + request.ip + ":" + request.port.ToString();
            message += "\nUserName: " + request.username;
            message += "\nPassword: " + request.pwd; ;
            message += "\n\n" + temp_msg + "\n";

            return message;
        }
        public string MakeRegisterRequest(RequestContextClient request, string temp_msg)
        {
            string message = advice + path + http_version;
            message += "Content-Type: " + content_type;
            message += "Content-Lenght: " + (temp_msg.Length).ToString() + "\n";
            message += "Host: " + request.ip + ":" + request.port.ToString();
            message += "\nUserName: " + request.username;
            message += "\nPassword: " + request.pwd; ;
            message += "\nEmail: " + request.email; ;
            message += "\n\n" + temp_msg + "\n";

            return message;
        }
    }
}
