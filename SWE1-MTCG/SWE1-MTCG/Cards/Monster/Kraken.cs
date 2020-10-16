﻿using System;
using System.Collections.Generic;
using System.Text;
using Cards;
using Enum;

namespace SWE1_MTCG.Cards.Monster
{
    class Kraken : BaseCards
    {
        public Kraken(/*element type*/)
        {
            this.card_type = cardTypes.Monster;
            this.element_type = elementTypes.Water;
            this.card_property = cardProperty.Kraken;
        }
    }
}
