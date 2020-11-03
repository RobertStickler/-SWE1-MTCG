using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using static NUnitTest.MoqSetup;

namespace NUnitTest
{
    public class MoqTest
    {
        private readonly Iclient _iclient;

        public MoqTest(Iclient clientService)
        {
            _iclient = clientService;
        }

        public Client GetPerson()
        {
            return _iclient.GetClientMsg(1);
        }

        [TestMethod]
        public void ShowUsageOfMock()
        {
            Client client = new Client() { message = "Some test message" };

            var mock = new Mock<Iclient>();
            mock.Setup(service => service.GetClientMsg(1)).Returns(client);

            MoqTest moqTest = new MoqTest(mock.Object);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(client, moqTest.GetPerson());
        }
    }
}
