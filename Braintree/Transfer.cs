using System;

namespace Braintree
{
    public class Transfer
    {
        public String Id { get; protected set; }
        public Decimal? Amount { get; protected set; }
        public String Message { get; protected set; }
        public DateTime? DisbursementDate { get; protected set; }
        public String FollowUpAction { get; protected set; }

        private BraintreeService service;
        private String merchantAccountId;

        public Transfer(NodeWrapper node, BraintreeService braintreeService)
        {
          Id = node.GetString("id");
          Amount = node.GetDecimal("amount");
          Message = node.GetString("message");
          DisbursementDate = node.GetDateTime("disbursement-date");
          FollowUpAction = node.GetString("follow-up-action");

          merchantAccountId = node.GetString("merchant-account-id");
          service = braintreeService;
        }

        public MerchantAccount MerchantAccount
        {
          get
          {
            MerchantAccountGateway gateway = new MerchantAccountGateway(service);
            return gateway.Find(merchantAccountId);
          }
        }

        public ResourceCollection<Transaction> Transactions{
          get
          {
            TransactionGateway gateway = new TransactionGateway(service);

            TransactionSearchRequest searchRequest = new TransactionSearchRequest().
              MerchantAccountId.Is(merchantAccountId).
              DisbursementDate.Between(DisbursementDate.Value, DisbursementDate.Value);

            return gateway.Search(searchRequest);
          }
        }
    }
}
