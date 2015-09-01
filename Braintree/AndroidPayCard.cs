using System;

namespace Braintree
{
    public class AndroidPayCard : PaymentMethod
    {
        public string CardType { get; protected set; }
        public string Last4 { get; protected set; }
        public string SourceCardType { get; protected set; }
        public string SourceCardLast4 { get; protected set; }
        public string SourceDescription { get; protected set; }
        public string VirtualCardType { get; protected set; }
        public string VirtualCardLast4 { get; protected set; }
        public string ExpirationMonth { get; protected set; }
        public string ExpirationYear { get; protected set; }
        public string Token { get; protected set; }
        public string GoogleTransactionId { get; protected set; }
        public string Bin { get; protected set; }
        public bool? IsDefault { get; protected set; }
        public string ImageUrl { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public Subscription[] Subscriptions { get; protected set; }

        protected internal AndroidPayCard(NodeWrapper node, BraintreeGateway gateway)
        {
            CardType = node.GetString("virtual-card-type");
            VirtualCardType = node.GetString("virtual-card-type");
            SourceCardType = node.GetString("source-card-type");
            Last4 = node.GetString("virtual-card-last-4");
            SourceCardLast4 = node.GetString("source-card-last-4");
            VirtualCardLast4 = node.GetString("virtual-card-last-4");
            SourceDescription = node.GetString("source-description");
            Bin = node.GetString("bin");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            GoogleTransactionId = node.GetString("google-transaction-id");
            Token = node.GetString("token");
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
