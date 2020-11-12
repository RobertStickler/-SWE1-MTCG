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
    class Server
    {
        static void Main(string[] args)
        {

            MySqlDataClass mysql = new MySqlDataClass();
            //mysql.runQuery("Select * from cardcollection;");
            /*List<BaseCards> liste =  mysql.getCardsFromDB();
            

            foreach (BaseCards element in liste)
            {
                Console.WriteLine("{0} {1} {2} {3}",element.getCardName(), element.getCardType(), element.getElementTypes(), element.getCardProperty());
            } */
            var request = mysql.GetUser();
            //ConnectionEstablishment.startServer();
        }
        

        
    }
}


