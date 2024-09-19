using System;

namespace Braintree
{
    public class PayPalAccount : PaymentMethod
    {
        public virtual string BillingAgreementId { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual string CustomerId { get; protected set; }
        public virtual string EditPayPalVaultId {get; protected set;}
        public virtual string Email { get; protected set; }
        public virtual string FundingSourceDescription { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual bool? IsDefault { get; protected set; }
        public virtual string PayerId { get; protected set; }
        public virtual DateTime? RevokedAt { get; protected set; }
        public virtual Subscription[] Subscriptions { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }

        protected internal PayPalAccount(NodeWrapper node, IBraintreeGateway gateway)
        {
            BillingAgreementId = node.GetString("billing-agreement-id");
            CreatedAt = node.GetDateTime("created-at");
            CustomerId = node.GetString("customer-id");
            EditPayPalVaultId = node.GetString("edit-paypal-vault-id");
            Email = node.GetString("email");
            FundingSourceDescription = node.GetString("funding-source-description");
            ImageUrl = node.GetString("image-url");
            IsDefault = node.GetBoolean("default");
            PayerId = node.GetString("payer-id");
            RevokedAt = node.GetDateTime("revoked-at");

            var subscriptionXmlNodes = node.GetList("subscriptions/subscription");
            Subscriptions = new Subscription[subscriptionXmlNodes.Count];
            for (int i = 0; i < subscriptionXmlNodes.Count; i++)
            {
                Subscriptions[i] = new Subscription(subscriptionXmlNodes[i], gateway);
            }

            Token = node.GetString("token");
            UpdatedAt = node.GetDateTime("updated-at");
        }

        [Obsolete("Mock Use Only")]
        protected internal PayPalAccount() { }
    }
}
