using System;

namespace Braintree
{
    public class ApplePayCard : PaymentMethod
    {
        public string CardType { get; protected set; }
        public string Last4 { get; protected set; }
        public string ExpirationMonth { get; protected set; }
        public string ExpirationYear { get; protected set; }
        public string Token { get; protected set; }
        public string PaymentInstrumentName { get; protected set; }
        public string SourceDescription { get; protected set; }
        public bool? IsDefault { get; protected set; }
        public string ImageUrl { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public Subscription[] Subscriptions { get; protected set; }

        protected internal ApplePayCard(NodeWrapper node, BraintreeGateway gateway)
        {
            CardType = node.GetString("card-type");
            Last4 = node.GetString("last-4");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            Token = node.GetString("token");
            PaymentInstrumentName = node.GetString("payment-instrument-name");
            SourceDescription = node.GetString("source-description");
            IsDefault = node.GetBoolean("default");
            ImageUrl = node.GetString("image-url");

            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");

            var subscriptionXmlNodes = node.GetList("subscriptions/subscription");
            Subscriptions = new Subscription[subscriptionXmlNodes.Count];
            for (int i = 0; i < subscriptionXmlNodes.Count; i++)
            {
                Subscriptions[i] = new Subscription(subscriptionXmlNodes[i], gateway);
            }
        }
    }
}
