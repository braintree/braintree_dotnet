#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace Braintree
{
    public class VisaCheckoutCard : PaymentMethod
    {

        private string _CountryOfIssuance;
        public virtual Address BillingAddress { get; protected set; }
        public virtual bool? IsDefault { get; protected set; }
        public virtual bool? IsExpired { get; protected set; }
        public virtual CreditCardBusiness Business { get; protected set; }
        public virtual CreditCardCardType CardType { get; protected set; }
        public virtual CreditCardCommercial Commercial { get; protected set; }
        public virtual CreditCardConsumer Consumer { get; protected set; }
        public virtual CreditCardCorporate Corporate { get; protected set; }
        public virtual CreditCardCustomerLocation CustomerLocation { get; protected set; }
        public virtual CreditCardDebit Debit { get; protected set; }
        public virtual CreditCardDurbinRegulated DurbinRegulated { get; protected set; }
        public virtual CreditCardHealthcare Healthcare { get; protected set; }
        public virtual CreditCardPayroll Payroll { get; protected set; }
        public virtual CreditCardPrepaid Prepaid { get; protected set; }
        public virtual CreditCardPrepaidReloadable PrepaidReloadable { get; protected set; }
        public virtual CreditCardPurchase Purchase{ get; protected set; }
        public virtual CreditCardVerification Verification { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual string Bin { get; protected set; }
        public virtual string CallId { get; protected set; }
        public virtual string CardholderName { get; protected set; }
        public virtual string CustomerId { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string LastFour { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual string UniqueNumberIdentifier { get; protected set; }
        public virtual Subscription[] Subscriptions { get; protected set; }

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

        protected internal VisaCheckoutCard(NodeWrapper node, IBraintreeGateway gateway)
        {
            if (node == null) return;

            _CountryOfIssuance = node.GetString("country-of-issuance");
            _IssuingBank = node.GetString("issuing-bank");
            _ProductId = node.GetString("product-id");
            BillingAddress = new Address(node.GetNode("billing-address"));
            Bin = node.GetString("bin");
            Business = node.GetEnum("business", CreditCardBusiness.UNKNOWN);
            CallId = node.GetString("call-id");
            CardholderName = node.GetString("cardholder-name");
            CardType = node.GetEnum("card-type", CreditCardCardType.UNRECOGNIZED);
            Commercial = node.GetEnum("commercial", CreditCardCommercial.UNKNOWN);
            Consumer = node.GetEnum("consumer", CreditCardConsumer.UNKNOWN);
            Corporate = node.GetEnum("corporate", CreditCardCorporate.UNKNOWN);
            CreatedAt = node.GetDateTime("created-at");
            CustomerId = node.GetString("customer-id");
            CustomerLocation = node.GetEnum("customer-location", CreditCardCustomerLocation.UNRECOGNIZED);
            Debit = node.GetEnum("debit", CreditCardDebit.UNKNOWN);
            DurbinRegulated = node.GetEnum("durbin-regulated", CreditCardDurbinRegulated.UNKNOWN);
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            Healthcare = node.GetEnum("healthcare", CreditCardHealthcare.UNKNOWN);
            ImageUrl = node.GetString("image-url");
            IsDefault = node.GetBoolean("default");
            IsExpired = node.GetBoolean("expired");
            LastFour = node.GetString("last-4");
            Payroll = node.GetEnum("payroll", CreditCardPayroll.UNKNOWN);
            Prepaid = node.GetEnum("prepaid", CreditCardPrepaid.UNKNOWN);
            PrepaidReloadable = node.GetEnum("prepaid-reloadable", CreditCardPrepaidReloadable.UNKNOWN);
            Purchase = node.GetEnum("purchase", CreditCardPurchase.UNKNOWN);
            Token = node.GetString("token");
            UniqueNumberIdentifier = node.GetString("unique-number-identifier");
            UpdatedAt = node.GetDateTime("updated-at");

            var subscriptionXmlNodes = node.GetList("subscriptions/subscription");
            Subscriptions = new Subscription[subscriptionXmlNodes.Count];
            for (int i = 0; i < subscriptionXmlNodes.Count; i++)
            {
                Subscriptions[i] = new Subscription(subscriptionXmlNodes[i], gateway);
            }

            var verificationNodes = node.GetList("verifications/verification");
            Verification = FindLatestVerification(verificationNodes, gateway);
        }

        private CreditCardVerification FindLatestVerification(List<NodeWrapper> verificationNodes, IBraintreeGateway gateway) {
            if(verificationNodes.Count > 0)
            {
                verificationNodes.Sort(delegate(NodeWrapper first, NodeWrapper second) {
                    DateTime time1 = (DateTime)first.GetDateTime("created-at");
                    DateTime time2 = (DateTime)second.GetDateTime("created-at");

                    return DateTime.Compare(time2, time1);
                });

                return new CreditCardVerification(verificationNodes[0], gateway);
            }

            return null;
        }
    }
}
