using System;

namespace Braintree
{
    public class ApplePayDetails
    {
        public virtual string CardType { get; protected set; }
        public virtual string PaymentInstrumentName { get; protected set; }
        public virtual string SourceDescription { get; protected set; }
        public virtual string CardholderName { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual string LastFour { get; protected set; }
        public virtual string ImageUrl { get; protected set; }

        protected internal ApplePayDetails(NodeWrapper node)
        {
            CardType = node.GetString("card-type");
            PaymentInstrumentName = node.GetString("payment-instrument-name");
            SourceDescription = node.GetString("source-description");
            CardholderName = node.GetString("cardholder-name");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            Token = node.GetString("token");
            LastFour = node.GetString("last-4");
            ImageUrl = node.GetString("image-url");
        }

        [Obsolete("Mock Use Only")]
        protected internal ApplePayDetails() { }
    }
}
