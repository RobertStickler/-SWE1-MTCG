using Bogus.Extensions;
using NUnit.Framework;
using SWE1_MTCG.Cards.Zauber;

namespace SWE1_MTCG.Test
{
    public class SpellCardTestFire
    {

        private FireSpell _fireSpell;

        [SetUp]
        public void Setup()
        {
            _fireSpell = new FireSpell();
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

    public class SpellCardTestWater
    {

        private WaterSpell _waterSpell;

        [SetUp]
        public void Setup()
        {
            _waterSpell = new WaterSpell();
        }

        [Test]
        public void Test1()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_waterSpell);
        }

        [TearDown]
        public void AfterAll()
        {

        }
    }

    public class SpellCardTestNormal
    {

        private NormalSpell _normalSpell;

        [SetUp]
        public void Setup()
        {
            _normalSpell = new NormalSpell();
        }

        [Test]
        public void Test1()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_normalSpell);
        }

        [TearDown]
        public void AfterAll()
        {

        }
    }

}