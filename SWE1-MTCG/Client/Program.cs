using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Client
{
    public class Program
    {
        public static void Main()
        {
            ClientFunctions conetction = new ClientFunctions();

            conetction.SocketConnection();
        }
        
    }
}


