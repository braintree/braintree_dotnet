using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionCreditCardRequestTest
    {
        [Test]
        [Category("Unit")]
        public void ToXml_Includes_Token()
        {
            TransactionCreditCardRequest request = new TransactionCreditCardRequest();
            request.Token = "my-token";
            Assert.IsTrue(request.ToXml().Contains("my-token"));
        }
    }
}
