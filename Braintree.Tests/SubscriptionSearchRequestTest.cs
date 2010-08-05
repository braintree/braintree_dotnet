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
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().PlanId.Is("abc");
            var xml = "<search><plan-id><is>abc</is></plan-id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_DaysPastDueIs()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue.Is("30");
            var xml = "<search><days-past-due><is>30</is></days-past-due></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_DaysPastDueIsNot()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue.IsNot("30");
            var xml = "<search><days-past-due><is-not>30</is-not></days-past-due></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_DaysPastDueStartsWith()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue.StartsWith("30");
            var xml = "<search><days-past-due><starts-with>30</starts-with></days-past-due></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_DaysPastDueEndsWith()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue.EndsWith("30");
            var xml = "<search><days-past-due><ends-with>30</ends-with></days-past-due></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_DaysPastDueContains()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue.Contains("30");
            var xml = "<search><days-past-due><contains>30</contains></days-past-due></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_PriceIs()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().Price.Is(1M);
            var xml = "<search><price><is>1</is></price></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_PriceBetween()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().Price.Between(1M, 2M);
            var xml = "<search><price><min>1</min><max>2</max></price></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_PriceGreaterThanOrEqualTo()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().Price.GreaterThanOrEqualTo(12.34M);
            var xml = "<search><price><min>12.34</min></price></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_PriceLessThanOrEqualTo()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().Price.LessThanOrEqualTo(12.34M);
            var xml = "<search><price><max>12.34</max></price></search>";
            Assert.AreEqual(xml, request.ToXml());
        }
    }
}
