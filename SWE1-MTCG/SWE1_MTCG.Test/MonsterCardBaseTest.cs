using Bogus.Extensions;
using NUnit.Framework;
using SWE1_MTCG.Cards.Monster;
using MyEnum;

namespace SWE1_MTCG.Test
{
    public class CardTestDragon
    {

        private Dragon _dragon;

        [SetUp]
        public void Setup()
        {
            _dragon = new Dragon(25, "Ancalagon", elementTypes.Water);
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
            _knight = new Knight(25, "Arthur", elementTypes.Water);
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
            _goblin = new Goblin(25, "Gnorsig", elementTypes.Water);
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
            _kraken = new Kraken(25, "Davy Jones", elementTypes.Water);
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
            _org = new Org(25, "Duke", elementTypes.Water);
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
            _wizard = new Wizard(25, "Merlin", elementTypes.Water);
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

    public class CardTestElf
    {
        private Elf _elf;

        [SetUp]
        public void Setup()
        {
            _elf = new Elf(25, "Ganandorf", elementTypes.Water);
        }

        [Test]
        public void Test1()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_elf);
            Assert.NotNull(_elf.getCardDamage());
            Assert.NotNull(_elf.getCardName());
        }
    }
}