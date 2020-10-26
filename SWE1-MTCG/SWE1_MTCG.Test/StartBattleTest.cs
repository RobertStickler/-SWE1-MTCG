using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enum;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Zauber;
using SWE1_MTCG;
using Cards;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class StartBattleTest
    {
        BattleLogic battleLogic = new BattleLogic();

        [Test]
        public void TestMethod()
        {
            //arrange
            List<BaseCards> Cards4Battle1 = new List<BaseCards>();
            Cards4Battle1.Add(new Goblin(26, "Acnologia", elementTypes.Water));
            Cards4Battle1.Add(new WaterSpell(10, "Magic Storm"));
            Cards4Battle1.Add(new Kraken(5, "Der Kraken", elementTypes.Water));
            Cards4Battle1.Add(new FireSpell(8, "Sunny Day"));

            List<BaseCards> Cards4Battle2 = new List<BaseCards>();
            Cards4Battle2.Add(new Dragon(25, "Dragneel", elementTypes.Fire));
            Cards4Battle2.Add(new WaterSpell(10, "Magic Storm"));
            Cards4Battle2.Add(new Org(4, "Der Kraken", elementTypes.Normal));
            Cards4Battle2.Add(new FireSpell(8, "Sunny Day"));

            //act
            int sieger = battleLogic.StartBattle(Cards4Battle1, Cards4Battle2);
            //assert
            Assert.AreNotEqual(sieger, 0);
        }
    }
}
