using SWE1_MTCG;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class MessageHandler
    {
        public void ManageMessages(RequestContext request)
        {
            
            //if um die einzelnen Bedingungen abzufragen
            if (request.method == "GET")
            {
                if (request.path == "/messages")
                {
                    //lists all messages
                    TCPClass.GetAllMessages(Liste);
                }
                else
                {
                    //list messege with number
                    int number = TCPClass.GetNumber(request.path);
                    TCPClass.GetOneMessages(Liste, number);
                }
            }
            else if (request.method == "POST")
            {
                //add message
                if (request.path == "/messages")
                {
                    request = TCPClass.AddMessage(request); //erstellt die uid
                    int index = TCPClass.IndexFinder(Liste); //findet einen freien platz
                    Liste.Insert(index, request); //fügt die message hinzu
                    Console.WriteLine("message added");
                }
                else
                {
                    Console.WriteLine("Error: /messages as path expected");
                }
            }
            else if (request.method == "PUT")
            {
                //update the message
                if (request.path != "/messages")
                {
                    int number = TCPClass.GetNumber(request.path); //sucht die nummer aus dem Path
                    TCPClass.UpdateMessage(request.message, Liste, number); //ändert die message an der stelle number
                }
                else
                {
                    Console.WriteLine("Error: /messages/ + number expected");
                }

            }
            else if (request.method == "DELETE")
            {
                if (request.path != "/messages")
                {
                    int number = TCPClass.GetNumber(request.path); //nummer suchen
                    TCPClass.DeleteMessage(Liste, number); //eig wird nur die uid an der stelle auf 0 gesetzt
                    Console.WriteLine("message {0} deleted", number);
                }
                else
                {
                    Console.WriteLine("Error: /messages/ + number expected");
                }
            }
            else
            {
                tcpClass.PrintUsage();
            }




            return;
        }





    }
}
