using System;

namespace Braintree
{
    public class AmexExpressCheckoutDetails
    {
        public string Token { get; protected set; }
        public string CardType { get; protected set; }
        public string Bin { get; protected set; }
        public string ExpirationMonth { get; protected set; }
        public string ExpirationYear { get; protected set; }
        public string CardMemberNumber { get; protected set; }
        public string CardMemberExpiryDate { get; protected set; }
        public string ImageUrl { get; protected set; }
        public string SourceDescription { get; protected set; }

        protected internal AmexExpressCheckoutDetails(NodeWrapper node)
        {
            Token = node.GetString("token");
            CardType = node.GetString("card-type");
            Bin = node.GetString("bin");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            CardMemberNumber = node.GetString("card-member-number");
            CardMemberExpiryDate = node.GetString("card-member-expiry-date");
            ImageUrl = node.GetString("image-url");
            SourceDescription = node.GetString("source-description");
        }
    }
}
