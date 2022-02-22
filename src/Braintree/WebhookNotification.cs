#pragma warning disable 1591

using System;
using System.ComponentModel;

namespace Braintree
{
    public enum WebhookKind
    {
        [Description("account_updater_daily_report")] ACCOUNT_UPDATER_DAILY_REPORT,
        [Description("check")] CHECK,
        [Description("connected_merchant_paypal_status_changed")] CONNECTED_MERCHANT_PAYPAL_STATUS_CHANGED,
        [Description("connected_merchant_status_transitioned")] CONNECTED_MERCHANT_STATUS_TRANSITIONED,
        [Description("disbursement")] DISBURSEMENT,
        [Description("disbursement_exception")] DISBURSEMENT_EXCEPTION,
        [Description("dispute_accepted")] DISPUTE_ACCEPTED,
        [Description("dispute_disputed")] DISPUTE_DISPUTED,
        [Description("dispute_expired")] DISPUTE_EXPIRED,
        [Description("dispute_lost")] DISPUTE_LOST,
        [Description("dispute_opened")] DISPUTE_OPENED,
        [Description("dispute_won")] DISPUTE_WON,
        [Description("grantor_updated_granted_payment_method")] GRANTOR_UPDATED_GRANTED_PAYMENT_METHOD,
        [Description("granted_payment_method_revoked")] GRANTED_PAYMENT_METHOD_REVOKED,
        [Description("local_payment_completed")] LOCAL_PAYMENT_COMPLETED,
        [Description("local_payment_expired")] LOCAL_PAYMENT_EXPIRED,
        [Description("local_payment_funded")] LOCAL_PAYMENT_FUNDED,
        [Description("local_payment_reversed")] LOCAL_PAYMENT_REVERSED,
        [Description("oauth_access_revoked")] OAUTH_ACCESS_REVOKED,
        [Description("partner_merchant_connected")] PARTNER_MERCHANT_CONNECTED,
        [Description("partner_merchant_declined")] PARTNER_MERCHANT_DECLINED,
        [Description("partner_merchant_disconnected")] PARTNER_MERCHANT_DISCONNECTED,
        [Description("payment_method_customer_data_updated")] PAYMENT_METHOD_CUSTOMER_DATA_UPDATED,
        [Description("payment_method_revoked_by_customer")] PAYMENT_METHOD_REVOKED_BY_CUSTOMER,
        [Description("recipient_updated_granted_payment_method")] RECIPIENT_UPDATED_GRANTED_PAYMENT_METHOD,
        [Description("subscription_canceled")] SUBSCRIPTION_CANCELED,
        [Description("subscription_expired")] SUBSCRIPTION_EXPIRED,
        [Description("subscription_charged_successfully")] SUBSCRIPTION_CHARGED_SUCCESSFULLY,
        [Description("subscription_charged_unsuccessfully")] SUBSCRIPTION_CHARGED_UNSUCCESSFULLY,
        [Description("subscription_trial_ended")] SUBSCRIPTION_TRIAL_ENDED,
        [Description("subscription_went_active")] SUBSCRIPTION_WENT_ACTIVE,
        [Description("subscription_went_past_due")] SUBSCRIPTION_WENT_PAST_DUE,
        [Description("sub_merchant_account_approved")] SUB_MERCHANT_ACCOUNT_APPROVED,
        [Description("sub_merchant_account_declined")] SUB_MERCHANT_ACCOUNT_DECLINED,
        [Description("transaction_disbursed")] TRANSACTION_DISBURSED,
        [Description("transaction_reviewed")] TRANSACTION_REVIEWED,
        [Description("transaction_settled")] TRANSACTION_SETTLED,
        [Description("transaction_settlement_declined")] TRANSACTION_SETTLEMENT_DECLINED,
        [Description("unrecognized")] UNRECOGNIZED
    }

    public class WebhookNotification
    {
        public virtual AccountUpdaterDailyReport AccountUpdaterDailyReport { get; protected set; }
        public virtual ConnectedMerchantPayPalStatusChanged ConnectedMerchantPayPalStatusChanged { get; protected set; }
        public virtual ConnectedMerchantStatusTransitioned ConnectedMerchantStatusTransitioned { get; protected set; }
        public virtual Disbursement Disbursement { get; protected set; }
        public virtual Dispute Dispute { get; protected set; }
        public virtual ValidationErrors Errors { get; protected set; }
        public virtual GrantedPaymentInstrumentUpdate GrantedPaymentInstrumentUpdate { get; protected set; }
        public virtual WebhookKind Kind { get; protected set; }
        public virtual MerchantAccount MerchantAccount { get; protected set; }
        public virtual LocalPaymentCompleted LocalPaymentCompleted { get; protected set; }
        public virtual LocalPaymentExpired LocalPaymentExpired { get; protected set; }
        public virtual LocalPaymentFunded LocalPaymentFunded { get; protected set; }
        public virtual LocalPaymentReversed LocalPaymentReversed { get; protected set; }
        public virtual PartnerMerchant PartnerMerchant { get; protected set; }
        public virtual string Message { get; protected set; }
        public virtual RevokedPaymentMethodMetadata RevokedPaymentMethodMetadata { get; protected set; }
        public virtual string SourceMerchantId { get; protected set; }
        public virtual Subscription Subscription { get; protected set; }
        public virtual DateTime? Timestamp { get; protected set; }
        public virtual Transaction Transaction { get; protected set; }
        public virtual TransactionReview TransactionReview { get; protected set; }
        public virtual OAuthAccessRevocation OAuthAccessRevocation { get; protected set; }
        public virtual PaymentMethodCustomerDataUpdatedMetadata PaymentMethodCustomerDataUpdatedMetadata { get; protected set; }
        
