using System;
using System.Collections.Generic;

namespace Braintree
{
    public class DisbursementType : Enumeration
    {
        public static readonly DisbursementType UNKNOWN = new DisbursementType("unknown");
        public static readonly DisbursementType CREDIT = new DisbursementType("credit");
        public static readonly DisbursementType DEBIT = new DisbursementType("debit");

        public static readonly DisbursementType[] ALL = {UNKNOWN, CREDIT, DEBIT};

        protected DisbursementType(string name) : base(name) {}
    }

    public class Disbursement
    {
        public virtual string Id { get; protected set; }
        public virtual decimal? Amount { get; protected set; }
        public virtual string ExceptionMessage { get; protected set; }
        public virtual DateTime? DisbursementDate { get; protected set; }
        public virtual DisbursementType DisbursementType { get; protected set; }
        public virtual string FollowUpAction { get; protected set; }
        public virtual MerchantAccount MerchantAccount { get; protected set; }
        public virtual List<string> TransactionIds { get; protected set; }
        public virtual bool? Success { get; protected set; }
        public virtual bool? Retry { get; protected set; }

        private IBraintreeGateway gateway;

        public Disbursement(NodeWrapper node, IBraintreeGateway gateway)
        {
            Id = node.GetString("id");
            Amount = node.GetDecimal("amount");
            ExceptionMessage = node.GetString("exception-message");
            DisbursementDate = node.GetDateTime("disbursement-date");
            DisbursementType = (DisbursementType)CollectionUtil.Find(
                    DisbursementType.ALL,
                    node.GetString("disbursement-type"),
                    DisbursementType.UNKNOWN
                    );
            FollowUpAction = node.GetString("follow-up-action");
            MerchantAccount = new MerchantAccount(node.GetNode("merchant-account"));
            TransactionIds = new List<string>();
            foreach (var stringNode in node.GetList("transaction-ids/item")) 
            {
                TransactionIds.Add(stringNode.GetString("."));
            }
            Success = node.GetBoolean("success");
            Retry = node.GetBoolean("retry");
            this.gateway = gateway;
        }

        [Obsolete("Mock Use Only")]
        protected internal Disbursement() { }

        public virtual ResourceCollection<Transaction> Transactions()
        {
            var gateway = new TransactionGateway(this.gateway);

            var searchRequest = new TransactionSearchRequest().
                Ids.IncludedIn(TransactionIds.ToArray());

            return gateway.Search(searchRequest);
        }
    }
}
