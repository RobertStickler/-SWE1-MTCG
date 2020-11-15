using System;
using System.Collections.Generic;
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
        public string userCardsString;

        List<BaseCards> cardCollection = new List<BaseCards>();
        List<BaseCards> cardDeck = new List<BaseCards>();
    }
}
