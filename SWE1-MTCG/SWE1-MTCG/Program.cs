using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cards;
using MyEnum;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Zauber;

namespace SWE1_MTCG
{
    class Program
    {
        public static void Main(string[] args)
        {
            Random rand = new Random();

            List<BaseCards> Cards4Battle1 = new List<BaseCards>();
            List<BaseCards> Cards4Battle2 = new List<BaseCards>();

            Cards4Battle1.Add(CardShop.GetRandCard(rand));
            Cards4Battle1.Add(CardShop.GetRandCard(rand));
            Cards4Battle1.Add(CardShop.GetRandCard(rand));
            Cards4Battle1.Add(CardShop.GetRandCard(rand));

            Cards4Battle2.Add(CardShop.GetRandCard(rand));
            Cards4Battle2.Add(CardShop.GetRandCard(rand));
            Cards4Battle2.Add(CardShop.GetRandCard(rand));
            Cards4Battle2.Add(CardShop.GetRandCard(rand));

            int Sieger = BattleLogic.StartBattle(Cards4Battle1, Cards4Battle2);
            Console.WriteLine(Sieger);
        }
    }  
    
}