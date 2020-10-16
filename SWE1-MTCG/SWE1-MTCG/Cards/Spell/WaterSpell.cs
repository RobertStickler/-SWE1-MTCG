using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Enum;

namespace SWE1_MTCG.Cards.Zauber
{
    public class WaterSpell: BaseCards
    {
        public WaterSpell()
        {
            this.card_type = cardTypes.Spell;
            this.element_type = elementTypes.Water;
        }
    }
}
