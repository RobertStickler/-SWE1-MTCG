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
                var playerOne = Liste[0];
                var playerTwo = Liste[1];
                Liste.RemoveAt(0);
                Liste.RemoveAt(1);
                string username = "";
                
                int sieger = BattleLogic.StartBattle(playerOne.cardDeck, playerTwo.cardDeck);
                
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
