using Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

class SERVER
{
    
    static void Main(string[] args)
    {
        MyTcpListener server = new MyTcpListener();
        server.startServer();
    }
}


