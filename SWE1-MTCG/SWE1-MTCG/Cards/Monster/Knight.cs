using Cards;
using Enum;
using System;
using System.Collections.Generic;
using System.Text;



namespace SWE1_MTCG.Cards.Monster
{
    public class Knight : BaseCards
    {
        public Knight(int demage, string name)
        {
            this.card_type = cardTypes.Monster;
            this.element_type = elementTypes.Normal;
            this.card_property = cardProperty.Knight;

            this.card_damage = demage;
            this.card_name = name;
        }
    }
}
