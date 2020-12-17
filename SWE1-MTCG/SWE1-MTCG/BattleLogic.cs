using System;
using System.Collections.Generic;
using System.Text;
using Bogus.DataSets;
using Cards;
using MyEnum;

namespace SWE1_MTCG
{
    public static class BattleLogic
    {
        public static int StartBattle(List<BaseCards> Cards4Battle1, List<BaseCards> Cards4Battle2)
        {
            Random rnd = new Random();
            int counterLoop = 0;
            int a = 0, b = 0;

            List<BaseCards> Dummy = new List<BaseCards>();

            while ((Test4Winner(Cards4Battle1.Count, Cards4Battle2.Count) == false) && (counterLoop < 100))
            {
                Console.WriteLine($"----------------------------- Round {counterLoop +1} ----------------------------------");

                int cardPlayer1 = rnd.Next(Cards4Battle1.Count);  // creates a number from 0 to 3
                int cardPlayer2 = rnd.Next(Cards4Battle2.Count);

                //Console.WriteLine("Player one card {0}", cardPlayer1);
                //Console.WriteLine("Player two card {0}", cardPlayer2);

                BaseCards Player1;
                BaseCards Player2;
                BaseCards winner = null;

                Player1 = Cards4Battle1[cardPlayer1];
                Player2 = Cards4Battle2[cardPlayer2];

                PrintInformationFromOneCard(Player1, Player2);

                //validate
                if (ValidateAttack(Player1, Player2) == false)
                {
                    winner = Player2;
                }
                else
                {
                    winner = Attack(Player1, Player2);
                }


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
                counterLoop++;
            }

            if (a == 0)
            {
                Console.WriteLine("The winner is Player 2");
                return 2;
            }

            if (b == 0)
            {
                Console.WriteLine("The winner is Player 1");
                return 1;
            }


            return 0;
        }
        public static BaseCards Attack(BaseCards attacker, BaseCards defender)
        {
            int damageAttacker;
            int damageDefender;

            switch (attacker.getCardType())
            {
                case cardTypes.Monster when defender.getCardType() == cardTypes.Monster:

                    Console.WriteLine($"Der effektive Damage ist: {attacker.getCardDamage()} ");
                    Console.WriteLine($"Der effektive Damage ist: {defender.getCardDamage()} ");

                    if (attacker.getCardDamage() > defender.getCardDamage())
                        return attacker;

                    return defender;
                case cardTypes.Monster when defender.getCardType() == cardTypes.Spell:
                    // monster attacks Spell
                    // nur wenn nicht beide NORMAL sind
                    damageAttacker = GetEffektivDemage(attacker, defender);
                    damageDefender = GetEffektivDemage(defender, attacker);

                    if (damageAttacker > damageDefender)
                        return attacker;

                    return defender;

                case cardTypes.Spell when defender.getCardType() == cardTypes.Monster:


                    // nur wenn nicht beide NORMAL sind
                    damageAttacker = GetEffektivDemage(attacker, defender);
                    damageDefender = GetEffektivDemage(defender, attacker);

                    if (damageAttacker > damageDefender)
                        return attacker;

                    else
                        return defender;

                case cardTypes.Spell when defender.getCardType() == cardTypes.Spell:

                    //Spell attacks Spell
                    damageAttacker = GetEffektivDemage(attacker, defender);
                    damageDefender = GetEffektivDemage(defender, attacker);

                    if (damageAttacker > damageDefender)
                        return attacker;

                    return defender;

                default:

                    // TODO: consider throw new InvalidDataException ?
                    Console.WriteLine("Error occured!");
                    return attacker;

            }
        }

        public static int GetEffektivDemage(BaseCards first, BaseCards second)
        {
            double temp = first.getCardDamage();
            switch (first.getElementTypes())
            {
                case elementTypes.Fire:
                    {
                        if (second.getElementTypes() == elementTypes.Normal)
                        {
                            Console.WriteLine("Feuer ist effektiv gegen Normal");
                            temp = first.getCardDamage() * 1.5;
                        }                            
                        else
                            temp = first.getCardDamage();
                        break;
                    }
                case elementTypes.Water:
                    {
                        if (second.getElementTypes() == elementTypes.Fire)
                        {
                            Console.WriteLine("Wasser ist effektiv gegen Feuer");
                            temp = first.getCardDamage() * 1.5;
                        }
                            
                        else
                            temp = first.getCardDamage();
                        break;
                    }
                case elementTypes.Normal:
                    {
                        if (second.getElementTypes() == elementTypes.Water)
                        {
                            Console.WriteLine("Normal ist effektiv gegen Wasser");
                            temp = first.getCardDamage() * 1.5;
                        }                            
                        else
                            temp = first.getCardDamage();
                        break;
                    }
            }
            Console.WriteLine($"Der effektive Damage ist: {Convert.ToInt32(temp)} ");
            return Convert.ToInt32(temp);
        }

       

        public static bool Test4Winner(int a, int b)
        {
            bool temp = false;

            if (a == 0 || b == 0)
                return true;

            return temp;
        }

        public static bool ValidateAttack(BaseCards Player1, BaseCards Player2)
        {
            if ((Player1.getCardProperty() == cardProperty.Goblin) && (Player2.getCardProperty() == cardProperty.Dragon))
            {
                Console.WriteLine("Goblin cannot attack Dragon");
                return false;
            }
            else if ((Player1.getCardProperty() == cardProperty.Ork) && (Player2.getCardProperty() == cardProperty.Wizard))
            {
                Console.WriteLine("Org cannot attack Wizard");
                return false;
            }
            else if ((Player1.getCardProperty() == cardProperty.Knight) && (Player2.getCardType() == cardTypes.Spell) && (Player2.getElementTypes() == elementTypes.Water))
            {
                Console.WriteLine("Knight cannot attack WaterSpell");
                return false;
            }
            else if ((Player1.getCardProperty() == cardProperty.Kraken) && (Player2.getCardType() == cardTypes.Spell))
            {
                Console.WriteLine("Kraken is immune to Spells");
                return false;
            }
            else if ((Player1.getCardProperty() == cardProperty.Dragon) && (Player2.getCardProperty() == cardProperty.Elf) && (Player2.getElementTypes() == elementTypes.Fire))
            {
                Console.WriteLine("Dragon cannot attack FireElves");
                return false;
            }


            return true;
        }
        static void PrintInformationFromOneCard(BaseCards Player1, BaseCards Player2)
        {
            Console.WriteLine($"{Player1.getCardType()}, {Player1.getElementTypes()}, {CheckCardtype(Player1)} {Player1.getCardName()}, | attacks | {Player2.getCardType()}, {Player2.getElementTypes()}, {CheckCardtype(Player2)} {Player2.getCardName()}");
                       

        }
        static string CheckCardtype(BaseCards Player)
        {
            if (Player.getCardType() == cardTypes.Monster)
            {
                return Player.getCardProperty().ToString() + ", ";
            }
            return "";
        }


    }
}
