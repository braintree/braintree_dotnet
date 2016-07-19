using System;
using System.Collections;
using System.Collections.Generic;

namespace Braintree
{
    public class DisputeStatus : Enumeration
    {
        public static readonly DisputeStatus OPEN = new DisputeStatus("open");
        public static readonly DisputeStatus LOST = new DisputeStatus("lost");
        public static readonly DisputeStatus WON = new DisputeStatus("won");
        public static readonly DisputeStatus UNRECOGNIZED = new DisputeStatus("unrecognized");

        public static readonly DisputeStatus[] ALL = {
            OPEN, LOST, WON, UNRECOGNIZED
        };

        protected DisputeStatus(string name) : base(name) {}
    }

    public class DisputeKind : Enumeration
    {
        public static readonly DisputeKind CHARGEBACK = new DisputeKind("chargeback");
        public static readonly DisputeKind PRE_ARBITRATION = new DisputeKind("pre_arbitration");
        public static readonly DisputeKind RETRIEVAL = new DisputeKind("retrieval");
        public static readonly DisputeKind UNRECOGNIZED = new DisputeKind("unrecognized");

        public static readonly DisputeKind[] ALL = {
            CHARGEBACK, PRE_ARBITRATION, RETRIEVAL, UNRECOGNIZED
        };

        protected DisputeKind(string name) : base(name) {}
    }

    public class DisputeReason : Enumeration
    {
        public static readonly DisputeReason CANCELLED_RECURRING_TRANSACTION = new DisputeReason("cancelled_recurring_transaction");
        public static readonly DisputeReason CREDIT_NOT_PROCESSED = new DisputeReason("credit_not_processed");
        public static readonly DisputeReason DUPLICATE = new DisputeReason("duplicate");
        public static readonly DisputeReason FRAUD = new DisputeReason("fraud");
        public static readonly DisputeReason GENERAL = new DisputeReason("general");
        public static readonly DisputeReason INVALID_ACCOUNT = new DisputeReason("invalid_account");
        public static readonly DisputeReason NOT_RECOGNIZED = new DisputeReason("not_recognized");
        public static readonly DisputeReason PRODUCT_NOT_RECEIVED = new DisputeReason("product_not_received");
        public static readonly DisputeReason PRODUCT_UNSATISFACTORY = new DisputeReason("product_unsatisfactory");
        public static readonly DisputeReason TRANSACTION_AMOUNT_DIFFERS = new DisputeReason("transaction_amount_differs");
        public static readonly DisputeReason RETRIEVAL = new DisputeReason("retrieval");


        public static readonly DisputeReason[] ALL = {
          CANCELLED_RECURRING_TRANSACTION, CREDIT_NOT_PROCESSED, DUPLICATE, FRAUD, GENERAL, INVALID_ACCOUNT, NOT_RECOGNIZED, PRODUCT_NOT_RECEIVED, PRODUCT_UNSATISFACTORY, TRANSACTION_AMOUNT_DIFFERS, RETRIEVAL
        };

        protected DisputeReason(string name) : base(name) {}
    }

    public class Dispute
    {
        public virtual decimal? Amount { get; protected set; }
        public virtual DateTime? ReceivedDate { get; protected set; }
        public virtual DateTime? ReplyByDate { get; protected set; }
        public virtual DateTime? DateOpened { get; protected set; }
        public virtual DateTime? DateWon { get; protected set; }
        public virtual DisputeReason Reason { get; protected set; }
        public virtual DisputeStatus Status { get; protected set; }
        public virtual DisputeKind Kind { get; protected set; }
        public virtual string CurrencyIsoCode { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual TransactionDetails TransactionDetails { get; protected set; }

        public Dispute(NodeWrapper node)
        {
            Amount = node.GetDecimal("amount");
            ReceivedDate = node.GetDateTime("received-date");
            ReplyByDate = node.GetDateTime("reply-by-date");
            DateOpened = node.GetDateTime("date-opened");
            DateWon = node.GetDateTime("date-won");
            Reason = (DisputeReason)CollectionUtil.Find(DisputeReason.ALL, node.GetString("reason"), DisputeReason.GENERAL);
            Status = (DisputeStatus)CollectionUtil.Find(DisputeStatus.ALL, node.GetString("status"), DisputeStatus.UNRECOGNIZED);
            Kind = (DisputeKind)CollectionUtil.Find(DisputeKind.ALL, node.GetString("kind"), DisputeKind.UNRECOGNIZED);
            CurrencyIsoCode = node.GetString("currency-iso-code");
            Id = node.GetString("id");
            TransactionDetails = new TransactionDetails(node.GetNode("transaction"));
        }

        [Obsolete("Mock Use Only")]
        protected internal Dispute() { }
    }
}
