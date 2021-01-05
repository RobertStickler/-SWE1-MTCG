using Bogus.Extensions;
using NUnit.Framework;
using SWE1_MTCG.Cards.Zauber;
using MyEnum;
using Cards;
using SWE1_MTCG;


namespace SWE1_MTCG.Test
{
    class GetRequestTest
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
            Assert.AreEqual(request.message.Trim('\n') , "Login");
        }
    }
}
