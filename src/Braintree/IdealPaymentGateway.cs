using System.Xml;

// NEXT_MAJOR_VERSION Remove this class as legacy Ideal has been removed/disabled in the Braintree Gateway
// DEPRECATED If you're looking to accept iDEAL as a payment method contact accounts@braintreepayments.com for a solution.
namespace Braintree
{
    public class IdealPaymentGateway : IIdealPaymentGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public IdealPaymentGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = new BraintreeService(gateway.Configuration);
        }

        public IdealPayment Find(string idealPaymentId)
        {
            XmlNode xml = service.Get(service.MerchantPath() + "/ideal_payments/" + idealPaymentId);

            return new IdealPayment(new NodeWrapper(xml));
        }

        public Result<Transaction> Sale(string idealPaymentId, TransactionRequest transactionRequest)
        {
            if (transactionRequest.Options == null) {
                transactionRequest.Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                };
            }

            transactionRequest.PaymentMethodNonce = idealPaymentId;
            return gateway.Transaction.Sale(transactionRequest);
        }
    }
}
