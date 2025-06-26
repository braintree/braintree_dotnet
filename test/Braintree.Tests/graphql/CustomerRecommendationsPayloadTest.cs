using Braintree;
using Braintree.GraphQL;
using Braintree.TestUtil;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;


namespace Braintree.Tests.GraphQL
{
    [TestFixture]
    public class CustomerRecommendationsPayloadTest
    {
        [Test]
        public void TestCustomerRecommendationsPayload()
        {
            var payloadResponse = TestHelper.ReadJsonFile("CustomerSession/customer_recommendations_successful_response.json", true);

            var data = payloadResponse["data"] as Dictionary<string, object>;
            var payload = new CustomerRecommendationsPayload(data);


            Assert.IsTrue(payload.IsInPayPalNetwork);
            Assert.AreEqual("a-customer-session-id", payload.SessionId);
            var paymentOptions = payload.Recommendations.PaymentOptions;
            Assert.AreEqual(RecommendedPaymentOption.PAYPAL, paymentOptions[0].PaymentOption);
            Assert.AreEqual(1, paymentOptions[0].RecommendedPriority);
            Assert.AreEqual(RecommendedPaymentOption.VENMO, paymentOptions[1].PaymentOption);
            Assert.AreEqual(2, paymentOptions[1].RecommendedPriority);
        }
    }
}

