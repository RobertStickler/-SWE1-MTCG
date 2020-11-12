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
            string name = CreateNameSpell();

            //set the cardProperty
            string card_Element = Enum.GetName(typeof(elementTypes), randElement);
            elementTypes tempElement = ((elementTypes)Enum.Parse(typeof(elementTypes), card_Element));

            tempCard = new SpellCard(damage, name, tempElement);
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
            string name = CreateNameMonster();

            //set the cardProperty
            string card_Element = Enum.GetName(typeof(elementTypes), randElement);
            elementTypes tempElement = ((elementTypes)Enum.Parse(typeof(elementTypes), card_Element));

            string card_property = Enum.GetName(typeof(cardProperty), randType);
            cardProperty tempProperty = ((cardProperty)Enum.Parse(typeof(cardProperty), card_property));


            tempCard = new MonsterCard(damage, name, tempElement, tempProperty);

            return tempCard;
                    
            }
        static int ChooseMainName()
        {
            int temp = Enum.GetNames(typeof(cardMainNamesMonster)).Length; //beginnt bei 1 zu zählen also 7
            int randElement = rand.Next(temp); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randElement;
        }
        static int ChooseMainNameSpell()
        {
            int temp = Enum.GetNames(typeof(cardMainNamesSpell)).Length; //beginnt bei 1 zu zählen also 7
            int randElement = rand.Next(temp); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randElement;
        }
        static int ChooseAttribute()
        {
            int temp = Enum.GetNames(typeof(cardAttributeInNameMonster)).Length; //beginnt bei 1 zu zählen also 7
            int randElement = rand.Next(temp); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randElement;
        }
        static int ChooseType()
        {
            int temp = Enum.GetNames(typeof(cardTypeForNameMonster)).Length; //beginnt bei 1 zu zählen also 7
            int randElement = rand.Next(temp); //generiert dann zahlen von 0 bis 6, also auch 7  insgesamt
            return randElement;
        }
        public static string CreateNameMonster()
        {
            string mainName = Enum.GetName(typeof(cardMainNamesMonster), ChooseMainName());
            string attribute = Enum.GetName(typeof(cardAttributeInNameMonster), ChooseAttribute());
            string type = Enum.GetName(typeof(cardTypeForNameMonster), ChooseType());

            string temp = mainName + " the " + attribute + " " + type;
            return temp;
        }
        public static string CreateNameSpell()
        {
            string mainName = Enum.GetName(typeof(cardMainNamesSpell), ChooseMainNameSpell());
            string attribute = Enum.GetName(typeof(elementTypes), ChooseElement());

            string temp = attribute + " " + mainName  ;
            return temp;
        }
    }
}
