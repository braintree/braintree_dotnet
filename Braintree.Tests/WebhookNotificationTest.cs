using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class WebhookNotificationTest
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
            string verification = gateway.WebhookNotification.Verify("verification_token");
            Assert.AreEqual("integration_public_key|c9f15b74b0d98635cd182c51e2703cffa83388c3", verification);
        }

        [Test]
        public void SampleNotification_ReturnsAParsableNotification()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");

            WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["signature"], sampleNotification["payload"]);

            Assert.AreEqual(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, notification.Kind);
            Assert.AreEqual("my_id", notification.Subscription.Id);
            TestHelper.AreDatesEqual(DateTime.Now.ToUniversalTime(), notification.Timestamp.Value);
        }

        [Test]
        [ExpectedException(typeof(InvalidSignatureException))]
        public void Parse_WithInvalidSignature()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            gateway.WebhookNotification.Parse(sampleNotification["signature"] + "bad_stuff", sampleNotification["payload"]);
        }


        [Test]
        [ExpectedException(typeof(InvalidSignatureException))]
        public void Parse_WithInvalidPublicId()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            gateway.WebhookNotification.Parse("bad" + sampleNotification["signature"], sampleNotification["payload"]);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForAMerchantAccountApprovedWebhook()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUB_MERCHANT_ACCOUNT_APPROVED, "my_id");

            WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["signature"], sampleNotification["payload"]);

            Assert.AreEqual(WebhookKind.SUB_MERCHANT_ACCOUNT_APPROVED, notification.Kind);
            Assert.AreEqual("my_id", notification.MerchantAccount.Id);
            Assert.AreEqual(MerchantAccountStatus.ACTIVE, notification.MerchantAccount.Status);
            Assert.AreEqual("master_ma_for_my_id", notification.MerchantAccount.MasterMerchantAccount.Id);
            Assert.AreEqual(MerchantAccountStatus.ACTIVE, notification.MerchantAccount.MasterMerchantAccount.Status);
            TestHelper.AreDatesEqual(DateTime.Now.ToUniversalTime(), notification.Timestamp.Value);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForAMerchantAccountDeclinedWebhook()
        {
          Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUB_MERCHANT_ACCOUNT_DECLINED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["signature"], sampleNotification["payload"]);

          Assert.AreEqual(WebhookKind.SUB_MERCHANT_ACCOUNT_DECLINED, notification.Kind);
          Assert.AreEqual("my_id", notification.MerchantAccount.Id);
          Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_APPLICANT_DETAILS_DECLINED_OFAC, notification.Errors.ForObject("merchant-account").OnField("base")[0].Code);
          Assert.AreEqual("Applicant declined due to OFAC.", notification.Message);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForATransactionDisbursedWebhook()
        {
          Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.TRANSACTION_DISBURSED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["signature"], sampleNotification["payload"]);

          Assert.AreEqual(WebhookKind.TRANSACTION_DISBURSED, notification.Kind);
          Assert.AreEqual(100, notification.Transaction.Amount);
          Assert.AreEqual("my_id", notification.Transaction.Id);
          Assert.IsTrue(notification.Transaction.DisbursementDetails.IsValid());
        }
    }
}
