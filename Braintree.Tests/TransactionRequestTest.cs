using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionRequestTest
    {
        [Test]
        public void ToXml_Includes_DeviceSessionId()
        {
            TransactionRequest request = new TransactionRequest();
            request.DeviceSessionId = "my_dsid";

            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
        }
    }
}
