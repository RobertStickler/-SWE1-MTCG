using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Enum;

namespace SWE1_MTCG.Cards.Monster
{
    public class Kraken : BaseCards
    {
        public Kraken (int demage, string name)
        {
            this.card_type = cardTypes.Monster;
            this.element_type = elementTypes.Water;
            this.card_property = cardProperty.Kraken;

            this.card_damage = demage;
            this.card_name = name;
        }
    }
}
