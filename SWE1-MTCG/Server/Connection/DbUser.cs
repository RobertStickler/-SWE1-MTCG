using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Cards;
using SWE1_MTCG;


namespace Server
{
    public class DbUser
    {
        public string userName;
        public string uid;
        public string email;
        public string pwd;
        public int coins;
        public int elo;
        public NetworkStream stream = null;

        public List<BaseCards> cardCollection = new List<BaseCards>();
        public List<BaseCards> cardDeck = new List<BaseCards>();
    }
}
