using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Enum;

namespace SWE1_MTCG.Cards.Monster
{
    public class Wizard : BaseCards
    {
        public Wizard(int demage, string name)
        {
            this.card_type = cardTypes.Monster;
            this.element_type = elementTypes.Normal;
            this.card_property = cardProperty.Wizard;

            this.card_damage = demage;
            this.card_name = name;
        }
    }
}
