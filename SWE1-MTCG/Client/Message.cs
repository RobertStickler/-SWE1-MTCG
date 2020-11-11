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



        public string GetMessage(RequestContextClient reqest)
        {         
            string temp_msg = null;

            /*
            while ((temp = Console.ReadLine()) != ".")
            {                
                temp_msg += temp + "\n";
            }
            */
            if (reqest.message_number == "1")
            {
                temp_msg = "StartTheBattle";
            }

            string message = advice + path + http_version;
            message += "Content-Type: " + content_type;
            message += "Content-Lenght: " + (temp_msg.Length).ToString() + "\n";
            message += "Host: " + reqest.ip + ":" + reqest.port.ToString();
            message += "\nUserName: " + reqest.username;
            message += "\nPassword: " + reqest.pwd;
            message += "\n\n" + temp_msg + "\n";

            return message;
        }
    }
}
