using Cards;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using MyEnum;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Zauber;

namespace SWE1_MTCG
{
    public class CardShop
    {
        static Random rand = new Random();
        public static BaseCards GetRandCard()
        {
            int desicionMonsterOrSpell = rand.Next(6); //generiert eine Rand Zahl die kleiner als 6 ist 
            bool isMonster = false;
            BaseCards tempCard;

            //als erstes sollten wir entscheiden, ob es eine Zauber oder eine Monsterkarte wird
            if (desicionMonsterOrSpell <= 3)
            {
                //dann ist es ein Monster
                isMonster = true;
            }

            if (isMonster == true)
            {
                //wenns ein Monster is, erstellen wir ein Monster
                tempCard = MakeMonster();
            }
            else
            {
                //eine Zauberkarte erstellen
                tempCard = MakeSpell();
            }

            return tempCard;
        }
        public static BaseCards MakeSpell()
        {
            BaseCards tempCard = null;

            //entscheiden welche Eigenschaft also elementTypes
            int randElement = ChooseElement();

            //entschieden wie viel damage
            int damage = rand.Next(51);

            //irgwie da mit nem namen machen
            string name = "tempname";

            switch (randElement)
            {
                case 0:
                    {
                        tempCard = new FireSpell(damage, name);
                        break;
                    }
                case 1:
                    {
                        tempCard = new WaterSpell(damage, name);
                        break;
                    }
                case 2:
                    {
                        tempCard = new NormalSpell(damage, name);
                        break;
                    }
            }      
           return tempCard;
        }

        static int ChooseProperty()
        {
            int cardPropertyLen = Enum.GetNames(typeof(cardProperty)).Length; //beginnt bei 1 zu zählen also 7
            int randType = rand.Next(cardPropertyLen); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randType;
        }

        static int ChooseElement()
        {
            int elementTypesLen = Enum.GetNames(typeof(elementTypes)).Length; //beginnt bei 1 zu zählen also 7
            int randElement = rand.Next(elementTypesLen); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randElement;
        }


        public static BaseCards MakeMonster()
        {
            BaseCards tempCard = null;

            //entscheiden welchen typ also cardProperty            
            int randType = ChooseProperty();

            //entscheiden welche Eigenschaft also elementTypes
            int randElement = ChooseElement();

            //entschieden wie viel damage
            int damage = rand.Next(51);

            //irgwie da mit nem namen machen
            string name = "tempname";

            //set the cardProperty
            string card_property = Enum.GetName(typeof(elementTypes), randElement);
            elementTypes tempElement = ((elementTypes)Enum.Parse(typeof(elementTypes), card_property));

            switch (randType)
            {
                case 0:
                    {                        
                        tempCard = new Dragon(damage, name, tempElement);
                        break;
                    }
                case 1:
                    {
                        tempCard = new Knight(damage, name, tempElement);
                        break;
                    }
                case 2:
                    {
                        tempCard = new Org(damage, name, tempElement);
                        break;
                    }
                case 3:
                    {
                        tempCard = new Wizard(damage, name, tempElement);
                        break;
                    }
                case 4:
                    {
                        tempCard = new Goblin(damage, name, tempElement);
                        break; ;
                    }
                case 5:
                    {
                        tempCard = new Kraken(damage, name, tempElement);
                        break; ;
                    }
                case 6:
                    {
                        tempCard = new Elf(damage, name, tempElement);
                        break; ;
                    }
            }
            return tempCard;
        }
    }
}
