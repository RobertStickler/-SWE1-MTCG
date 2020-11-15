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

            //Console.WriteLine("1...Start Battle");
            PrintMenueOne();
            int choice = Int32.Parse(Console.ReadLine().Trim('\n'));
            //inut error handling

            ClientFunctions conetction = new ClientFunctions();

            conetction.SocketConnection(choice);
        }
        public static void PrintMenueOne()
        {
            Console.WriteLine("choose your action");
            Console.WriteLine("1...login");
            Console.WriteLine("2...register");
            Console.WriteLine("0...quit");

        }
        public static void PrintMenueTwo()
        {
            Console.WriteLine("choose your action");
            Console.WriteLine("3...Start a Battle");
            Console.WriteLine("4...Look your Deck");
            Console.WriteLine("5...Go to the Shop");
            Console.WriteLine("0...quit");

        }

    }
}


