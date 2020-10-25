using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Enum;

namespace SWE1_MTCG.Cards.Zauber
{
    public class FireSpell : BaseCards
    {
        public FireSpell(int damage, string name) : base(damage, name)
        {
            this.card_type = cardTypes.Spell;
            this.element_type = elementTypes.Fire;
        }

    }
}
