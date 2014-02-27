using System;
using System.Collections;
using System.Collections.Generic;

namespace Braintree
{
    public class Disbursement
    {
        public String Id { get; protected set; }
        public Decimal? Amount { get; protected set; }
        public String ExceptionMessage { get; protected set; }
        public DateTime? DisbursementDate { get; protected set; }
        public String FollowUpAction { get; protected set; }
        public MerchantAccount MerchantAccount { get; protected set; }
        public List<String> TransactionIds { get; protected set; }
        public Boolean? Success { get; protected set; }
        public Boolean? Retry { get; protected set; }

        private BraintreeService service;

        public Disbursement(NodeWrapper node, BraintreeService braintreeService)
        {
            Id = node.GetString("id");
            Amount = node.GetDecimal("amount");
            ExceptionMessage = node.GetString("exception-message");
            DisbursementDate = node.GetDateTime("disbursement-date");
            FollowUpAction = node.GetString("follow-up-action");
            MerchantAccount = new MerchantAccount(node.GetNode("merchant-account"));
            TransactionIds = new List<String>();
            foreach (NodeWrapper stringNode in node.GetList("transaction-ids/item")) 
            {
                TransactionIds.Add(stringNode.GetString("."));
            }
            Success = node.GetBoolean("success");
            Retry = node.GetBoolean("retry");
            service = braintreeService;
        }

        public ResourceCollection<Transaction> Transactions()
        {
            TransactionGateway gateway = new TransactionGateway(service);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
                Ids.IncludedIn(TransactionIds.ToArray());

            return gateway.Search(searchRequest);
        }
    }
}
