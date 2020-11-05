using Cards;
using MyEnum;
using System;
using System.Collections.Generic;
using System.Text;



namespace SWE1_MTCG.Cards.Monster
{
    public class Knight : BaseCards
    {
        public Knight(int damage, string name, elementTypes element) : base(damage, name, element)
        {
            this.card_type = cardTypes.Monster;
            this.card_property = cardProperty.Knight;
        }
    }
}
