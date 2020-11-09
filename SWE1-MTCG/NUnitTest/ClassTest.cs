using NUnit.Framework;
using Moq;
using SWE1_MTCG;
using System.Collections.Generic;
using System;
using System.IO;

namespace NUnitTest
{
    public class ClassTest
    {
        static readonly string data = "POST /messages HTTP/1.1\nHost: localhost: 6543\nUser - Agent: insomnia / 2020.4.2\nAccept: */*\nContent-Length: 16\n\nich bin ein Test";

        [Test]
        public void GetRequestTest()
        {
            RequestContext request = TCPClass.GetRequest(data);

            Assert.IsNotNull(request);
            Assert.AreEqual(request.message , "ich bin ein Test\n");
        }

        [Test]
        public void GetAllMessagesTest()
        {
            List<RequestContext> Liste = new List<RequestContext>();
            RequestContext request = TCPClass.GetRequest(data);
            Liste.Add(request);

            //string message = "\n0 uid: 61127db9\nmessage:\nich bin ein Test\n";            
            
            Console.WriteLine("hello \n whats up");
            var output = new StringWriter();
            Console.SetOut(output);

            TCPClass.GetAllMessages(Liste);
            
            int number = 0;
            RequestContext aPart = Liste[0];
            Assert.That(output.ToString(), Is.EqualTo(string.Format("\n{0} uid: {1} \nmessage: {2}\r\n", number, aPart.unique_id, aPart.message, Environment.NewLine)));
        }
    }    
}
