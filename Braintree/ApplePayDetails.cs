using System;

namespace Braintree
{
    public class ApplePayDetails
    {
        public String CardType { get; protected set; }
        public String CardholderName { get; protected set; }
        public String ExpirationMonth { get; protected set; }
        public String ExpirationYear { get; protected set; }
        public String Token { get; protected set; }

        protected internal ApplePayDetails(NodeWrapper node)
        {
            CardType = node.GetString("card-type");
            CardholderName = node.GetString("cardholder-name");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            Token = node.GetString("token");
        }
    }
}
