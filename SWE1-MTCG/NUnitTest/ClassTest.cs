using NUnit.Framework;
using Moq;
using SWE1_MTCG;

namespace NUnitTest
{
    public class ClassTest
    {
        TCPClass tcpClass = new TCPClass();
        
        public class Client
        {
            public string message { get; set; }

        }

        [Test]
        public void GetRequestTest()
        {
            string data = "ich bin ein Test";
            RequestContext request = TCPClass.GetRequest(data);

            Assert.IsNotNull(request);
            Assert.AreEqual(request.message , "ich bin ein Test");
        }
    }
    
}
