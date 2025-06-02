using System;

namespace Braintree
{
    // NEXT_MAJOR_VERSION Rename Android Pay to Google Pay
    public class AndroidPayCard : PaymentMethod
    {
        public virtual bool? IsDefault { get; protected set; }
        public virtual bool? IsNetworkTokenized { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual string Bin { get; protected set; }
        public virtual string Business { get; protected set; }
        public virtual string CardType { get; protected set; }
        public virtual string Commercial { get; protected set; }
        public virtual string Consumer { get; protected set; }
        public virtual string Corporate { get; protected set; }
        public virtual string CountryOfIssuance { get; protected set; }
        public virtual string CustomerId { get; protected set; }
        public virtual string Debit { get; protected set; }
        public virtual string DurbinRegulated { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual string GoogleTransactionId { get; protected set; }
        public virtual string Healthcare { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string IssuingBank { get; protected set; }
        public virtual string Last4 { get; protected set; }
        public virtual string Payroll { get; protected set; }
        public virtual string Prepaid { get; protected set; }
        public virtual string PrepaidReloadable { get; protected set; }
        public virtual string ProductId { get; protected set; }
        public virtual string Purchase { get; protected set; }
        public virtual string SourceCardLast4 { get; protected set; }
        public virtual string SourceCardType { get; protected set; }
        public virtual string SourceDescription { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual string VirtualCardLast4 { get; protected set; }
        public virtual string VirtualCardType { get; protected set; }
        public virtual Subscription[] Subscriptions { get; protected set; }

        protected internal AndroidPayCard(NodeWrapper node, IBraintreeGateway gateway)
        {
            Bin = node.GetString("bin");
            Business = node.GetString("business");
            CardType = node.GetString("virtual-card-type");
            Commercial = node.GetString("commercial");
            Consumer = node.GetString("consumer");
            Corporate = node.GetString("corporate");
            CountryOfIssuance = node.GetString("country-of-issuance");
            CreatedAt = node.GetDateTime("created-at");
            CustomerId = node.GetString("customer-id");
            Debit = node.GetString("debit");
            DurbinRegulated = node.GetString("durbin-regulated");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            GoogleTransactionId = node.GetString("google-transaction-id");
            Healthcare = node.GetString("healthcare");
            ImageUrl = node.GetString("image-url");
            IsDefault = node.GetBoolean("default");
            IsNetworkTokenized = node.GetBoolean("is-network-tokenized");
            IssuingBank = node.GetString("issuing-bank");
            Last4 = node.GetString("virtual-card-last-4");
            Payroll = node.GetString("payroll");
            Prepaid = node.GetString("prepaid");
            PrepaidReloadable = node.GetString("prepaid-reloadable");
            ProductId = node.GetString("product-id");
            Purchase = node.GetString("purchase");
            SourceCardLast4 = node.GetString("source-card-last-4");
            SourceCardType = node.GetString("source-card-type");
            SourceDescription = node.GetString("source-description");
            Token = node.GetString("token");
            UpdatedAt = node.GetDateTime("updated-at");
            VirtualCardLast4 = node.GetString("virtual-card-last-4");
            VirtualCardType = node.GetString("virtual-card-type");

            var subscriptionXmlNodes = node.GetList("subscriptions/subscription");
            Subscriptions = new Subscription[subscriptionXmlNodes.Count];
            for (int i = 0; i < subscriptionXmlNodes.Count; i++)
            {
                Subscriptions[i] = new Subscription(subscriptionXmlNodes[i], gateway);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal AndroidPayCard() { }
    }
}
