using System.Collections.Generic;
using Cards;
using MyEnum;
using NUnit.Framework;
using SWE1_MTCG.Cards.Monster;
using SWE1_MTCG.Cards.Zauber;

namespace SWE1_MTCG.Test
{
    [TestFixture]
    public class BattleLogicTest
    {
        private MonsterCard _firedragon;
        private SpellCard _waterSpell;

        [SetUp]
        public void Setup()
        {
            //waterSpell ist schwächer, wegen des elements gewinnt er aber den kampf
            _firedragon = new MonsterCard("sadf34s", 35, "Der Test Drache", elementTypes.Fire, cardProperty.Dragon);
            _waterSpell = new SpellCard("pouicyx23", 25, "Test Water Spell", elementTypes.Water);
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            //act
            BaseCards winner = BattleLogic.Attack(_firedragon, _waterSpell);
            //assert
            Assert.AreEqual(_waterSpell.getCardDamage(), winner.getCardDamage());
        }
    }

    public class DemageTestFireWater
    {
        private MonsterCard _firedragon;
        private SpellCard _waterSpell;

        [SetUp]
        public void Setup()
        {
            _firedragon = new MonsterCard("sadf34s", 40, "Der Test Drache", elementTypes.Fire, cardProperty.Dragon);
            _waterSpell = new SpellCard("pouicyx23", 25, "Test Water Spell", elementTypes.Water);
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            //act
            int a = BattleLogic.GetEffektivDemage(_firedragon, _waterSpell);
            int b = BattleLogic.GetEffektivDemage(_waterSpell, _firedragon);
            //assert
            Assert.IsTrue(a > b);
        }
    }

    public class DemageTestNormalWater
    {
        private MonsterCard _normaldragon;
        private SpellCard _waterSpell;

        [SetUp]
        public void Setup()
        {
            _normaldragon = new MonsterCard("sadf34s", 25, "Der Test Drache", elementTypes.Normal, cardProperty.Dragon);
            _waterSpell = new SpellCard("pouicyx23", 35, "Test Water Spell", elementTypes.Water);
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            //act
            int a = BattleLogic.GetEffektivDemage(_normaldragon, _waterSpell);
            int b = BattleLogic.GetEffektivDemage(_waterSpell, _normaldragon);
            //assert
            Assert.IsTrue(a > b);
        }
    }

    public class DemageTestFireNormal
    {
        private MonsterCard _normaldragon;
        private SpellCard _fireSpell;

        [SetUp]
        public void Setup()
        {
            _normaldragon = new MonsterCard("sadf34s", 35, "Der Test Drache", elementTypes.Normal, cardProperty.Dragon);
            _fireSpell = new SpellCard("pouicyx23", 25, "Test Water Spell", elementTypes.Fire);
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            //act
            int a = BattleLogic.GetEffektivDemage(_normaldragon, _fireSpell);
            int b = BattleLogic.GetEffektivDemage(_fireSpell, _normaldragon);
            //assert
            Assert.IsTrue(a < b);
        }
    }

    public class GoblinsVsDragon
    {
        private MonsterCard _normaldragon;
        private MonsterCard _fireGoblins;

        [SetUp]
        public void Setup()
        {
            _normaldragon = new MonsterCard("sadf34s", 1, "Der Test Drache", elementTypes.Normal, cardProperty.Dragon);
            _fireGoblins = new MonsterCard("asr345gf", 50, "Der Test Goblin", elementTypes.Fire, cardProperty.Goblin);
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            //act
            bool winner = BattleLogic.ValidateAttack(_fireGoblins, _normaldragon);
            //assert
            Assert.AreEqual(winner, false);
        }
    }

    public class WizardVsOrk
    {
        private MonsterCard _waterOrk;
        private MonsterCard _fireWizard;

        [SetUp]
        public void Setup()
        {
            _fireWizard = new MonsterCard("sadf34s", 1, "Der Test WIzard", elementTypes.Fire, cardProperty.Wizard);
            _waterOrk = new MonsterCard("asr345gf", 50, "Der Test GOblin", elementTypes.Water, cardProperty.Ork);
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            //act
            bool winner = BattleLogic.ValidateAttack(_waterOrk, _fireWizard);
            //assert
            Assert.AreEqual(winner, false);
        }
    }

    public class KnightVsWaterSpell
    {
        private MonsterCard _knight;
        private SpellCard _waterSpell;

