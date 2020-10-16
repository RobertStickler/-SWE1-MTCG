using Cards;
using Enum;
using System;
using System.Collections.Generic;
using System.Text;



namespace SWE1_MTCG.Cards.Monster
{
    class Knight : BaseCards
    {
        public Knight(/*element type*/)
        {
            this.card_type = cardTypes.Monster;
            this.element_type = elementTypes.Normal;
            this.card_property = cardProperty.Knight;
        }
    }
}
