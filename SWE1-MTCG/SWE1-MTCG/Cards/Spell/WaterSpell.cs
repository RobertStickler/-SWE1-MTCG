using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Enum;

namespace SWE1_MTCG.Cards.Zauber
{
    public class WaterSpell: BaseCards
    {
        public WaterSpell(int damage, string name) : base(damage, name)
        {
            this.card_type = cardTypes.Spell;
            this.element_type = elementTypes.Water;
        }
    }
}
