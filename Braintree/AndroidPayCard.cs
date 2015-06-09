using System;

namespace Braintree
{
    public class AndroidPayCard : PaymentMethod
    {
        public String CardType { get; protected set; }
        public String Last4 { get; protected set; }
        public String SourceCardType { get; protected set; }
        public String SourceCardLast4 { get; protected set; }
        public String VirtualCardType { get; protected set; }
        public String VirtualCardLast4 { get; protected set; }
        public String ExpirationMonth { get; protected set; }
        public String ExpirationYear { get; protected set; }
        public String Token { get; protected set; }
        public String GoogleTransactionId { get; protected set; }
        public String Bin { get; protected set; }
        public Boolean? IsDefault { get; protected set; }
        public String ImageUrl { get; protected set; }
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
