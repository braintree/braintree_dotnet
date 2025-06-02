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
    public class MonetaryAmountInputTest
    {
        [Test]
        public void TestToGraphQLVariables()
        {

            var amount = new MonetaryAmountInput("300.0", "USD");

            var dict = amount.ToGraphQLVariables();
            
            Assert.AreEqual("300.0", dict["value"].ToString());
            Assert.AreEqual("USD", dict["currencyCode"]);
        }
    }
}
