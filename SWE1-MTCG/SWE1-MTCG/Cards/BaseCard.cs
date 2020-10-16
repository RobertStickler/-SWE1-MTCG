using System;
using System.Net.NetworkInformation;
using Enum;


namespace Cards
{
	public abstract class BaseCards
	{
		protected elementTypes element_type;
		protected cardTypes card_type;
		protected cardProperty card_property;

		protected int card_damage;
		protected string card_name;


		public int getCardDamage()
		{
			return card_damage;
		}
		public string getCardName()
		{
			return card_name;
		}
	}
}


