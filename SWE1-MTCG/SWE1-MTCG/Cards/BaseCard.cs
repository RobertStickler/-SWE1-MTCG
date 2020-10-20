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

		/*public BaseCards(int damage, string name)
        {
			this.card_damage = damage;
			this.card_name = name;

			this.card_type = cardTypes.Monster;
			this.element_type = elementTypes.Water;
			this.card_property = cardProperty.Dragon;
		}*/

		

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


