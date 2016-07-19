using System;

namespace Braintree
{
    public class AmexExpressCheckoutCard : PaymentMethod
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
        public virtual bool? IsDefault { get; protected set; }
        public virtual string CustomerId { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual Subscription[] Subscriptions { get; protected set; }

        protected internal AmexExpressCheckoutCard(NodeWrapper node, IBraintreeGateway gateway)
        {
            Token = node.GetString("token");
            CardType = node.GetString("card-type");
            Bin = node.GetString("bin");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            CardMemberNumber = node.GetString("card-member-number");
            CardMemberExpiryDate = node.GetString("card-member-expiry-date");
            SourceDescription = node.GetString("source-description");
            IsDefault = node.GetBoolean("default");
            ImageUrl = node.GetString("image-url");
            CustomerId = node.GetString("customer-id");

            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");

            var subscriptionXmlNodes = node.GetList("subscriptions/subscription");
            Subscriptions = new Subscription[subscriptionXmlNodes.Count];
            for (int i = 0; i < subscriptionXmlNodes.Count; i++)
            {
                Subscriptions[i] = new Subscription(subscriptionXmlNodes[i], gateway);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal AmexExpressCheckoutCard() { }
    }
}
