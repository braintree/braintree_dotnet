using System;

namespace Braintree
{
    public class ApplePayDetails
    {
        public string CardType { get; protected set; }
        public string PaymentInstrumentName { get; protected set; }
        public string SourceDescription { get; protected set; }
        public string CardholderName { get; protected set; }
        public string ExpirationMonth { get; protected set; }
        public string ExpirationYear { get; protected set; }
        public string Token { get; protected set; }

        protected internal ApplePayDetails(NodeWrapper node)
        {
            CardType = node.GetString("card-type");
            PaymentInstrumentName = node.GetString("payment-instrument-name");
            SourceDescription = node.GetString("source-description");
            CardholderName = node.GetString("cardholder-name");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            Token = node.GetString("token");
        }
    }
}
