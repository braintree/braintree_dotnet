#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace Braintree
{
    public class SamsungPayCard : PaymentMethod
    {
        public virtual string Bin { get; protected set; }
        public virtual CreditCardCardType CardType { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual string CustomerId { get; protected set; }
        public virtual CreditCardCustomerLocation CustomerLocation { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual bool? IsDefault { get; protected set; }
        public virtual bool? IsExpired { get; protected set; }
        public virtual string LastFour { get; protected set; }
        public virtual string SourceCardLastFour { get; protected set; }
        public virtual Subscription[] Subscriptions { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual string UniqueNumberIdentifier { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual CreditCardPrepaid Prepaid { get; protected set; }
        public virtual CreditCardPayroll Payroll { get; protected set; }
        public virtual CreditCardDebit Debit { get; protected set; }
        public virtual CreditCardCommercial Commercial { get; protected set; }
        public virtual CreditCardHealthcare Healthcare { get; protected set; }
        public virtual CreditCardDurbinRegulated DurbinRegulated { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual Address BillingAddress { get; protected set; }
        public virtual string CardholderName { get; protected set; }

        private string _CountryOfIssuance;

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
            get
            {
                return ExpirationMonth + "/" + ExpirationYear;
            }
            protected set
            {
                ExpirationMonth = value.Split('/')[0];
                ExpirationYear = value.Split('/')[1];
            }
        }

        public string MaskedNumber
        {
            get
            {
                return string.Format("{0}******{1}", Bin, LastFour);
            }
        }

        protected internal SamsungPayCard(NodeWrapper node, IBraintreeGateway gateway)
        {
            if (node == null) return;

            Bin = node.GetString("bin");
            CardType = (CreditCardCardType)CollectionUtil.Find(CreditCardCardType.ALL, node.GetString("card-type"), CreditCardCardType.UNRECOGNIZED);
            CustomerId = node.GetString("customer-id");
            IsDefault = node.GetBoolean("default");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            IsExpired = node.GetBoolean("expired");
            CustomerLocation = (CreditCardCustomerLocation)CollectionUtil.Find(CreditCardCustomerLocation.ALL, node.GetString("customer-location"), CreditCardCustomerLocation.UNRECOGNIZED);
            LastFour = node.GetString("last-4");
            SourceCardLastFour = node.GetString("source-card-last-4");
            UniqueNumberIdentifier = node.GetString("unique-number-identifier");
            Token = node.GetString("token");
            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");
            Prepaid = (CreditCardPrepaid)CollectionUtil.Find(CreditCardPrepaid.ALL, node.GetString("prepaid"), CreditCardPrepaid.UNKNOWN);
            Payroll = (CreditCardPayroll)CollectionUtil.Find(CreditCardPayroll.ALL, node.GetString("payroll"), CreditCardPayroll.UNKNOWN);
            DurbinRegulated = (CreditCardDurbinRegulated)CollectionUtil.Find(CreditCardDurbinRegulated.ALL, node.GetString("durbin-regulated"), CreditCardDurbinRegulated.UNKNOWN);
            Debit = (CreditCardDebit)CollectionUtil.Find(CreditCardDebit.ALL, node.GetString("debit"), CreditCardDebit.UNKNOWN);
            Commercial = (CreditCardCommercial)CollectionUtil.Find(CreditCardCommercial.ALL, node.GetString("commercial"), CreditCardCommercial.UNKNOWN);
            Healthcare = (CreditCardHealthcare)CollectionUtil.Find(CreditCardHealthcare.ALL, node.GetString("healthcare"), CreditCardHealthcare.UNKNOWN);
            _CountryOfIssuance = node.GetString("country-of-issuance");
            _IssuingBank = node.GetString("issuing-bank");
            _ProductId = node.GetString("product-id");
            ImageUrl = node.GetString("image-url");
            BillingAddress = new Address(node.GetNode("billing-address"));
            CardholderName = node.GetString("cardholder-name");

            var subscriptionXmlNodes = node.GetList("subscriptions/subscription");
            Subscriptions = new Subscription[subscriptionXmlNodes.Count];
            for (int i = 0; i < subscriptionXmlNodes.Count; i++)
            {
                Subscriptions[i] = new Subscription(subscriptionXmlNodes[i], gateway);
            }
        }
    }
}
