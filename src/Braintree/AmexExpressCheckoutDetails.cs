using System;

namespace Braintree
{
    public class AmexExpressCheckoutDetails
    {
        public virtual string Token { get; protected set; }
        public virtual string CardType { get; protected set; }
        public virtual string Bin { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual string CardMemberNumber { get; protected set; }
        public virtual string CardMemberExpiryDate { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string SourceDescription { get; protected set; }

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

        [Obsolete("Mock Use Only")]
        protected internal AmexExpressCheckoutDetails() { }
    }
}
