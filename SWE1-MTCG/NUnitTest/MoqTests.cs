using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace NUnitTest
{
    public class MoqTests
    {
        public interface Iclient
        {
            Client GetClientMsg(int msg);
        }
        public class Client
        {
            public string message { get; set; }

        }

        [TestMethod]
        public void ShowHowToMockAnInterface()
        {
            Client client = new Client() { message = "Some test message"};

            var mock = new Mock<Iclient>();
            mock.Setup(service => service.GetClientMsg(1)).Returns(client);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(client, mock.Object.GetClientMsg(1));
        }
    }
}
