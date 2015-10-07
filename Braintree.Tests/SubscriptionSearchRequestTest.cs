using System;
using NUnit.Framework;

namespace Braintree.Tests
{
    //NOTE: good
    [TestFixture]
    public class SubscriptionSearchRequestTest
    {
        [Test]
        public void ToXml_BillingCyclesRemainingIs()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().BillingCyclesRemaining.Is(1);
            var xml = "<search><billing-cycles-remaining><is>1</is></billing-cycles-remaining></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_BillingCyclesRemainingBetween()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().BillingCyclesRemaining.Between(1, 2);
            var xml = "<search><billing-cycles-remaining><min>1</min><max>2</max></billing-cycles-remaining></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_BillingCyclesRemainingGreaterThanOrEqualTo()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().BillingCyclesRemaining.GreaterThanOrEqualTo(12.34);
            var xml = "<search><billing-cycles-remaining><min>12.34</min></billing-cycles-remaining></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_BillingCyclesRemainingLessThanOrEqualTo()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().BillingCyclesRemaining.LessThanOrEqualTo(12.34);
            var xml = "<search><billing-cycles-remaining><max>12.34</max></billing-cycles-remaining></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_IdIs()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().Id.Is("30");
            var xml = "<search><id><is>30</is></id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_IdIsNot()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().Id.IsNot("30");
            var xml = "<search><id><is-not>30</is-not></id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_IdStartsWith()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().Id.StartsWith("30");
            var xml = "<search><id><starts-with>30</starts-with></id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_IdEndsWith()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().Id.EndsWith("30");
            var xml = "<search><id><ends-with>30</ends-with></id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_IdContains()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().Id.Contains("30");
            var xml = "<search><id><contains>30</contains></id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_MerchantAccountIdIncludedIn()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().MerchantAccountId.IncludedIn("abc", "def");
            var xml = "<search><merchant-account-id type=\"array\"><item>abc</item><item>def</item></merchant-account-id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_MerchantAccountIdIncludedInWithExplicitArray()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().MerchantAccountId.IncludedIn(new string[] {"abc", "def"});
            var xml = "<search><merchant-account-id type=\"array\"><item>abc</item><item>def</item></merchant-account-id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_MerchantAccountIdIs()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().MerchantAccountId.Is("abc");
            var xml = "<search><merchant-account-id type=\"array\"><item>abc</item></merchant-account-id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_PlanIdIs()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().PlanId.Is("abc");
            var xml = "<search><plan-id><is>abc</is></plan-id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_PlanIdIsNot()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().PlanId.IsNot("30");
            var xml = "<search><plan-id><is-not>30</is-not></plan-id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_PlanIdStartsWith()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().PlanId.StartsWith("30");
            var xml = "<search><plan-id><starts-with>30</starts-with></plan-id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_PlanIdEndsWith()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().PlanId.EndsWith("30");
            var xml = "<search><plan-id><ends-with>30</ends-with></plan-id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_PlanIdContains()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().PlanId.Contains("30");
            var xml = "<search><plan-id><contains>30</contains></plan-id></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_PlanIdIncludedIn()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().PlanId.IncludedIn("abc", "def");
            var xml = "<search><plan-id type=\"array\"><item>abc</item><item>def</item></plan-id></search>";
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
        public void ToXml_DaysPastDueBetween()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue.Between(2, 3);
            var xml = "<search><days-past-due><min>2</min><max>3</max></days-past-due></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_DaysPastDueGreaterThanOrEqualTo()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue.GreaterThanOrEqualTo(3);
            var xml = "<search><days-past-due><min>3</min></days-past-due></search>";
            Assert.AreEqual(xml, request.ToXml());
        }

        [Test]
        public void ToXml_DaysPastDueLessThanOrEqualTo()
        {
            SubscriptionSearchRequest request = new SubscriptionSearchRequest().DaysPastDue.LessThanOrEqualTo(4);
            var xml = "<search><days-past-due><max>4</max></days-past-due></search>";
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
