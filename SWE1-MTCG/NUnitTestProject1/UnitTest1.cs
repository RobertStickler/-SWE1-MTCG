using NUnit.Framework;
using System;
using System.IO;
using MTCG;

namespace TEST
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            CalcTestClass calc = new CalcTestClass();
            int x = 23;
            int y = 34;

            int actualValue = calc.xPlusY(x, y);
            int expectedVal = 57;

            Assert.AreEqual(expectedVal, actualValue);
        }
    }
}