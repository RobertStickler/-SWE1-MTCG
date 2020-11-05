using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using MyEnum;

namespace SWE1_MTCG.Cards.Monster
{
    public class Wizard : BaseCards
    {
        public Wizard(int damage, string name, elementTypes element) : base(damage, name, element)
        {
            this.card_type = cardTypes.Monster;
            this.card_property = cardProperty.Wizard;
        }
    }
}
