using System;

namespace Braintree
{
    public class PayPalAccount : PaymentMethod
    {
        public virtual string Email { get; protected set; }
        public virtual string BillingAgreementId { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual bool? IsDefault { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string PayerId { get; protected set; }
        public virtual string CustomerId { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual Subscription[] Subscriptions { get; protected set; }
        public virtual DateTime? RevokedAt { get; protected set; }

        protected internal PayPalAccount(NodeWrapper node, IBraintreeGateway gateway)
        {
            Email = node.GetString("email");
            BillingAgreementId = node.GetString("billing-agreement-id");
            Token = node.GetString("token");
            IsDefault = node.GetBoolean("default");
            ImageUrl = node.GetString("image-url");
            PayerId = node.GetString("payer-id");
            CustomerId = node.GetString("customer-id");
            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");

            var subscriptionXmlNodes = node.GetList("subscriptions/subscription");
            Subscriptions = new Subscription[subscriptionXmlNodes.Count];
            for (int i = 0; i < subscriptionXmlNodes.Count; i++)
            {
                Subscriptions[i] = new Subscription(subscriptionXmlNodes[i], gateway);
            }

            RevokedAt = node.GetDateTime("revoked-at");
        }

        [Obsolete("Mock Use Only")]
        protected internal PayPalAccount() { }
    }
}
