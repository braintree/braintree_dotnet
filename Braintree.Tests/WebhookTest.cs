using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class WebhookTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
        }

        [Test]
        public void Verify_CreatesVerificationString()
        {
            string verification = gateway.Webhook.Verify("verification_token");
            Assert.AreEqual("integration_public_key|c9f15b74b0d98635cd182c51e2703cffa83388c3", verification);
        }

        [Test]
        public void SampleNotification_ReturnsAParsableNotification()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_PAST_DUE, "my_id");

            WebhookNotification notification = gateway.Webhook.Parse(sampleNotification["signature"], sampleNotification["payload"]);

            Assert.AreEqual(WebhookKind.SUBSCRIPTION_PAST_DUE, notification.Kind);
            Assert.AreEqual("my_id", notification.Subscription.Id);
        }

        [Test]
        [ExpectedException(typeof(InvalidSignatureException))]
        public void Parse_WithInvalidSignature()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_PAST_DUE, "my_id");
            gateway.Webhook.Parse(sampleNotification["signature"] + "bad_stuff", sampleNotification["payload"]);
        }


        [Test]
        [ExpectedException(typeof(InvalidSignatureException))]
        public void Parse_WithInvalidPublicId()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_PAST_DUE, "my_id");
            gateway.Webhook.Parse("bad" + sampleNotification["signature"], sampleNotification["payload"]);
        }
    }
}