        public WebhookNotification(NodeWrapper node, IBraintreeGateway gateway)
        {
            Timestamp = node.GetDateTime("timestamp");
            Kind = node.GetEnum("kind", WebhookKind.UNRECOGNIZED);

            NodeWrapper WrapperNode = node.GetNode("subject");

            if (node.GetString("source-merchant-id") != null)
            {
                SourceMerchantId = node.GetString("source-merchant-id");
            }

            if (WrapperNode.GetNode("api-error-response") != null)
            {
                WrapperNode = WrapperNode.GetNode("api-error-response");
            }

            if (WrapperNode.GetNode("subscription") != null)
            {
                Subscription = new Subscription(WrapperNode.GetNode("subscription"), gateway);
            }

            if (WrapperNode.GetNode("merchant-account") != null)
            {
                MerchantAccount = new MerchantAccount(WrapperNode.GetNode("merchant-account"));
            }

            if (WrapperNode.GetNode("dispute") != null)
            {
                Dispute = new Dispute(WrapperNode.GetNode("dispute"));
            }

            if (WrapperNode.GetNode("transaction") != null)
            {
                Transaction = new Transaction(WrapperNode.GetNode("transaction"), gateway);
            }

            if (WrapperNode.GetNode("transaction-review") != null)
            {
                TransactionReview = new TransactionReview(WrapperNode.GetNode("transaction-review"));
            }

            if (WrapperNode.GetNode("disbursement") != null)
            {
                Disbursement = new Disbursement(WrapperNode.GetNode("disbursement"), gateway);
            }

            if (WrapperNode.GetNode("partner-merchant") != null)
            {
                PartnerMerchant = new PartnerMerchant(WrapperNode.GetNode("partner-merchant"));
            }

            if (WrapperNode.GetNode("oauth-application-revocation") != null)
            {
                OAuthAccessRevocation = new OAuthAccessRevocation(WrapperNode.GetNode("oauth-application-revocation"));
            }

            if (WrapperNode.GetNode("connected-merchant-status-transitioned") != null)
            {
                ConnectedMerchantStatusTransitioned = new ConnectedMerchantStatusTransitioned(WrapperNode.GetNode("connected-merchant-status-transitioned"));
            }

            if (WrapperNode.GetNode("connected-merchant-paypal-status-changed") != null)
            {
                ConnectedMerchantPayPalStatusChanged = new ConnectedMerchantPayPalStatusChanged(WrapperNode.GetNode("connected-merchant-paypal-status-changed"));
            }

            if (WrapperNode.GetNode("account-updater-daily-report") != null)
            {
                AccountUpdaterDailyReport = new AccountUpdaterDailyReport(WrapperNode.GetNode("account-updater-daily-report"));
            }

            if (WrapperNode.GetNode("granted-payment-instrument-update") != null)
            {
                GrantedPaymentInstrumentUpdate = new GrantedPaymentInstrumentUpdate(WrapperNode.GetNode("granted-payment-instrument-update"));
            }

            if (Kind == WebhookKind.GRANTED_PAYMENT_METHOD_REVOKED || Kind == WebhookKind.PAYMENT_METHOD_REVOKED_BY_CUSTOMER)
            {
                RevokedPaymentMethodMetadata = new RevokedPaymentMethodMetadata(WrapperNode, gateway);
            }

            if (WrapperNode.GetNode("errors") != null)
            {
                Errors = new ValidationErrors(WrapperNode.GetNode("errors"));
            }

            if (WrapperNode.GetNode("message") != null)
            {
                Message = WrapperNode.GetString("message");
            }

            if (WrapperNode.GetNode("local-payment") != null)
            {
                LocalPaymentCompleted = new LocalPaymentCompleted(WrapperNode.GetNode("local-payment"), gateway);
            } 

            if (WrapperNode.GetNode("local-payment-expired") != null)
            {
                LocalPaymentExpired = new LocalPaymentExpired(WrapperNode.GetNode("local-payment-expired"), gateway);
            }

            if (WrapperNode.GetNode("local-payment-funded") != null)
            {
                LocalPaymentFunded = new LocalPaymentFunded(WrapperNode.GetNode("local-payment-funded"), gateway);
            }

            if (WrapperNode.GetNode("local-payment-reversed") != null)
            {
                LocalPaymentReversed = new LocalPaymentReversed(WrapperNode.GetNode("local-payment-reversed"), gateway); 
            }

            if (WrapperNode.GetNode("payment-method-customer-data-updated-metadata") != null)
            {
                PaymentMethodCustomerDataUpdatedMetadata = new PaymentMethodCustomerDataUpdatedMetadata(WrapperNode.GetNode("payment-method-customer-data-updated-metadata"), gateway);
            }
        }
        
        protected internal WebhookNotification() { }
    }
}
