using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using SWE1_MTCG;

namespace NUnitTest
{
    public class SimpleTest
    {
        [Test]
        public void RequestTest()
        {
            //arrange
            RequestContext request = new RequestContext();
            //act         
            //assert
            Assert.NotNull(request);
        }
        [Test]
        public void TCPClassTest()
        {
            //arrange
            TCPClass tcpClass = new TCPClass();
            //act         
            //assert
            Assert.NotNull(tcpClass);
        }
        [Test]
        public void RequestContextTest()
        {
            var A = new RequestContext();
            var B = new ResponseContext();

            Assert.AreEqual("0", A.unique_id);
            Assert.NotNull(B);
        }
    }
        
}