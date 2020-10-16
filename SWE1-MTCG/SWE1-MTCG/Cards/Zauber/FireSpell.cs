using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Enum;

namespace SWE1_MTCG.Cards.Zauber
{
    class FireSpell : BaseCards
    {
        public FireSpell()
        {
            this.card_type = cardTypes.Spell;
            this.element_type = elementTypes.Fire;
        }
    }
}
