using MyEnum;
using NUnit.Framework;
using SWE1_MTCG.Cards.Monster;


namespace SWE1_MTCG.Test
{
    public class CardTestDragon
    {
        private MonsterCard _firedragon;

        [SetUp]
        public void Setup()
        {
            _firedragon = new MonsterCard("sadf34s", 40, "Der Test Drache", elementTypes.Fire, cardProperty.Dragon);
        }

        [Test]
        public void Test1()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_firedragon);
        }

        [Test]
        public void Test2()
        {
            //arrange
            //act
            //assert
            Assert.NotNull(_firedragon.getCardDamage());
            Assert.NotNull(_firedragon.getCardName());
        }

        [TearDown]
        public void AfterAll()
        {

        }
    }

    public class CardTestKnight
    {
        private MonsterCard _knight;

        [SetUp]
        public void Setup()
        {
            _knight = new MonsterCard("sadf34s", 40, "Der Test Krieger", elementTypes.Fire, cardProperty.Knight);
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
}