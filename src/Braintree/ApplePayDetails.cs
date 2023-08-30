using System;

namespace Braintree
{
    public class ApplePayDetails
    {
        public virtual string CardType { get; protected set; }
        public virtual string PaymentInstrumentName { get; protected set; }
        public virtual string SourceDescription { get; protected set; }
        public virtual string CardholderName { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual string LastFour { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string Prepaid { get; protected set; }
        public virtual string Healthcare { get; protected set; }
        public virtual string Debit { get; protected set; }
        public virtual string DurbinRegulated { get; protected set; }
        public virtual string Commercial { get; protected set; }
        public virtual string Payroll { get; protected set; }
        public virtual string IssuingBank { get; protected set; }
        public virtual string CountryOfIssuance { get; protected set; }
        public virtual string ProductId { get; protected set; }
        public virtual string Bin { get; protected set; }
        public virtual string GlobalId { get; protected set; }
        public virtual string MerchantTokenIdentifier { get; protected set; }
        public virtual string SourceCardLast4 { get; protected set; }

        protected internal ApplePayDetails(NodeWrapper node)
        {
            CardType = node.GetString("card-type");
            PaymentInstrumentName = node.GetString("payment-instrument-name");
            SourceDescription = node.GetString("source-description");
            CardholderName = node.GetString("cardholder-name");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            Token = node.GetString("token");
            LastFour = node.GetString("last-4");
            ImageUrl = node.GetString("image-url");
            Prepaid = node.GetString("prepaid");
            Healthcare = node.GetString("healthcare");
            Debit = node.GetString("debit");
            DurbinRegulated = node.GetString("durbin-regulated");
            Commercial = node.GetString("commercial");
            Payroll = node.GetString("payroll");
            IssuingBank = node.GetString("issuing-bank");
            CountryOfIssuance = node.GetString("country-of-issuance");
            ProductId = node.GetString("product-id");
            Bin = node.GetString("bin");
            GlobalId = node.GetString("global-id");
            MerchantTokenIdentifier = node.GetString("merchant-token-identifier");
            SourceCardLast4 = node.GetString("source-card-last4");
        }

        [Obsolete("Mock Use Only")]
        protected internal ApplePayDetails() { }
    }
}
