using Braintree.Exceptions;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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
            string verification = gateway.WebhookNotification.Verify("20f9f8ed05f77439fe955c977e4c8a53");
            Assert.AreEqual("integration_public_key|d9b899556c966b3f06945ec21311865d35df3ce4", verification);
        }

        [Test]
        public void Verify_ThrowsErrorOnNullSignature()
        {
            InvalidSignatureException exception = Assert.Throws<InvalidSignatureException>(() => gateway.WebhookNotification.Parse(null, "payload"));
            Assert.AreEqual(exception.Message, "signature cannot be null");
        }

        [Test]
        public void Verify_ThrowsErrorOnNullPayload()
        {
            InvalidSignatureException exception = Assert.Throws<InvalidSignatureException>(() => gateway.WebhookNotification.Parse("signature", null));
            Assert.AreEqual(exception.Message, "payload cannot be null");
        }

        [Test]
        public void Verify_ThrowsErrorOnInvalidChallenge()
        {
            InvalidChallengeException exception = Assert.Throws<InvalidChallengeException>(() => gateway.WebhookNotification.Verify("bad challenge"));
            Assert.AreEqual(exception.Message, "challenge contains non-hex characters");
        }

        [Test]
        public void SampleNotification_ReturnsAParsableNotification()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");

            WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

            Assert.AreEqual(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, notification.Kind);
            Assert.AreEqual("my_id", notification.Subscription.Id);
            TestHelper.AreDatesEqual(DateTime.Now.ToUniversalTime(), notification.Timestamp.Value);
            Assert.Null(notification.SourceMerchantId);
        }

        [Test]
        public void SampleNotification_CanIncludeSourceMerchantId()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id", "my_source_merchant_id");

            WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

            Assert.AreEqual("my_source_merchant_id", notification.SourceMerchantId);
        }

        [Test]
        public void Parse_WithInvalidPublicKey()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            InvalidSignatureException exception = Assert.Throws<InvalidSignatureException>(() => gateway.WebhookNotification.Parse("bad" + sampleNotification["bt_signature"], sampleNotification["bt_payload"]));
            Assert.AreEqual(exception.Message, "no matching public key");
        }

        [Test]
        public void Parse_WithChangedPayload()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            InvalidSignatureException exception = Assert.Throws<InvalidSignatureException>(() => gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], "bad" + sampleNotification["bt_payload"]));
            Assert.AreEqual(exception.Message, "signature does not match payload - one has been modified");
        }

        [Test]
        public void Parse_WithInvalidCharactersInPayload()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            InvalidSignatureException exception = Assert.Throws<InvalidSignatureException>(() => gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], "^& bad ,* chars @!" + sampleNotification["bt_payload"]));
            Assert.AreEqual(exception.Message, "payload contains illegal characters");
        }

        [Test]
        public void Parse_AllowsAllValidChars()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            InvalidSignatureException exception = Assert.Throws<InvalidSignatureException>(() => gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+=/\n"));
            Assert.AreEqual(exception.Message, "signature does not match payload - one has been modified");
        }

        [Test]
        public void Retries()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_WENT_PAST_DUE, "my_id");
            gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"].Trim());
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForAMerchantAccountApprovedWebhook()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUB_MERCHANT_ACCOUNT_APPROVED, "my_id");

            WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

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
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUB_MERCHANT_ACCOUNT_DECLINED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.SUB_MERCHANT_ACCOUNT_DECLINED, notification.Kind);
          Assert.AreEqual("my_id", notification.MerchantAccount.Id);
          Assert.AreEqual(ValidationErrorCode.MERCHANT_ACCOUNT_APPLICANT_DETAILS_DECLINED_OFAC, notification.Errors.ForObject("merchant-account").OnField("base")[0].Code);
          Assert.AreEqual("Applicant declined due to OFAC.", notification.Message);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForATransactionDisbursedWebhook()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.TRANSACTION_DISBURSED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.TRANSACTION_DISBURSED, notification.Kind);
          Assert.AreEqual(100.00, notification.Transaction.Amount);
          Assert.AreEqual("my_id", notification.Transaction.Id);
          Assert.IsTrue(notification.Transaction.DisbursementDetails.IsValid());
        }


        [Test]
        public void SampleNotification_ReturnsANotificationForATransactionSettledWebHook()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.TRANSACTION_SETTLED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.TRANSACTION_SETTLED, notification.Kind);
          Transaction transaction = notification.Transaction;
          Assert.AreEqual(TransactionStatus.SETTLED, transaction.Status);
          Assert.AreEqual(100.00, transaction.Amount);
          Assert.AreEqual("my_id", transaction.Id);

          UsBankAccountDetails usBankAccountDetails = transaction.UsBankAccountDetails;
          Assert.AreEqual("123456789", usBankAccountDetails.RoutingNumber);
          Assert.AreEqual("1234", usBankAccountDetails.Last4);
          Assert.AreEqual("checking", usBankAccountDetails.AccountType);
          Assert.AreEqual("Dan Schulman", usBankAccountDetails.AccountHolderName);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForATransactionSettlementDeclinedWebhook()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.TRANSACTION_SETTLEMENT_DECLINED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.TRANSACTION_SETTLEMENT_DECLINED, notification.Kind);
          Transaction transaction = notification.Transaction;
          Assert.AreEqual(TransactionStatus.SETTLEMENT_DECLINED, transaction.Status);
          Assert.AreEqual(100.00, transaction.Amount);
          Assert.AreEqual("my_id", transaction.Id);

          UsBankAccountDetails usBankAccountDetails = transaction.UsBankAccountDetails;
          Assert.AreEqual("123456789", usBankAccountDetails.RoutingNumber);
          Assert.AreEqual("1234", usBankAccountDetails.Last4);
          Assert.AreEqual("checking", usBankAccountDetails.AccountType);
          Assert.AreEqual("Dan Schulman", usBankAccountDetails.AccountHolderName);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForADisbursementExceptionWebhook()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.DISBURSEMENT_EXCEPTION, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

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
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.DISBURSEMENT, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

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
        public void SampleNotification_ReturnsANotificationForDisputeOpenedWebhook()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.DISPUTE_OPENED, "my_id");

            WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

            Assert.AreEqual(WebhookKind.DISPUTE_OPENED, notification.Kind);
            Assert.AreEqual("my_id", notification.Dispute.Id);
            Assert.AreEqual("my_id", notification.Dispute.TransactionDetails.Id);
            Assert.AreEqual("250.00", notification.Dispute.TransactionDetails.Amount);
            Assert.AreEqual(DisputeStatus.OPEN, notification.Dispute.Status);
            Assert.AreEqual(DisputeKind.CHARGEBACK, notification.Dispute.Kind);
            Assert.AreEqual(new DateTime(2014, 3, 21), notification.Dispute.DateOpened);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForDisputeLostWebhook()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.DISPUTE_LOST, "my_id");

            WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

            Assert.AreEqual(WebhookKind.DISPUTE_LOST, notification.Kind);
            Assert.AreEqual("my_id", notification.Dispute.Id);
            Assert.AreEqual(DisputeKind.CHARGEBACK, notification.Dispute.Kind);
            Assert.AreEqual(new DateTime(2014, 3, 21), notification.Dispute.DateOpened);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForDisputeWonWebhook()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.DISPUTE_WON, "my_id");

            WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

            Assert.AreEqual(WebhookKind.DISPUTE_WON, notification.Kind);
            Assert.AreEqual("my_id", notification.Dispute.Id);
            Assert.AreEqual(DisputeKind.CHARGEBACK, notification.Dispute.Kind);
            Assert.AreEqual(new DateTime(2014, 3, 21), notification.Dispute.DateOpened);
            Assert.AreEqual(new DateTime(2014, 3, 22), notification.Dispute.DateWon);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForAPartnerMerchantConnected()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.PARTNER_MERCHANT_CONNECTED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

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
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.PARTNER_MERCHANT_DISCONNECTED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.PARTNER_MERCHANT_DISCONNECTED, notification.Kind);
          Assert.AreEqual(null, notification.PartnerMerchant.MerchantPublicId);
          Assert.AreEqual(null, notification.PartnerMerchant.PublicKey);
          Assert.AreEqual(null, notification.PartnerMerchant.PrivateKey);
          Assert.AreEqual("abc123", notification.PartnerMerchant.PartnerMerchantId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForAPartnerMerchantDeclined()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.PARTNER_MERCHANT_DECLINED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.PARTNER_MERCHANT_DECLINED, notification.Kind);
          Assert.AreEqual("abc123", notification.PartnerMerchant.PartnerMerchantId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForOAuthAccessRevocation()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.OAUTH_ACCESS_REVOKED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.OAUTH_ACCESS_REVOKED, notification.Kind);
          Assert.AreEqual("my_id", notification.OAuthAccessRevocation.MerchantId);
          Assert.AreEqual("oauth_application_client_id", notification.OAuthAccessRevocation.OAuthApplicationClientId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForAConnectedMerchantStatusTransitioned()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.CONNECTED_MERCHANT_STATUS_TRANSITIONED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.CONNECTED_MERCHANT_STATUS_TRANSITIONED, notification.Kind);
          Assert.AreEqual("my_id", notification.ConnectedMerchantStatusTransitioned.MerchantPublicId);
          Assert.AreEqual("my_id", notification.ConnectedMerchantStatusTransitioned.MerchantId);
          Assert.AreEqual("new_status", notification.ConnectedMerchantStatusTransitioned.Status);
          Assert.AreEqual("oauth_application_client_id", notification.ConnectedMerchantStatusTransitioned.OAuthApplicationClientId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForAConnectedMerchantPayPalStatusChanged()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.CONNECTED_MERCHANT_PAYPAL_STATUS_CHANGED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.CONNECTED_MERCHANT_PAYPAL_STATUS_CHANGED, notification.Kind);
          Assert.AreEqual("my_id", notification.ConnectedMerchantPayPalStatusChanged.MerchantPublicId);
          Assert.AreEqual("my_id", notification.ConnectedMerchantPayPalStatusChanged.MerchantId);
          Assert.AreEqual("link", notification.ConnectedMerchantPayPalStatusChanged.Action);
          Assert.AreEqual("oauth_application_client_id", notification.ConnectedMerchantPayPalStatusChanged.OAuthApplicationClientId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForSubscriptionChargedSuccessfully()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_CHARGED_SUCCESSFULLY, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.SUBSCRIPTION_CHARGED_SUCCESSFULLY, notification.Kind);
          Assert.AreEqual("my_id", notification.Subscription.Id);
          Assert.AreEqual(1, notification.Subscription.Transactions.Count);

          Transaction transaction = notification.Subscription.Transactions[0];
          Assert.AreEqual(TransactionStatus.SUBMITTED_FOR_SETTLEMENT, transaction.Status);
          Assert.AreEqual(49.99m, transaction.Amount);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForSubscriptionChargedUnsuccessfully()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.SUBSCRIPTION_CHARGED_UNSUCCESSFULLY, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.SUBSCRIPTION_CHARGED_UNSUCCESSFULLY, notification.Kind);
          Assert.AreEqual("my_id", notification.Subscription.Id);
          Assert.AreEqual(1, notification.Subscription.Transactions.Count);

          Transaction transaction = notification.Subscription.Transactions[0];
          Assert.AreEqual(TransactionStatus.FAILED, transaction.Status);
          Assert.AreEqual(49.99m, transaction.Amount);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForAccountUpdaterDailyReport()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.ACCOUNT_UPDATER_DAILY_REPORT, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.ACCOUNT_UPDATER_DAILY_REPORT, notification.Kind);
          Assert.AreEqual("link-to-csv-report", notification.AccountUpdaterDailyReport.ReportUrl);
          Assert.AreEqual(DateTime.Parse("2016-01-14"), notification.AccountUpdaterDailyReport.ReportDate);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForIdealPaymentComplete()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.IDEAL_PAYMENT_COMPLETE, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.IDEAL_PAYMENT_COMPLETE, notification.Kind);
          IdealPayment idealPayment = notification.IdealPayment;

          Assert.AreEqual("my_id", idealPayment.Id);
          Assert.AreEqual("COMPLETE", idealPayment.Status);
          Assert.AreEqual("ORDERABC", idealPayment.OrderId);
          Assert.AreEqual(10.00m, idealPayment.Amount);
          Assert.AreEqual("https://example.com", idealPayment.ApprovalUrl);
          Assert.AreEqual("1234567890", idealPayment.IdealTransactionId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForIdealPaymentFailed()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.IDEAL_PAYMENT_FAILED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.IDEAL_PAYMENT_FAILED, notification.Kind);
          IdealPayment idealPayment = notification.IdealPayment;

          Assert.AreEqual("my_id", idealPayment.Id);
          Assert.AreEqual("FAILED", idealPayment.Status);
          Assert.AreEqual("ORDERABC", idealPayment.OrderId);
          Assert.AreEqual(10.00m, idealPayment.Amount);
          Assert.AreEqual("https://example.com", idealPayment.ApprovalUrl);
          Assert.AreEqual("1234567890", idealPayment.IdealTransactionId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForGrantedPaymentInstrumentUpdate()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.GRANTED_PAYMENT_INSTRUMENT_UPDATE, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.GRANTED_PAYMENT_INSTRUMENT_UPDATE, notification.Kind);
          GrantedPaymentInstrumentUpdate update = notification.GrantedPaymentInstrumentUpdate;

          Assert.AreEqual("vczo7jqrpwrsi2px", update.GrantOwnerMerchantId);
          Assert.AreEqual("cf0i8wgarszuy6hc", update.GrantRecipientMerchantId);
          Assert.AreEqual("ee257d98-de40-47e8-96b3-a6954ea7a9a4", update.PaymentMethodNonce);
          Assert.AreEqual("abc123z", update.Token);
          Assert.AreEqual("expiration-month", update.UpdatedFields[0]);
          Assert.AreEqual("expiration-year", update.UpdatedFields[1]);
          Assert.AreEqual(2, update.UpdatedFields.Count);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForLocalPaymentCompleted()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.LOCAL_PAYMENT_COMPLETED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.LOCAL_PAYMENT_COMPLETED, notification.Kind);
          LocalPaymentCompleted localPayment = notification.LocalPaymentCompleted;

          Assert.AreEqual("a-payment-id", localPayment.PaymentId);
          Assert.AreEqual("a-payer-id", localPayment.PayerId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForCheck()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.CHECK, "");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.CHECK, notification.Kind);
        }
    }
}
