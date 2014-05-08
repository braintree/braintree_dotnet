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
        [ExpectedException(typeof(InvalidSignatureException), ExpectedMessage="no matching public key")]
        public void Parse_WithInvalidPublicKey()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            gateway.WebhookNotification.Parse("bad" + sampleNotification["signature"], sampleNotification["payload"]);
        }

        [Test]
        [ExpectedException(typeof(InvalidSignatureException), ExpectedMessage="signature does not match payload - one has been modified")]
        public void Parse_WithChangedPayload()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            gateway.WebhookNotification.Parse(sampleNotification["signature"], "bad" + sampleNotification["payload"]);
        }

        [Test]
        [ExpectedException(typeof(InvalidSignatureException), ExpectedMessage="payload contains illegal characters")]
        public void Parse_WithInvalidCharactersInPayload()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            gateway.WebhookNotification.Parse(sampleNotification["signature"], "^& bad ,* chars @!" + sampleNotification["payload"]);
        }

        [Test]
        [ExpectedException(typeof(InvalidSignatureException), ExpectedMessage="signature does not match payload - one has been modified")]
        public void Parse_AllowsAllValidChars()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            gateway.WebhookNotification.Parse(sampleNotification["signature"], "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+=/\n");
        }

        [Test]
        public void Retries()
        {
            Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            gateway.WebhookNotification.Parse(sampleNotification["signature"], sampleNotification["payload"].Trim());
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
          Assert.AreEqual(100.00, notification.Transaction.Amount);
          Assert.AreEqual("my_id", notification.Transaction.Id);
          Assert.IsTrue(notification.Transaction.DisbursementDetails.IsValid());
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForADisbursementExceptionWebhook()
        {
          Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.DISBURSEMENT_EXCEPTION, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["signature"], sampleNotification["payload"]);

          Assert.AreEqual(WebhookKind.DISBURSEMENT_EXCEPTION, notification.Kind);
          Assert.AreEqual("my_id", notification.Disbursement.Id);
          Assert.AreEqual(100.00, notification.Disbursement.Amount);
          Assert.AreEqual("bank_rejected", notification.Disbursement.ExceptionMessage);
          Assert.AreEqual(DateTime.Parse("2014-02-10"), notification.Disbursement.DisbursementDate);
          Assert.AreEqual("update_funding_information", notification.Disbursement.FollowUpAction);
          Assert.AreEqual("merchant_account_id", notification.Disbursement.MerchantAccount.Id);
          Assert.AreEqual(new string[] {"asdf", "qwer"}, notification.Disbursement.TransactionIds);
          Assert.AreEqual(false, notification.Disbursement.Success);
          Assert.AreEqual(false, notification.Disbursement.Retry);
        }

        public void SampleNotification_ReturnsANotificationForADisbursementWebhook()
        {
          Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.DISBURSEMENT, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["signature"], sampleNotification["payload"]);

          Assert.AreEqual(WebhookKind.DISBURSEMENT, notification.Kind);
          Assert.AreEqual("my_id", notification.Disbursement.Id);
          Assert.AreEqual(100.00, notification.Disbursement.Amount);
          Assert.AreEqual("bank_rejected", notification.Disbursement.ExceptionMessage);
          Assert.AreEqual(DateTime.Parse("2014-02-10"), notification.Disbursement.DisbursementDate);
          Assert.AreEqual("update_funding_information", notification.Disbursement.FollowUpAction);
          Assert.AreEqual("merchant_account_id", notification.Disbursement.MerchantAccount.Id);
          Assert.AreEqual(new string[] {"asdf", "qwer"}, notification.Disbursement.TransactionIds);
          Assert.AreEqual(true, notification.Disbursement.Success);
          Assert.AreEqual(false, notification.Disbursement.Retry);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForAPartnerMerchantConnected()
        {
          Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.PARTNER_MERCHANT_CONNECTED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["signature"], sampleNotification["payload"]);

          Assert.AreEqual(WebhookKind.PARTNER_MERCHANT_CONNECTED, notification.Kind);
          Assert.AreEqual("public_id", notification.PartnerMerchant.MerchantPublicId);
          Assert.AreEqual("public_key", notification.PartnerMerchant.PublicKey);
          Assert.AreEqual("private_key", notification.PartnerMerchant.PrivateKey);
          Assert.AreEqual("cse_key", notification.PartnerMerchant.ClientSideEncryptionKey);
          Assert.AreEqual("abc123", notification.PartnerMerchant.PartnerMerchantId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForAPartnerMerchantDisconnected()
        {
          Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.PARTNER_MERCHANT_DISCONNECTED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["signature"], sampleNotification["payload"]);

          Assert.AreEqual(WebhookKind.PARTNER_MERCHANT_DISCONNECTED, notification.Kind);
          Assert.AreEqual(null, notification.PartnerMerchant.MerchantPublicId);
          Assert.AreEqual(null, notification.PartnerMerchant.PublicKey);
          Assert.AreEqual(null, notification.PartnerMerchant.PrivateKey);
          Assert.AreEqual("abc123", notification.PartnerMerchant.PartnerMerchantId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForAPartnerMerchantDeclined()
        {
          Dictionary<String, String> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.PARTNER_MERCHANT_DECLINED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["signature"], sampleNotification["payload"]);

          Assert.AreEqual(WebhookKind.PARTNER_MERCHANT_DECLINED, notification.Kind);
          Assert.AreEqual("abc123", notification.PartnerMerchant.PartnerMerchantId);
        }
    }
}
