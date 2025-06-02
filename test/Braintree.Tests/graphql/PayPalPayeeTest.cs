using Braintree;
using Braintree.GraphQL;
using Braintree.TestUtil;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;

namespace Braintree.Tests.GraphQL
{
    [TestFixture]
    public class PayPalPayeeInputTest
    {
        [Test]
        public void TestToGraphQLVariables()
        {
            var payee = PayPalPayeeInput.Builder()
            .EmailAddress("test@example.com")
            .ClientId("merchant-public-id")
            .Build();


            var dict = payee.ToGraphQLVariables();
            
            Assert.AreEqual("test@example.com", dict["emailAddress"]);
            Assert.AreEqual("merchant-public-id", dict["clientId"]);
        }
    }
}
