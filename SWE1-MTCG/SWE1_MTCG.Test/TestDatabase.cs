using System;
using System.Collections.Generic;
using System.Text;
using NuGet.Frameworks;
using NUnit.Framework;
using Server;


namespace SWE1_MTCG.Test
{
    class TestDatabase
    {
        private string loginRequest =
            "POST /messages HTTP/1.1\nContent-Type: text/plain; charset=utf-8\nContent-Lenght: 5\nHost: 127.0.0.1:6543\nUserName: admin_MTCG-Game-Token\nPassword: admin\n\nLogin\n";

        [Test]
        public void Test1()
        {
            //arrange
            var request = new RequestContext();
            //act
            request = MessageHandler.GetRequest(loginRequest);
            //assert
            Assert.AreEqual(request.message.Trim('\n'), "Login");
        }
    }
    class TestTokenTrue
    {
        [Test]
        public void Test()
        {
            //arrange
            string testUsername = "Robert_MTCG-Game-Token";
            //act
            bool checker = DbFunctions.CheckToken(testUsername);
            //assert
            Assert.AreEqual(checker, true);

        }
    }
    class TestTokenFalse
    {
        [Test]
        public void Test()
        {
            //arrange
            string testUsername = "Robert_IrgEinToken";
            //act
            bool checker = DbFunctions.CheckToken(testUsername);
            //assert
            Assert.AreEqual(checker, false);
        }
    }
    class TestValidateEmail
    {
        [Test]
        public void TestTrueEmail()
        {
            //arrange
            string testEmail = "if19b098@gmail.com";


            //act
            bool checker = DbFunctions.ValidEmail(testEmail);

            //assert
            Assert.AreEqual(checker, true);

        }
            [Test]
            public void TestFalseEmail()
            {
                //arrange
                string testEmail = "if19b098gmail";

                //act
                bool checker = DbFunctions.ValidEmail(testEmail);

                //assert
                Assert.AreEqual(checker, false);

            }
        }
}
