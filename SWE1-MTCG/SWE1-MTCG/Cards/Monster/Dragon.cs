using Bogus.DataSets;
using Cards;
using Enum;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SWE1_MTCG.Cards.Monster
{
    class Dragon : BaseCards
    {
        public Dragon(/*element type*/)
        {
            this.card_type = cardTypes.Monster;
            this.element_type = elementTypes.Water;
            this.card_property = cardProperty.Dragon;
        }
    }
}
