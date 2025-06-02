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
    public class CustomerRecommendationsTest
    {
        [Test]
        public void Constructor_WithPaymentRecommendations_SetsPropertiesCorrectly()
        {
            var paymentRecommendations = new List<PaymentRecommendation>
            {
                new PaymentRecommendation(RecommendedPaymentOption.PAYPAL, 1),
                new PaymentRecommendation(RecommendedPaymentOption.VENMO, 2),
            };

            var customerRecommendations = new CustomerRecommendations(paymentRecommendations);

            Assert.AreEqual(paymentRecommendations, customerRecommendations.PaymentRecommendations);
            Assert.AreEqual(2, customerRecommendations.PaymentOptions.Count);

            Assert.AreEqual(1, customerRecommendations.PaymentOptions[0].RecommendedPriority);
            Assert.AreEqual(RecommendedPaymentOption.PAYPAL, customerRecommendations.PaymentOptions[0].PaymentOption);

            Assert.AreEqual(2, customerRecommendations.PaymentOptions[1].RecommendedPriority);
            Assert.AreEqual(RecommendedPaymentOption.VENMO, customerRecommendations.PaymentOptions[1].PaymentOption);
        }

        [Test]
        public void DefaultConstructor_InitializesEmptyLists()
        {
            var customerRecommendations = new CustomerRecommendations();

            Assert.IsNotNull(customerRecommendations.PaymentRecommendations);
            Assert.IsNotNull(customerRecommendations.PaymentOptions);
            Assert.AreEqual(0, customerRecommendations.PaymentRecommendations.Count);
            Assert.AreEqual(0, customerRecommendations.PaymentOptions.Count);
        }
    }
}