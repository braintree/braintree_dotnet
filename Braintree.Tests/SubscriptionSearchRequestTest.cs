using System;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class SubscriptionSearchRequestTest
    {
        [Test]
        public void ToXml_PlanIdIs()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().PlanId().Is("abc");
            var xml = "<search><planId><is>abc</is></planId></search>";
            Assert.AreEqual(xml, request.ToXml());
        }
    }
}
