using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using MyEnum;

namespace SWE1_MTCG.Cards.Zauber
{
    public class NormalSpell : BaseCards
    {
        public NormalSpell (int damage, string name) : base(damage, name)
        {
            this.card_type = cardTypes.Spell;
            this.element_type = elementTypes.Normal;
        }
    }
}
