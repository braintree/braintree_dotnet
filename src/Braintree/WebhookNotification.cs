#pragma warning disable 1591

using System;

namespace Braintree
{
    public class WebhookKind : Enumeration
    {
        public static readonly WebhookKind CHECK = new WebhookKind("check");
        public static readonly WebhookKind PARTNER_MERCHANT_CONNECTED = new WebhookKind("partner_merchant_connected");
        public static readonly WebhookKind PARTNER_MERCHANT_DISCONNECTED = new WebhookKind("partner_merchant_disconnected");
        public static readonly WebhookKind PARTNER_MERCHANT_DECLINED = new WebhookKind("partner_merchant_declined");
        public static readonly WebhookKind OAUTH_ACCESS_REVOKED = new WebhookKind("oauth_access_revoked");
        public static readonly WebhookKind CONNECTED_MERCHANT_STATUS_TRANSITIONED = new WebhookKind("connected_merchant_status_transitioned");
        public static readonly WebhookKind CONNECTED_MERCHANT_PAYPAL_STATUS_CHANGED = new WebhookKind("connected_merchant_paypal_status_changed");
        public static readonly WebhookKind SUBSCRIPTION_CANCELED = new WebhookKind("subscription_canceled");
        public static readonly WebhookKind SUBSCRIPTION_CHARGED_SUCCESSFULLY = new WebhookKind("subscription_charged_successfully");
        public static readonly WebhookKind SUBSCRIPTION_CHARGED_UNSUCCESSFULLY = new WebhookKind("subscription_charged_unsuccessfully");
        public static readonly WebhookKind SUBSCRIPTION_EXPIRED = new WebhookKind("subscription_expired");
        public static readonly WebhookKind SUBSCRIPTION_TRIAL_ENDED = new WebhookKind("subscription_trial_ended");
        public static readonly WebhookKind SUBSCRIPTION_WENT_ACTIVE = new WebhookKind("subscription_went_active");
        public static readonly WebhookKind SUBSCRIPTION_WENT_PAST_DUE = new WebhookKind("subscription_went_past_due");
        public static readonly WebhookKind SUB_MERCHANT_ACCOUNT_APPROVED = new WebhookKind("sub_merchant_account_approved");
        public static readonly WebhookKind SUB_MERCHANT_ACCOUNT_DECLINED = new WebhookKind("sub_merchant_account_declined");
        public static readonly WebhookKind UNRECOGNIZED = new WebhookKind("unrecognized");
        public static readonly WebhookKind TRANSACTION_DISBURSED = new WebhookKind("transaction_disbursed");
        public static readonly WebhookKind TRANSACTION_SETTLED = new WebhookKind("transaction_settled");
        public static readonly WebhookKind TRANSACTION_SETTLEMENT_DECLINED = new WebhookKind("transaction_settlement_declined");
        public static readonly WebhookKind DISBURSEMENT_EXCEPTION = new WebhookKind("disbursement_exception");
        public static readonly WebhookKind DISBURSEMENT = new WebhookKind("disbursement");
        public static readonly WebhookKind DISPUTE_OPENED = new WebhookKind("dispute_opened");
        public static readonly WebhookKind DISPUTE_LOST = new WebhookKind("dispute_lost");
        public static readonly WebhookKind DISPUTE_WON = new WebhookKind("dispute_won");
        public static readonly WebhookKind ACCOUNT_UPDATER_DAILY_REPORT = new WebhookKind("account_updater_daily_report");
        public static readonly WebhookKind IDEAL_PAYMENT_COMPLETE = new WebhookKind("ideal_payment_complete");
        public static readonly WebhookKind IDEAL_PAYMENT_FAILED = new WebhookKind("ideal_payment_failed");
        public static readonly WebhookKind GRANTED_PAYMENT_INSTRUMENT_UPDATE = new WebhookKind("granted_payment_instrument_update");
        public static readonly WebhookKind LOCAL_PAYMENT_COMPLETED = new WebhookKind("local_payment_completed");

