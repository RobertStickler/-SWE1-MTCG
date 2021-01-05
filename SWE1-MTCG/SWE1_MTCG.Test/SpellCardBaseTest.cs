using Bogus.Extensions;
using NUnit.Framework;
using SWE1_MTCG.Cards.Zauber;
using MyEnum;

namespace SWE1_MTCG.Test
{
    public class SpellCardTestFire
    {

        private SpellCard _fireSpell;

        [SetUp]
        public void Setup()
        {
            _fireSpell = new SpellCard("pouicyx23", 25, "Test Water Spell", elementTypes.Fire);
        }

        [Test]
        public void Test1()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_fireSpell);
        }

        [TearDown]
        public void AfterAll()
        {

        }
    }
}