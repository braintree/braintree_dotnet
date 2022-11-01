using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Braintree
{
    public enum DisputeStatus
    {
        [Description("open")] OPEN,
        [Description("lost")] LOST,
        [Description("won")] WON,
        [Description("unrecognized")] UNRECOGNIZED,
        [Description("accepted")] ACCEPTED,
        [Description("disputed")] DISPUTED,
        [Description("expired")] EXPIRED
    }

    public enum DisputeKind
    {
        [Description("chargeback")] CHARGEBACK,
        [Description("pre_arbitration")] PRE_ARBITRATION,
        [Description("retrieval")] RETRIEVAL,
        [Description("unrecognized")] UNRECOGNIZED
    }

    public enum DisputeReason
    {
        [Description("cancelled_recurring_transaction")] CANCELLED_RECURRING_TRANSACTION,
        [Description("credit_not_processed")] CREDIT_NOT_PROCESSED,
        [Description("duplicate")] DUPLICATE,
        [Description("fraud")] FRAUD,
        [Description("general")] GENERAL,
        [Description("invalid_account")] INVALID_ACCOUNT,
        [Description("not_recognized")] NOT_RECOGNIZED,
        [Description("product_not_received")] PRODUCT_NOT_RECEIVED,
        [Description("product_unsatisfactory")] PRODUCT_UNSATISFACTORY,
        [Description("transaction_amount_differs")] TRANSACTION_AMOUNT_DIFFERS,
        [Description("retrieval")] RETRIEVAL
    }

    // NEXT_MAJOR_VERSION Remove this enum
    public enum DisputeChargebackProtectionLevel
    {
        [Description("effortless")] EFFORTLESS,
        [Description("standard")] STANDARD,
        [Description("not_protected")] NOT_PROTECTED
    }

    public enum DisputeProtectionLevel
    {
        [Description("Effortless Chargeback Protection tool")] EFFORTLESS_CBP,
        [Description("Chargeback Protection tool")] STANDARD_CBP,
        [Description("No Protection")] NO_PROTECTION
    }

    public class Dispute
    {
        public virtual decimal? Amount { get; protected set; }
        public virtual decimal? AmountDisputed { get; protected set; }
        public virtual decimal? AmountWon { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? DateOpened { get; protected set; }
        public virtual DateTime? DateWon { get; protected set; }
        public virtual DateTime? ReceivedDate { get; protected set; }
        public virtual DateTime? ReplyByDate { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual DisputeReason Reason { get; protected set; }
        public virtual DisputeStatus Status { get; protected set; }
        public virtual DisputeKind Kind { get; protected set; }
        // NEXT_MAJOR_VERSION Remove this attribute
        [ObsoleteAttribute("use ProtectionLevel instead", false)]
        public virtual DisputeChargebackProtectionLevel ChargebackProtectionLevel { get; protected set; }
        public virtual DisputeProtectionLevel ProtectionLevel { get; protected set; }
        public virtual string CaseNumber { get; protected set; }
        public virtual string CurrencyIsoCode { get; protected set; }
        public virtual string GraphQLId { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual string ProcessorComments { get; protected set; }
        public virtual string MerchantAccountId { get; protected set; }
        public virtual string OriginalDisputeId { get; protected set; }
        public virtual string ReasonCode { get; protected set; }
        public virtual string ReasonDescription { get; protected set; }
        public virtual string ReferenceNumber { get; protected set; }
        public virtual TransactionDetails TransactionDetails { get; protected set; }
        public virtual DisputeTransaction Transaction { get; protected set; }
        public List<DisputeStatusHistory> StatusHistory;
        public List<DisputeEvidence> Evidence;
        public List<DisputePayPalMessage> PayPalMessages;

        public Dispute(NodeWrapper node)
        {
            Amount = node.GetDecimal("amount");
            AmountDisputed = node.GetDecimal("amount-disputed");
            AmountWon = node.GetDecimal("amount-won");
            CreatedAt = node.GetDateTime("created-at");
            DateOpened = node.GetDateTime("date-opened");
            DateWon = node.GetDateTime("date-won");
            ReceivedDate = node.GetDateTime("received-date");
            ReplyByDate = node.GetDateTime("reply-by-date");
            UpdatedAt = node.GetDateTime("updated-at");
            Reason = node.GetEnum("reason", DisputeReason.GENERAL);
            Status = node.GetEnum("status", DisputeStatus.UNRECOGNIZED);
            Kind = node.GetEnum("kind", DisputeKind.UNRECOGNIZED);
            #pragma warning disable 0618
            ChargebackProtectionLevel = node.GetEnum("chargeback-protection-level", DisputeChargebackProtectionLevel.NOT_PROTECTED);
            #pragma warning restore 0618
            switch (node.GetString("chargeback-protection-level")) {
                case "effortless":
                    ProtectionLevel = DisputeProtectionLevel.EFFORTLESS_CBP;
                    break;
                case "standard":
                    ProtectionLevel = DisputeProtectionLevel.STANDARD_CBP;
                    break;
                default:
                    ProtectionLevel = DisputeProtectionLevel.NO_PROTECTION;
                    break;
            }
            CaseNumber = node.GetString("case-number");
            CurrencyIsoCode = node.GetString("currency-iso-code");
            GraphQLId = node.GetString("global-id");
            Id = node.GetString("id");
            ProcessorComments = node.GetString("processor-comments");
            MerchantAccountId = node.GetString("merchant-account-id");
            OriginalDisputeId = node.GetString("original-dispute-id");
            ReasonCode = node.GetString("reason-code");
            ReasonDescription = node.GetString("reason-description");
            ReferenceNumber = node.GetString("reference-number");

            if (node.GetNode("transaction") != null) {
                TransactionDetails = new TransactionDetails(node.GetNode("transaction"));
                Transaction = new DisputeTransaction(node.GetNode("transaction"));
            }

            Evidence = new List<DisputeEvidence>();
            foreach (var evidenceResponse in node.GetList("evidence/evidence"))
            {
                Evidence.Add(new DisputeEvidence(evidenceResponse));
            }

            PayPalMessages = new List<DisputePayPalMessage>();
            foreach (var paypalMessageResponse in node.GetList("paypal-messages/paypal-messages"))
            {
                PayPalMessages.Add(new DisputePayPalMessage(paypalMessageResponse));
            }

            StatusHistory = new List<DisputeStatusHistory>();
            foreach (var historyStatusResponse in node.GetList("status-history/status-history"))
            {
                StatusHistory.Add(new DisputeStatusHistory(historyStatusResponse));
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal Dispute() { }
    }
}
