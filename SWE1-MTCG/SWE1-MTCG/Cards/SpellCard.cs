using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using MyEnum;

namespace SWE1_MTCG.Cards.Zauber
{
    public class SpellCard : BaseCards
    {
        public SpellCard(string uid, int damage, string name, elementTypes element_types) : base(damage, name)
        {
            this.card_type = cardTypes.Spell;
            this.element_type = element_types;
            this.uid = uid;
        }

    }
}
