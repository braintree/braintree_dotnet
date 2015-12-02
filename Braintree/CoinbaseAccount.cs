using System;

namespace Braintree
{
    public class CoinbaseAccount : PaymentMethod
    {
        public virtual string UserId { get; protected set; }
        public virtual string UserEmail { get; protected set; }
        public virtual string UserName { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual bool? IsDefault { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string CustomerId { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual Subscription[] Subscriptions { get; protected set; }

        protected internal CoinbaseAccount(NodeWrapper node, IBraintreeGateway gateway)
        {
            UserId = node.GetString("user-id");
            UserEmail = node.GetString("user-email");
            UserName = node.GetString("user-name");

            Token = node.GetString("token");
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
        protected internal CoinbaseAccount() { }
    }
}
