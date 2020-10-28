using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace Client
{
    public class Message
    {
        string content_type = "text/plain; charset=utf-8\n";
        string path = "/messages";
        string http_version = " HTTP/1.1\n";



        public string GetMessage(TcpClient client, string ip, string port, string advice, string message_number )
        {
            
            string temp;
            string temp_msg = null;

            while ((temp = Console.ReadLine()) != "quit")
            {
                temp_msg += temp + "\n";
            }

            string message = advice + path + http_version;
            message += "Content-Type: " + content_type;
            message += "Content-Lenght: " + (temp_msg.Length).ToString() + "\n";
            message += "Host: " + ip + ":" + port + "\n";
            message += "\n" + temp_msg + "\n";

            return message;
        }
    }
}