        [SetUp]
        public void Setup()
        {
            _knight = new MonsterCard("sadf34s", 1, "Der Test Knight", elementTypes.Fire, cardProperty.Knight);
            _waterSpell = new SpellCard("pouicyx23", 35, "Test Water Spell", elementTypes.Water);
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            //act
            bool winner = BattleLogic.ValidateAttack(_knight, _waterSpell);
            //assert
            Assert.AreEqual(winner, false);

        }
    }

    public class KrakenVsSpell
    {
        private MonsterCard _kraken;
        private SpellCard _waterSpell, _fireSpell, _normalSpell;

        [SetUp]
        public void Setup()
        {
            _kraken = new MonsterCard("sadf34s", 1, "Der Test Knight", elementTypes.Water, cardProperty.Kraken);
            _waterSpell = new SpellCard("pouicyx23", 35, "Test Water Spell", elementTypes.Water);
            _fireSpell = new SpellCard("pouicyx23", 35, "Test Fire Spell", elementTypes.Fire);
            _normalSpell = new SpellCard("pouicyx23", 35, "Test Normal Spell", elementTypes.Normal);
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            //act
            bool winner1 = BattleLogic.ValidateAttack(_kraken, _waterSpell);
            bool winner2 = BattleLogic.ValidateAttack(_kraken, _fireSpell);
            bool winner3 = BattleLogic.ValidateAttack(_kraken, _normalSpell);
            //assert
            Assert.AreEqual(winner1, false);
            Assert.AreEqual(winner2, false);
            Assert.AreEqual(winner3, false);
        }
    }

    public class DragonsVsFireElves
    {
        private MonsterCard _dragon;
        private MonsterCard _fireElves;

        [SetUp]
        public void Setup()
        {
            _dragon = new MonsterCard("sadf34s", 50, "Der Test Dragon", elementTypes.Water, cardProperty.Dragon);
            _fireElves = new MonsterCard("asdf234", 1, "Der Feuere Elf", elementTypes.Fire, cardProperty.Elf);
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            //act
            bool winner = BattleLogic.ValidateAttack(_dragon, _fireElves);
            //assert
            Assert.AreEqual(winner, false);
        }
    }

    public class StartBattleTest
    {

        private MonsterCard _firedragon, _fireork, _waterdragon, _waterKnight, _fireWizard;
        private SpellCard _waterSpell, _normalSpell, _normaloSpello;

        [SetUp]
        public void Setup()
        {
            _firedragon = new MonsterCard("sadf34s", 40, "Der Test Drache", elementTypes.Fire, cardProperty.Dragon);
            _waterSpell = new SpellCard("pouicyx23", 25, "Test Water Spell", elementTypes.Water);
            _fireork = new MonsterCard("se6v23mgh", 13, "Der Test Org", elementTypes.Fire, cardProperty.Ork);
            _normalSpell = new SpellCard("c465mgf", 23, "Test Normal Spell", elementTypes.Normal);

            _waterdragon = new MonsterCard("sdrtbnhg", 40, "Der Test Drache2", elementTypes.Water, cardProperty.Dragon);
            _waterKnight = new MonsterCard("sdfefwkj9", 34, "Der Test Krieger", elementTypes.Water, cardProperty.Knight);
            _fireWizard = new MonsterCard("oz5evhg", 42, "Der Test Hexer Merlin", elementTypes.Fire, cardProperty.Wizard);
            _normaloSpello = new SpellCard("pouicyx23", 27, "Test Water Spell", elementTypes.Water);
        }

        [Test]
        public void TestMethod()
        {
            //arrange
            List<BaseCards> Cards4Battle1 = new List<BaseCards>();
            Cards4Battle1.Add(_firedragon);
            Cards4Battle1.Add(_waterSpell);
            Cards4Battle1.Add(_fireork);
            Cards4Battle1.Add(_normalSpell);

            List<BaseCards> Cards4Battle2 = new List<BaseCards>();
            Cards4Battle2.Add(_waterdragon);
            Cards4Battle2.Add(_waterKnight);
            Cards4Battle2.Add(_fireWizard);
            Cards4Battle2.Add(_normaloSpello);

            //act
            int sieger = BattleLogic.StartBattle(Cards4Battle1, Cards4Battle2);
            //assert
            Assert.AreNotEqual(sieger, 0); //0 wird returnt, wenn fehler
        }

    }
}
