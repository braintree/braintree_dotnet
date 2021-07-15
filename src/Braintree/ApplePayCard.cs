using System;

namespace Braintree
{
    public class ApplePayCard : PaymentMethod
    {
        public virtual string Bin { get; protected set; }
        public virtual string CardType { get; protected set; }
        public virtual string Last4 { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual string PaymentInstrumentName { get; protected set; }
        public virtual string SourceDescription { get; protected set; }
        public virtual bool? IsDefault { get; protected set; }
        public virtual bool? IsExpired { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string CustomerId { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual Subscription[] Subscriptions { get; protected set; }
        public virtual string Prepaid { get; protected set; }
        public virtual string Healthcare { get; protected set; }
        public virtual string Debit { get; protected set; }
        public virtual string DurbinRegulated { get; protected set; }
        public virtual string Commercial { get; protected set; }
        public virtual string Payroll { get; protected set; }
        public virtual string IssuingBank { get; protected set; }
        public virtual string CountryOfIssuance { get; protected set; }
        public virtual string ProductId { get; protected set; }

        protected internal ApplePayCard(NodeWrapper node, IBraintreeGateway gateway)
        {
            Bin = node.GetString("bin");
            CardType = node.GetString("card-type");
            Last4 = node.GetString("last-4");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            Token = node.GetString("token");
            PaymentInstrumentName = node.GetString("payment-instrument-name");
            SourceDescription = node.GetString("source-description");
            IsDefault = node.GetBoolean("default");
            IsExpired = node.GetBoolean("expired");
            ImageUrl = node.GetString("image-url");
            CustomerId = node.GetString("customer-id");
            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");
            Prepaid = node.GetString("prepaid");
            Healthcare = node.GetString("healthcare");
            Debit = node.GetString("debit");
            DurbinRegulated = node.GetString("durbin-regulated");
            Commercial = node.GetString("commercial");
            Payroll = node.GetString("payroll");
            IssuingBank = node.GetString("issuing-bank");
            CountryOfIssuance = node.GetString("country-of-issuance");
            ProductId = node.GetString("product-id");

            var subscriptionXmlNodes = node.GetList("subscriptions/subscription");
            Subscriptions = new Subscription[subscriptionXmlNodes.Count];
            for (int i = 0; i < subscriptionXmlNodes.Count; i++)
            {
                Subscriptions[i] = new Subscription(subscriptionXmlNodes[i], gateway);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal ApplePayCard() { }
    }
}
