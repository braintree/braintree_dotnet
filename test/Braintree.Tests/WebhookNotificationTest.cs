using Braintree.Exceptions;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
        public void SampleNotification_ReturnsANotificationForDisputeAcceptedWebhook()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.DISPUTE_ACCEPTED, "my_id");

            WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

            Assert.AreEqual(WebhookKind.DISPUTE_ACCEPTED, notification.Kind);
            Assert.AreEqual("my_id", notification.Dispute.Id);
            Assert.AreEqual("my_id", notification.Dispute.TransactionDetails.Id);
            Assert.AreEqual("250.00", notification.Dispute.TransactionDetails.Amount);
            Assert.AreEqual(DisputeStatus.ACCEPTED, notification.Dispute.Status);
            Assert.AreEqual(DisputeKind.CHARGEBACK, notification.Dispute.Kind);
            Assert.AreEqual(new DateTime(2014, 3, 21), notification.Dispute.DateOpened);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForDisputeDisputedWebhook()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.DISPUTE_DISPUTED, "my_id");

            WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

            Assert.AreEqual(WebhookKind.DISPUTE_DISPUTED, notification.Kind);
            Assert.AreEqual("my_id", notification.Dispute.Id);
            Assert.AreEqual("my_id", notification.Dispute.TransactionDetails.Id);
            Assert.AreEqual("250.00", notification.Dispute.TransactionDetails.Amount);
            Assert.AreEqual(DisputeStatus.DISPUTED, notification.Dispute.Status);
            Assert.AreEqual(DisputeKind.CHARGEBACK, notification.Dispute.Kind);
            Assert.AreEqual(new DateTime(2014, 3, 21), notification.Dispute.DateOpened);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForDisputeExpiredWebhook()
        {
            Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.DISPUTE_EXPIRED, "my_id");

            WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

            Assert.AreEqual(WebhookKind.DISPUTE_EXPIRED, notification.Kind);
            Assert.AreEqual("my_id", notification.Dispute.Id);
            Assert.AreEqual("my_id", notification.Dispute.TransactionDetails.Id);
            Assert.AreEqual("250.00", notification.Dispute.TransactionDetails.Amount);
            Assert.AreEqual(DisputeStatus.EXPIRED, notification.Dispute.Status);
            Assert.AreEqual(DisputeKind.CHARGEBACK, notification.Dispute.Kind);
            Assert.AreEqual(new DateTime(2014, 3, 21), notification.Dispute.DateOpened);
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
        public void SampleNotification_ReturnsANotificationForGrantorUpdatedGrantedPaymentMethod()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.GRANTOR_UPDATED_GRANTED_PAYMENT_METHOD, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.GRANTOR_UPDATED_GRANTED_PAYMENT_METHOD, notification.Kind);
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
        public void SampleNotification_ReturnsANotificationForRecipientUpdatedGrantedPaymentMethod()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.RECIPIENT_UPDATED_GRANTED_PAYMENT_METHOD, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.RECIPIENT_UPDATED_GRANTED_PAYMENT_METHOD, notification.Kind);
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
        public void SampleNotification_ReturnsANotificationForGrantedCreditCardRevoked()
        {
          String webhookXmlResponse = "<notification>"
                + "<source-merchant-id>12345</source-merchant-id>"
                + "<timestamp type='datetime'>2018-10-10T22:46:41Z</timestamp>"
                + "<kind>granted_payment_method_revoked</kind>"
                + "<subject>"
                + "<credit-card>"
                + "<bin>555555</bin>"
                + "<card-type>MasterCard</card-type>"
                + "<cardholder-name>Amber Ankunding</cardholder-name>"
                + "<commercial>Unknown</commercial>"
                + "<country-of-issuance>Unknown</country-of-issuance>"
                + "<created-at type='datetime'>2018-10-10T22:46:41Z</created-at>"
                + "<customer-id>credit_card_customer_id</customer-id>"
                + "<customer-location>US</customer-location>"
                + "<debit>Unknown</debit>"
                + "<default type='boolean'>true</default>"
                + "<durbin-regulated>Unknown</durbin-regulated>"
                + "<expiration-month>06</expiration-month>"
                + "<expiration-year>2020</expiration-year>"
                + "<expired type='boolean'>false</expired>"
                + "<global-id>cGF5bWVudG1ldGhvZF8zcHQ2d2hz</global-id>"
                + "<healthcare>Unknown</healthcare>"
                + "<image-url>https://assets.braintreegateway.com/payment_method_logo/mastercard.png?environment=test</image-url>"
                + "<issuing-bank>Unknown</issuing-bank>"
                + "<last-4>4444</last-4>"
                + "<payroll>Unknown</payroll>"
                + "<prepaid>Unknown</prepaid>"
                + "<product-id>Unknown</product-id>"
                + "<subscriptions type='array'/>"
                + "<token>credit_card_token</token>"
                + "<unique-number-identifier>08199d188e37460163207f714faf074a</unique-number-identifier>"
                + "<updated-at type='datetime'>2018-10-10T22:46:41Z</updated-at>"
                + "<venmo-sdk type='boolean'>false</venmo-sdk>"
                + "<verifications type='array'/>"
                + "</credit-card>"
                + "</subject>"
                + "</notification>";
          String encodedPayload = Convert.ToBase64String(Encoding.GetEncoding(0).GetBytes(webhookXmlResponse)) + '\n';

          Dictionary<string, string> sampleNotification = TestHelper.SampleNotificationFromXml(gateway, encodedPayload);

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.GRANTED_PAYMENT_METHOD_REVOKED, notification.Kind);
          RevokedPaymentMethodMetadata metadata = notification.RevokedPaymentMethodMetadata;

          Assert.AreEqual("credit_card_customer_id", metadata.CustomerId);
          Assert.AreEqual("credit_card_token", metadata.Token);
          Assert.IsTrue(metadata.RevokedPaymentMethod is CreditCard);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForGrantedPayPalAccountRevoked()
        {
          String webhookXmlResponse = "<notification>"
			  + "<source-merchant-id>12345</source-merchant-id>"
			  + "<timestamp type='datetime'>2018-10-10T22:46:41Z</timestamp>"
			  + "<kind>granted_payment_method_revoked</kind>"
			  + "<subject>"
			  + "<paypal-account>"
			  + "<billing-agreement-id>billing_agreement_id</billing-agreement-id>"
			  + "<created-at type='dateTime'>2018-10-11T21:10:33Z</created-at>"
			  + "<customer-id>paypal_customer_id</customer-id>"
			  + "<default type='boolean'>true</default>"
			  + "<email>johndoe@example.com</email>"
			  + "<global-id>cGF5bWVudG1ldGhvZF9wYXlwYWxfdG9rZW4</global-id>"
			  + "<image-url>https://assets.braintreegateway.com/payment_method_logo/mastercard.png?environment=test</image-url>"
			  + "<subscriptions type='array'></subscriptions>"
			  + "<token>paypal_token</token>"
			  + "<updated-at type='dateTime'>2018-10-11T21:10:33Z</updated-at>"
			  + "<payer-id>a6a8e1a4</payer-id>"
			  + "</paypal-account>"
			  + "</subject>"
			  + "</notification>";
          String encodedPayload = Convert.ToBase64String(Encoding.GetEncoding(0).GetBytes(webhookXmlResponse)) + '\n';

          Dictionary<string, string> sampleNotification = TestHelper.SampleNotificationFromXml(gateway, encodedPayload);

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.GRANTED_PAYMENT_METHOD_REVOKED, notification.Kind);
          RevokedPaymentMethodMetadata metadata = notification.RevokedPaymentMethodMetadata;

          Assert.AreEqual("paypal_customer_id", metadata.CustomerId);
          Assert.AreEqual("paypal_token", metadata.Token);
          Assert.IsTrue(metadata.RevokedPaymentMethod is PayPalAccount);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForGrantedVenmoAccountRevoked()
        {
          String webhookXmlResponse = "<notification>"
			  + "<source-merchant-id>12345</source-merchant-id>"
			  + "<timestamp type='datetime'>2018-10-10T22:46:41Z</timestamp>"
			  + "<kind>granted_payment_method_revoked</kind>"
			  + "<subject>"
			  + "<venmo-account>"
			  + "<created-at type='dateTime'>2018-10-11T21:28:37Z</created-at>"
			  + "<updated-at type='dateTime'>2018-10-11T21:28:37Z</updated-at>"
			  + "<default type='boolean'>true</default>"
			  + "<image-url>https://assets.braintreegateway.com/payment_method_logo/mastercard.png?environment=test</image-url>"
			  + "<token>venmo_token</token>"
			  + "<source-description>Venmo Account: venmojoe</source-description>"
			  + "<username>venmojoe</username>"
			  + "<venmo-user-id>456</venmo-user-id>"
			  + "<subscriptions type='array'/>"
			  + "<customer-id>venmo_customer_id</customer-id>"
			  + "<global-id>cGF5bWVudG1ldGhvZF92ZW5tb2FjY291bnQ</global-id>"
			  + "</venmo-account>"
			  + "</subject>"
			  + "</notification>";
          String encodedPayload = Convert.ToBase64String(Encoding.GetEncoding(0).GetBytes(webhookXmlResponse)) + '\n';

          Dictionary<string, string> sampleNotification = TestHelper.SampleNotificationFromXml(gateway, encodedPayload);

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.GRANTED_PAYMENT_METHOD_REVOKED, notification.Kind);
          RevokedPaymentMethodMetadata metadata = notification.RevokedPaymentMethodMetadata;

          Assert.AreEqual("venmo_customer_id", metadata.CustomerId);
          Assert.AreEqual("venmo_token", metadata.Token);
          Assert.IsTrue(metadata.RevokedPaymentMethod is VenmoAccount);
        }

        [Test]
        public void WebhookTesting_SampleNotification_ReturnsANotificationForGrantedVenmoAccountRevoked()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.GRANTED_PAYMENT_METHOD_REVOKED, "granted_payment_method_revoked_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.GRANTED_PAYMENT_METHOD_REVOKED, notification.Kind);
          RevokedPaymentMethodMetadata metadata = notification.RevokedPaymentMethodMetadata;

          Assert.AreEqual("venmo_customer_id", metadata.CustomerId);
          Assert.AreEqual("granted_payment_method_revoked_id", metadata.Token);
          Assert.IsTrue(metadata.RevokedPaymentMethod is VenmoAccount);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForPaymentMethodRevokedByCustomer()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.PAYMENT_METHOD_REVOKED_BY_CUSTOMER, "my_payment_method_token");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.PAYMENT_METHOD_REVOKED_BY_CUSTOMER, notification.Kind);
          RevokedPaymentMethodMetadata metadata = notification.RevokedPaymentMethodMetadata;

          Assert.AreEqual("my_payment_method_token", metadata.Token);
          Assert.IsTrue(metadata.RevokedPaymentMethod is PayPalAccount);
          PayPalAccount paypalAccount = (PayPalAccount) metadata.RevokedPaymentMethod;
          Assert.IsNotNull(paypalAccount.RevokedAt);
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
          Assert.AreEqual("ee257d98-de40-47e8-96b3-a6954ea7a9a4", localPayment.PaymentMethodNonce);
          Assert.NotNull(localPayment.Transaction);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForLocalPaymentExpired()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.LOCAL_PAYMENT_EXPIRED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.LOCAL_PAYMENT_EXPIRED, notification.Kind);
          LocalPaymentExpired localPayment = notification.LocalPaymentExpired;

          Assert.AreEqual("a-payment-id", localPayment.PaymentId);
          Assert.AreEqual("a-payment-context-id", localPayment.PaymentContextId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForLocalPaymentFunded()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.LOCAL_PAYMENT_FUNDED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.LOCAL_PAYMENT_FUNDED, notification.Kind);

          LocalPaymentFunded localPayment = notification.LocalPaymentFunded;
          Assert.AreEqual("a-payment-id", localPayment.PaymentId);
          Assert.AreEqual("a-payment-context-id", localPayment.PaymentContextId);

          Transaction transaction = localPayment.Transaction;
          Assert.NotNull(transaction);
          Assert.AreEqual("1", transaction.Id);
          Assert.AreEqual(TransactionStatus.SETTLED, transaction.Status);
          Assert.AreEqual("order1234", transaction.OrderId);
        }

        [Test]
        public void SampleNotification_ReturnsANotificationForLocalPaymentReversed()
        {
          Dictionary<string, string> sampleNotification = gateway.WebhookTesting.SampleNotification(WebhookKind.LOCAL_PAYMENT_REVERSED, "my_id");

          WebhookNotification notification = gateway.WebhookNotification.Parse(sampleNotification["bt_signature"], sampleNotification["bt_payload"]);

          Assert.AreEqual(WebhookKind.LOCAL_PAYMENT_REVERSED, notification.Kind);
          LocalPaymentReversed localPayment = notification.LocalPaymentReversed;

          Assert.AreEqual("a-payment-id", localPayment.PaymentId);
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
