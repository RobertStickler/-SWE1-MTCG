using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Enum;

namespace SWE1_MTCG.Cards.Monster
{
    public class Org : BaseCards
    {
        public Org(int demage, string name)
        {
            this.card_type = cardTypes.Monster;
            this.element_type = elementTypes.Fire;
            this.card_property = cardProperty.Org;

            this.card_damage = demage;
            this.card_name = name;
        }
    }
}
