using System;
using System.Collections.Generic;
using Cards;
using Enum;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Zauber;

namespace SWE1_MTCG
{
    class Program
    {
        public static void Main(string[] args)
        {
            BattleLogic battleLogic = new BattleLogic();

            //Dragon _dragon = new Dragon(25, "Acnologia", elementTypes.Water);
            //FireSpell _dragonBreath = new FireSpell(10, "attackspelll");

            //battleLogic.PrintHelp();

            //Zwei Decks für das Battle erstellen
            List<BaseCards> Cards4Battle1 = new List<BaseCards>();
            Cards4Battle1.Add(new Dragon(26, "Acnologia", elementTypes.Water));
            Cards4Battle1.Add(new WaterSpell(10, "Magic Storm"));
            Cards4Battle1.Add(new Kraken(5, "Der Kraken", elementTypes.Water));
            Cards4Battle1.Add(new FireSpell(8, "Sunny Day"));

            if(Cards4Battle1.Count > 4)
                Console.WriteLine("Error: to many Cards");

            List<BaseCards> Cards4Battle2 = new List<BaseCards>();
            Cards4Battle2.Add(new Dragon(25, "Dragneel", elementTypes.Fire));
            Cards4Battle2.Add(new WaterSpell(10, "Magic Storm"));
            Cards4Battle2.Add(new Org(4, "Der Kraken", elementTypes.Normal));
            Cards4Battle2.Add(new FireSpell(8, "Sunny Day"));

            if (Cards4Battle2.Count > 4)
                Console.WriteLine("Error: to many Cards");

            //Console.WriteLine(Cards4Battle1[0].getCardName()); //Output: Acnologia

            battleLogic.StartBattle(Cards4Battle1, Cards4Battle2);

        }
    }

    
    
}