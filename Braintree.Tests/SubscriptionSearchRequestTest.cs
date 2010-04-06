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

        [Test]
        public void ToXml_DaysPastDueIs()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue().Is("30");
            var xml = "<search><daysPastDue><is>30</is></daysPastDue></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_DaysPastDueIsNot()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue().IsNot("30");
            var xml = "<search><daysPastDue><isNot>30</isNot></daysPastDue></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_DaysPastDueStartsWith()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue().StartsWith("30");
            var xml = "<search><daysPastDue><startsWith>30</startsWith></daysPastDue></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_DaysPastDueEndsWith()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue().EndsWith("30");
            var xml = "<search><daysPastDue><endsWith>30</endsWith></daysPastDue></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_DaysPastDueContains()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue().Contains("30");
            var xml = "<search><daysPastDue><contains>30</contains></daysPastDue></search>";
            Assert.AreEqual(xml, request.ToXml());
        }
    }
}
