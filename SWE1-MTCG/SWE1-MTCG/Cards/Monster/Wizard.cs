using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Enum;

namespace SWE1_MTCG.Cards.Monster
{
    class Wizard : BaseCards
    {
        public Wizard(/*element type*/)
        {
            this.card_type = cardTypes.Monster;
            this.element_type = elementTypes.Normal;
            this.card_property = cardProperty.Wizard;
        }
    }
}
