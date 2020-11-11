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
        public static void AddToBattleQueue(RequestContext request)
        {
            //warten
            List<RequestContext> battleQueue = new List<RequestContext>();
            battleQueue.Add(request);

            while(true)
            {
                if(battleQueue.Count == 2)
                {
                    //start battle
                }


            }
            //start the battel
                    Console.WriteLine("two players found!");


        }

    }
}
