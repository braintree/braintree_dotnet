using System;

namespace Braintree
{
    public class ApplePayDetails
    {
        public virtual string Bin { get; protected set; }
        public virtual string CardholderName { get; protected set; }
        public virtual string CardType { get; protected set; }
        public virtual string Commercial { get; protected set; }
        public virtual string CountryOfIssuance { get; protected set; }
        public virtual string Debit { get; protected set; }
        public virtual string DurbinRegulated { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual string GlobalId { get; protected set; }
        public virtual string Healthcare { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string IssuingBank { get; protected set; }
        public virtual string LastFour { get; protected set; }
        public virtual string MerchantTokenIdentifier { get; protected set; }
        public virtual string PaymentInstrumentName { get; protected set; }
        public virtual string Payroll { get; protected set; }
        public virtual string Prepaid { get; protected set; }
        public virtual String PrepaidReloadable { get; protected set; }
        public virtual string ProductId { get; protected set; }
        public virtual string SourceCardLast4 { get; protected set; }
        public virtual string SourceDescription { get; protected set; }
        public virtual string Token { get; protected set; }

        protected internal ApplePayDetails(NodeWrapper node)
        {
            Bin = node.GetString("bin");
            CardholderName = node.GetString("cardholder-name");
            CardType = node.GetString("card-type");
            Commercial = node.GetString("commercial");
            CountryOfIssuance = node.GetString("country-of-issuance");
            Debit = node.GetString("debit");
            DurbinRegulated = node.GetString("durbin-regulated");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            GlobalId = node.GetString("global-id");
            Healthcare = node.GetString("healthcare");
            ImageUrl = node.GetString("image-url");
            IssuingBank = node.GetString("issuing-bank");
            LastFour = node.GetString("last-4");
            MerchantTokenIdentifier = node.GetString("merchant-token-identifier");
            PaymentInstrumentName = node.GetString("payment-instrument-name");
            Payroll = node.GetString("payroll");
            Prepaid = node.GetString("prepaid");
            PrepaidReloadable = node.GetString("prepaid-reloadable");
            ProductId = node.GetString("product-id");
            SourceCardLast4 = node.GetString("source-card-last4");
            SourceDescription = node.GetString("source-description");
            Token = node.GetString("token");
        }

        [Obsolete("Mock Use Only")]
        protected internal ApplePayDetails() { }
    }
}
