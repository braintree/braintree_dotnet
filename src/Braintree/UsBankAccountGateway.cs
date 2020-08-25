using System.Xml;

namespace Braintree
{
    public class UsBankAccountGateway : IUsBankAccountGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public UsBankAccountGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = gateway.Service;
        }

        public UsBankAccount Find(string token)
        {
            XmlNode xml = service.Get(service.MerchantPath() + "/payment_methods/us_bank_account/" + token);

            return new UsBankAccount(new NodeWrapper(xml));
        }

        public Result<Transaction> Sale(string token, TransactionRequest transactionRequest)
        {
            if (transactionRequest.Options == null) {
                transactionRequest.Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                };
            }

            transactionRequest.PaymentMethodToken = token;
            return gateway.Transaction.Sale(transactionRequest);
        }
    }
}
