using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEnum;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Zauber;
using SWE1_MTCG;
using Cards;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class AttackTest
    {
        BattleLogic battleLogic = new BattleLogic();
        private Dragon _dragon;
        private FireSpell _fireSpell;

        [SetUp]
        public void Setup()
        {
            _dragon = new Dragon(25, "Ancalagon", elementTypes.Water);
            _fireSpell = new FireSpell(25, "Gnorsig");
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            //act
            BaseCards winner = battleLogic.Attack(_dragon, _fireSpell);
            //assert
            Assert.AreEqual(_dragon.getCardDamage(), winner.getCardDamage());
        }
    }
    public class DemageTest
    {
        BattleLogic battleLogic = new BattleLogic();
        private Dragon _dragon;
        private FireSpell _fireSpell;

        [SetUp]
        public void Setup()
        {
            _dragon = new Dragon(25, "Ancalagon", elementTypes.Water);
            _fireSpell = new FireSpell(25, "Gnorsig");
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            //act
            int a = battleLogic.GetEffektivDemage(_dragon, _fireSpell);
            int b = battleLogic.GetEffektivDemage(_fireSpell, _dragon);
            //assert
            Assert.IsTrue(a > b);
        }
    }
}
