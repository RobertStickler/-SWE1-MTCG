using Cards;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using MyEnum;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Zauber;
using NamesCollection;
using Microsoft.VisualBasic.FileIO;


namespace SWE1_MTCG
{
    public class CardShop
    {
        //static Random rand = new Random(); kommt jz vom server, um verschiedene Systemzeiten zu haben
        
        public static BaseCards GetRandCard(Random rand)
        {
            int desicionMonsterOrSpell =rand.Next(6); //generiert eine Rand Zahl die kleiner als 6 ist 
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
                tempCard = MakeMonster(rand);
            }
            else
            {
                //eine Zauberkarte erstellen
                tempCard = MakeSpell(rand);
            }

            return tempCard;
        }
        public static BaseCards MakeSpell(Random rand)
        {
            BaseCards tempCard = null;

            //uid erstellen
            string teststring = string.Format("{0:N}", Guid.NewGuid()); //erstellt eine unique Id 
            string uid = teststring.Substring(0, 9); //kürzt die ID auf 9 stellen

            //entscheiden welche Eigenschaft also elementTypes
            int randElement = ChooseElement(rand);

            //entschieden wie viel damage
            int damage = rand.Next(51);

            //irgwie da mit nem namen machen
            string name = CreateNameSpell(rand);

            //set the cardProperty
            string card_Element = Enum.GetName(typeof(elementTypes), randElement);
            elementTypes tempElement = ((elementTypes)Enum.Parse(typeof(elementTypes), card_Element));

            tempCard = new SpellCard(uid, damage, name, tempElement);
           return tempCard;
        }

        static int ChooseProperty(Random rand)
        {
            int cardPropertyLen = Enum.GetNames(typeof(cardProperty)).Length; //beginnt bei 1 zu zählen also 7
            int randType = rand.Next(cardPropertyLen); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randType;
        }

        static int ChooseElement(Random rand)
        {
            int elementTypesLen = Enum.GetNames(typeof(elementTypes)).Length; //beginnt bei 1 zu zählen also 7
            int randElement = rand.Next(elementTypesLen); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randElement;
        }


        public static BaseCards MakeMonster(Random rand)
        {
            BaseCards tempCard = null;

            //uid erstellen
            string teststring = string.Format("{0:N}", Guid.NewGuid()); //erstellt eine unique Id 
            string uid = teststring.Substring(0, 9); //kürzt die ID auf 9 stellen

            //entscheiden welchen typ also cardProperty            
            int randType = ChooseProperty(rand);

            //entscheiden welche Eigenschaft also elementTypes
            int randElement = ChooseElement(rand);

            //entschieden wie viel damage
            int damage = rand.Next(1, 51);

            //irgwie da mit nem namen machen
            string name = CreateNameMonster(rand);

            //set the cardProperty
            string card_Element = Enum.GetName(typeof(elementTypes), randElement);
            elementTypes tempElement = ((elementTypes)Enum.Parse(typeof(elementTypes), card_Element));

            string card_property = Enum.GetName(typeof(cardProperty), randType);
            cardProperty tempProperty = ((cardProperty)Enum.Parse(typeof(cardProperty), card_property));


            tempCard = new MonsterCard(uid, damage, name, tempElement, tempProperty);

            return tempCard;
                    
            }
        static int ChooseMainName(Random rand)
        {
            int temp = Enum.GetNames(typeof(CardMainNamesMonster)).Length; //beginnt bei 1 zu zählen also 7
            int randElement = rand.Next(temp); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randElement;
        }
        static int ChooseMainNameSpell(Random rand)
        {
            int temp = Enum.GetNames(typeof(CardMainNamesSpell)).Length; //beginnt bei 1 zu zählen also 7
            int randElement = rand.Next(temp); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randElement;
        }
        static int ChooseAttribute(Random rand)
        {
            int temp = Enum.GetNames(typeof(CardAttributeInNameMonster)).Length; //beginnt bei 1 zu zählen also 7
            int randElement = rand.Next(temp); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randElement;
        }
        static int ChooseType(Random rand)
        {
            int temp = Enum.GetNames(typeof(CardTypeForNameMonster)).Length; //beginnt bei 1 zu zählen also 7
            int randElement = rand.Next(temp); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randElement;
        }
        public static string CreateNameMonster(Random rand)
        {
            string mainName = Enum.GetName(typeof(CardMainNamesMonster), ChooseMainName(rand));
            string attribute = Enum.GetName(typeof(CardAttributeInNameMonster), ChooseAttribute(rand));
            string type = Enum.GetName(typeof(CardTypeForNameMonster), ChooseType(rand));

            string temp = mainName + " the " + attribute + " " + type;
            return temp;
        }
        public static string CreateNameSpell(Random rand)
        {
            string mainName = Enum.GetName(typeof(CardMainNamesSpell), ChooseMainNameSpell(rand));
            string attribute = Enum.GetName(typeof(elementTypes), ChooseElement(rand));

            string temp = attribute + " " + mainName  ;
            return temp;
        }
    }
}