        public static readonly WebhookKind[] ALL = {
            CHECK,
            PARTNER_MERCHANT_CONNECTED,
            PARTNER_MERCHANT_DISCONNECTED,
            PARTNER_MERCHANT_DECLINED,
            OAUTH_ACCESS_REVOKED,
            CONNECTED_MERCHANT_STATUS_TRANSITIONED,
            CONNECTED_MERCHANT_PAYPAL_STATUS_CHANGED,
            SUBSCRIPTION_CANCELED,
            SUBSCRIPTION_CHARGED_SUCCESSFULLY,
            SUBSCRIPTION_CHARGED_UNSUCCESSFULLY,
            SUBSCRIPTION_EXPIRED,
            SUBSCRIPTION_TRIAL_ENDED,
            SUBSCRIPTION_WENT_ACTIVE,
            SUBSCRIPTION_WENT_PAST_DUE,
            SUB_MERCHANT_ACCOUNT_APPROVED,
            SUB_MERCHANT_ACCOUNT_DECLINED,
            TRANSACTION_DISBURSED,
            TRANSACTION_SETTLED,
            TRANSACTION_SETTLEMENT_DECLINED,
            DISBURSEMENT_EXCEPTION,
            DISBURSEMENT,
            DISPUTE_OPENED,
            DISPUTE_LOST,
            DISPUTE_WON,
            ACCOUNT_UPDATER_DAILY_REPORT,
            IDEAL_PAYMENT_COMPLETE,
            IDEAL_PAYMENT_FAILED,
            GRANTED_PAYMENT_INSTRUMENT_UPDATE,
            LOCAL_PAYMENT_COMPLETED
        };

        protected WebhookKind(string name) : base(name) {}
    }

    public class WebhookNotification
    {
        public virtual WebhookKind Kind { get; protected set; }
        public virtual Subscription Subscription { get; protected set; }
        public virtual MerchantAccount MerchantAccount { get; protected set; }
        public virtual ValidationErrors Errors { get; protected set; }
        public virtual string Message { get; protected set; }
        public virtual DateTime? Timestamp { get; protected set; }
        public virtual Transaction Transaction { get; protected set; }
        public virtual Disbursement Disbursement { get; protected set; }
        public virtual Dispute Dispute { get; protected set; }
        public virtual PartnerMerchant PartnerMerchant { get; protected set; }
        public virtual OAuthAccessRevocation OAuthAccessRevocation { get; protected set; }
        public virtual ConnectedMerchantStatusTransitioned ConnectedMerchantStatusTransitioned { get; protected set; }
        public virtual ConnectedMerchantPayPalStatusChanged ConnectedMerchantPayPalStatusChanged { get; protected set; }
        public virtual AccountUpdaterDailyReport AccountUpdaterDailyReport { get; protected set; }
        public virtual IdealPayment IdealPayment { get; protected set; }
        public virtual GrantedPaymentInstrumentUpdate GrantedPaymentInstrumentUpdate { get; protected set; }
        public virtual string SourceMerchantId { get; protected set; }
        public virtual LocalPaymentCompleted LocalPaymentCompleted { get; protected set; }


        public WebhookNotification(NodeWrapper node, IBraintreeGateway gateway)
        {
            Timestamp = node.GetDateTime("timestamp");
            Kind = (WebhookKind)CollectionUtil.Find(WebhookKind.ALL, node.GetString("kind"), WebhookKind.UNRECOGNIZED);

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

            if (WrapperNode.GetNode("ideal-payment") != null)
            {
                IdealPayment = new IdealPayment(WrapperNode.GetNode("ideal-payment"));
            }

            if (WrapperNode.GetNode("granted-payment-instrument-update") != null)
            {
                GrantedPaymentInstrumentUpdate = new GrantedPaymentInstrumentUpdate(WrapperNode.GetNode("granted-payment-instrument-update"));
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
                LocalPaymentCompleted = new LocalPaymentCompleted(WrapperNode.GetNode("local-payment"));
            }
        }

        protected internal WebhookNotification() { }
    }
}
