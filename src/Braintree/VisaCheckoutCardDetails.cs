using System;

namespace Braintree
{
    public class VisaCheckoutCardDetails
    {

        private string _CountryOfIssuance;
        public virtual Address BillingAddress { get; protected set; }
        public virtual bool? IsExpired { get; protected set; }
        public virtual CreditCardCardType CardType { get; protected set; }
        public virtual CreditCardCommercial Commercial { get; protected set; }
        public virtual CreditCardCustomerLocation CustomerLocation { get; protected set; }
        public virtual CreditCardDebit Debit { get; protected set; }
        public virtual CreditCardDurbinRegulated DurbinRegulated { get; protected set; }
        public virtual CreditCardHealthcare Healthcare { get; protected set; }
        public virtual CreditCardPayroll Payroll { get; protected set; }
        public virtual CreditCardPrepaid Prepaid { get; protected set; }
        public virtual CreditCardPrepaidReloadable PrepaidReloadable { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual string Bin { get; protected set; }
        public virtual string CallId { get; protected set; }
        public virtual string CardholderName { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string LastFour { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual string UniqueNumberIdentifier { get; protected set; }

        public virtual string CountryOfIssuance
        {
            get
            {
                if (_CountryOfIssuance == "")
                {
                    return CreditCard.CountryOfIssuanceUnknown;
                }
                else
                {
                    return _CountryOfIssuance;
                }
            }
        }

        private string _IssuingBank;

        public virtual string IssuingBank
        {
            get
            {
                if (_IssuingBank == "")
                {
                    return CreditCard.IssuingBankUnknown;
                }
                else
                {
                    return _IssuingBank;
                }
            }
        }

        private string _ProductId;

        public virtual string ProductId
        {
            get
            {
                if (_ProductId == "")
                {
                    return CreditCard.ProductIdUnknown;
                }
                else
                {
                    return _ProductId;
                }
            }
        }

        public virtual string ExpirationDate
        {
            get => ExpirationMonth + "/" + ExpirationYear;
            protected set
            {
                ExpirationMonth = value.Split('/')[0];
                ExpirationYear = value.Split('/')[1];
            }
        }

        public string MaskedNumber => $"{Bin}******{LastFour}";

        protected internal VisaCheckoutCardDetails(NodeWrapper node)
        {
            if (node == null) return;

            _CountryOfIssuance = node.GetString("country-of-issuance");
            _IssuingBank = node.GetString("issuing-bank");
            _ProductId = node.GetString("product-id");
            BillingAddress = new Address(node.GetNode("billing-address"));
            Bin = node.GetString("bin");
            CallId = node.GetString("call-id");
            CardholderName = node.GetString("cardholder-name");
            CardType = node.GetEnum("card-type", CreditCardCardType.UNRECOGNIZED);
            Commercial = node.GetEnum("commercial", CreditCardCommercial.UNKNOWN);
            CreatedAt = node.GetDateTime("created-at");
            CustomerLocation = node.GetEnum("customer-location", CreditCardCustomerLocation.UNRECOGNIZED);
            Debit = node.GetEnum("debit", CreditCardDebit.UNKNOWN);
            DurbinRegulated = node.GetEnum("durbin-regulated", CreditCardDurbinRegulated.UNKNOWN);
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            Healthcare = node.GetEnum("healthcare", CreditCardHealthcare.UNKNOWN);
            ImageUrl = node.GetString("image-url");
            IsExpired = node.GetBoolean("expired");
            LastFour = node.GetString("last-4");
            Payroll = node.GetEnum("payroll", CreditCardPayroll.UNKNOWN);
            Prepaid = node.GetEnum("prepaid", CreditCardPrepaid.UNKNOWN);
            PrepaidReloadable = node.GetEnum("prepaid-reloadable", CreditCardPrepaidReloadable.UNKNOWN);
            Token = node.GetString("token");
            UniqueNumberIdentifier = node.GetString("unique-number-identifier");
            UpdatedAt = node.GetDateTime("updated-at");
        }
    }
}
