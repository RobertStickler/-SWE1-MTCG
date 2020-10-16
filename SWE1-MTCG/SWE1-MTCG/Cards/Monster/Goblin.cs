using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Enum;

namespace SWE1_MTCG.Cards.Monster
{
    class Goblin : BaseCards
    {
        public Goblin(/*element type*/)
        {
            this.card_type = cardTypes.Monster;
            this.element_type = elementTypes.Fire;
            this.card_property = cardProperty.Goblin;
        }
    }
}