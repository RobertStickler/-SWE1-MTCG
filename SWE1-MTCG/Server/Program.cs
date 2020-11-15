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
            
           
            //var request = mysql.GetOneUser("Robert");
            ServerClientConnection.startServer(); 
        }
        

        
    }
}


