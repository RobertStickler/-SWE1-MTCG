using System;
using System.Collections.Generic;
using System.Text;
using SWE1_MTCG;
using Cards;


namespace Server
{
    class BattleMaker
    {
        public static List<BaseCards> GetRandCards()
        {
            List<BaseCards> Cards4Battle1 = new List<BaseCards>();
            
            Cards4Battle1.Add(CardShop.GetRandCard());
            Cards4Battle1.Add(CardShop.GetRandCard());
            Cards4Battle1.Add(CardShop.GetRandCard());
            Cards4Battle1.Add(CardShop.GetRandCard());

            return Cards4Battle1;
        }
        public static string AddToBattleQueue(List<RequestContext> Liste)
        {

            if((Liste.Count >=  2))
            {
                //start battle
                string username = "";
                int sieger = BattleLogic.StartBattle(Liste[0].cardDeck, Liste[1].cardDeck);
                
                if(sieger == 1)
                {
                    username = Liste[0].GetUsernameFromDict();
                    return username;
                }
                if (sieger == 2)
                {
                    username = Liste[1].GetUsernameFromDict();
                    return username;
                }
                return "noOne";
            }
            //start the battel
            //Console.WriteLine("two players found!");

            return "noOne";
        }

    }
}
