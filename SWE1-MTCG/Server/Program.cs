using Cards;
using SWE1_MTCG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace Server
{
    public class Server
    {
        public static Random rand = new Random();

        static void Main(string[] args)
        {

            MySqlDataClass mysql = new MySqlDataClass();

            ServerClientConnection.startServer(); 


            //GenerateNewCards();

        }
        static void GenerateNewCards()
        {
            Console.WriteLine("if you are ready, press something");
            Console.ReadLine();
            MySqlDataClass DbClass = new MySqlDataClass();
            DbClass.GenerateNewCards(rand);
            Console.ReadLine();
        }

        
    }
}


