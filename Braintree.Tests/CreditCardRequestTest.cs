using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class CreditCardRequestTest
    {
        [Test]
        public void ToXml_Includes_DeviceSessionId()
        {
            CreditCardRequest request = new CreditCardRequest();
            request.DeviceSessionId = "my_dsid";

            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
        }
    }
}
