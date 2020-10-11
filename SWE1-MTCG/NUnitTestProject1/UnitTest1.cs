using NUnit.Framework;
using System;
using System.IO;
using MTCG;

namespace TEST
{
    [TestFixture]
    public class Tests
    {

        private CalcTestClass _calc;

        [SetUp]
        public void Setup()
        {
            _calc = new CalcTestClass();
            //_calc.Bias = 10; // für alle test, wenn sich diese Ändern muss eine neue Methode (Before) erstellt werden
        }



        [Test]
        public void TestSum()
        {
            //arrange
            int x = 23;
            int y = 34;
            _calc.Bias = 10;

            //act
            int actualValue = _calc.xPlusY(x, y) + _calc.Bias;
            int expectedVal = 57 + _calc.Bias;

            //assert
            Assert.AreEqual(expectedVal, actualValue);
        }

        [Test]
        public void TestIsGreaterZero()
        {
            //arrange
            int value1 = 3;

            //act
            bool isGreater = _calc.GreaterThanZero(value1);

            //assert
            Assert.IsTrue(isGreater);
            Assert.IsFalse(_calc.GreaterThanZero(value1 * (-1)));
        }

        [TearDown]
        public void AfterAll()
        {

        }
    }
}