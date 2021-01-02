using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using SWE1_MTCG;
namespace Server
{
    public class TradingObject
    {
        public string userUid;
        public string wantedCardType;
        public string cardUid;
        public int requiredDamage;
        
        public BaseCards card = null;
    }
}
