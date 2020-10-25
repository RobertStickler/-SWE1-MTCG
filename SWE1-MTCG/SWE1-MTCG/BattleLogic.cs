using System;
using System.Collections.Generic;
using System.Text;
using Bogus.DataSets;
using Cards;
using Enum;

namespace SWE1_MTCG
{
    public class BattleLogic
    {
        public void PrintHelp()
        {
            Console.WriteLine("Hello World!");
        }
        public BaseCards Attack(BaseCards attacker, BaseCards defender)
        {

            switch (attacker.getCardType())
            {
                case cardTypes.Monster:
                    {
                        if (defender.getCardType() == cardTypes.Monster)
                        {
                            //monster attacks monster pure strenght
                            //is ledgit DAVOR testen

                            if (attacker.getCardDamage() > defender.getCardDamage())                            
                                return attacker;
                            
                            else
                                return defender;

                            
                        }
                        else if (defender.getCardType() == cardTypes.Spell)
                        {
                            //monster attacks Spell
                            //nur wenn nicht beide NORMAL sind
                            int damageAttacker = GetEffektivDemage(attacker, defender);
                            int damageDefender = GetEffektivDemage(defender, attacker);

                            if (damageAttacker > damageDefender)
                                return attacker;

                            else
                                return defender;
                        }
                        else
                        {
                            Console.WriteLine("Error by attacking!");
                        }
                        break;
                    }
                case cardTypes.Spell:
                    {
                        if (defender.getCardType() == cardTypes.Monster)
                        {
                            //nur wenn nicht beide NORMAL sind
                            int damageAttacker = GetEffektivDemage(attacker, defender);
                            int damageDefender = GetEffektivDemage(defender, attacker);

                            if (damageAttacker > damageDefender)
                                return attacker;

                            else
                                return defender;
                        }
                        else if (defender.getCardType() == cardTypes.Spell)
                        {
                            //Spell attacks Spell
                            int damageAttacker = GetEffektivDemage(attacker, defender);
                            int damageDefender = GetEffektivDemage(defender, attacker);

                            if (damageAttacker > damageDefender)
                                return attacker;

                            else
                                return defender;
                        }
                        else
                        {
                            Console.WriteLine("Error by attacking!");
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error occured!");
                        return attacker;
                    }
                    
            }
            return attacker;

        }

        public int GetEffektivDemage(BaseCards first, BaseCards second)
        {
            switch(first.getElementTypes())
            {
                case elementTypes.Fire:
                    {
                        if (second.getElementTypes() == elementTypes.Normal)
                            return first.getCardDamage() * 2;
                        else
                            return first.getCardDamage();
                    }
                case elementTypes.Water:
                    {
                        if (second.getElementTypes() == elementTypes.Fire)
                            return first.getCardDamage() * 2;
                        else
                            return first.getCardDamage();
                    }
                case elementTypes.Normal:
                    {
                        if (second.getElementTypes() == elementTypes.Water)
                            return first.getCardDamage() * 2;
                        else
                            return first.getCardDamage();
                    }
            }

            return 0;
        }

        public List<BaseCards> StartBattle(List<BaseCards> Cards4Battle1, List<BaseCards> Cards4Battle2)
        {
            Random rnd = new Random();
            int counterLoop = 0;
            int a = 0, b = 0;


            while ((Test4Winner(Cards4Battle1.Count, Cards4Battle2.Count) == false) && (counterLoop < 100))
            {

                a = Cards4Battle1.Count;
                b = Cards4Battle2.Count;

                int cardPlayer1 = rnd.Next(a);  // creates a number from 0 to 3
                int cardPlayer2 = rnd.Next(b);

                Console.WriteLine("Player one card {0}", cardPlayer1);
                Console.WriteLine("Player two card {0}", cardPlayer2);

                BaseCards Player1;
                BaseCards Player2;

                Player1 = Cards4Battle1[cardPlayer1];
                Player2 = Cards4Battle2[cardPlayer2];

                BaseCards winner = Attack(Player1, Player2);


                if (winner == Player1)
                {
                    Cards4Battle1.Add(Cards4Battle2[cardPlayer2]);
                    Cards4Battle2.Remove(Cards4Battle2[cardPlayer2]);
                }
                else if (winner == Player2)
                {
                    Cards4Battle2.Add(Cards4Battle1[cardPlayer1]);
                    Cards4Battle1.Remove(Cards4Battle1[cardPlayer1]);
                }
                //bei einem unetschieden, passiert nichts

                Console.WriteLine("Player one ammount {0}", Cards4Battle1.Count);
                Console.WriteLine("Player two ammount {0}", Cards4Battle2.Count);
                counterLoop ++ ;
                
            }

            if (a == 1)
                Console.WriteLine("The winner is Player 2");
            if (b == 1)
                Console.WriteLine("The winner is Player 1");

            return Cards4Battle1;
        }

        public bool Test4Winner(int a, int b)
        {
            bool temp = false;

            if (a == 0 || b == 0)
                return true;

            return temp;
        }

    }
}
