using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Enum;

namespace SWE1_MTCG.Cards.Zauber
{
    class NormalSpell : BaseCards
    {
        public NormalSpell()
        {
            this.card_type = cardTypes.Spell;
            this.element_type = elementTypes.Normal;
        }
    }
}
