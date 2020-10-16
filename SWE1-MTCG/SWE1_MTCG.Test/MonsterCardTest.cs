using Bogus.Extensions;
using NUnit.Framework;
using SWE1_MTCG.Cards.Monster;

namespace SWE1_MTCG.Test
{
    public class CardTestDragon
    {

        private Dragon _dragon;

        [SetUp]
        public void Setup()
        {
            _dragon = new Dragon(25, "Ancalagon");
        }

        [Test]
        public void Test1()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_dragon);
        }

        [Test]
        public void Test2()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_dragon.getCardDamage());
            Assert.NotNull(_dragon.getCardName());
        }

        [TearDown]
        public void AfterAll()
        {

        }
    }
    public class CardTestKnight
    {
        private Knight _knight;

        [SetUp]
        public void Setup()
        {
            _knight = new Knight(25, "Arthur");
        }

        [Test]
        public void Test1()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_knight);
            Assert.NotNull(_knight.getCardDamage());
            Assert.NotNull(_knight.getCardName());
        }
    }
    public class CardTestGoblin
    {
        private Goblin _goblin;

        [SetUp]
        public void Setup()
        {
            _goblin = new Goblin(25, "Gnorsig");
        }

        [Test]
        public void Test1()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_goblin);
            Assert.NotNull(_goblin.getCardDamage());
            Assert.NotNull(_goblin.getCardName());
        }
    }
    public class CardTestKraken
    {
        private Kraken _kraken;

        [SetUp]
        public void Setup()
        {
            _kraken = new Kraken(25, "Davy Jones");
        }

        [Test]
        public void Test1()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_kraken);
            Assert.NotNull(_kraken.getCardDamage());
            Assert.NotNull(_kraken.getCardName());
        }
    }
    public class CardTestOrg
    {
        private Org _org;

        [SetUp]
        public void Setup()
        {
            _org = new Org(25, "Duke");
        }

        [Test]
        public void Test1()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_org);
            Assert.NotNull(_org.getCardDamage());
            Assert.NotNull(_org.getCardName());
        }
    }
    public class CardTestWizard
    {
        private Wizard _wizard;

        [SetUp]
        public void Setup()
        {
            _wizard = new Wizard(25, "Merlin");
        }

        [Test]
        public void Test1()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_wizard);
            Assert.NotNull(_wizard.getCardDamage());
            Assert.NotNull(_wizard.getCardName());
        }
    }
}