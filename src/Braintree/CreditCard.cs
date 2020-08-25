#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Braintree
{

    public enum CreditCardCustomerLocation
    {
        [Description("us")] US,
        [Description("international")] INTERNATIONAL,
        [Description("unrecognized")] UNRECOGNIZED
    }

    public enum CreditCardPrepaid
    {
        [Description("Yes")] YES,
        [Description("No")] NO,
        [Description("Unknown")] UNKNOWN
    }

    public enum CreditCardPayroll
    {
        [Description("Yes")] YES,
        [Description("No")] NO,
        [Description("Unknown")] UNKNOWN
    }

    public enum CreditCardDebit
    {
        [Description("Yes")] YES,
        [Description("No")] NO,
        [Description("Unknown")] UNKNOWN
    }

    public enum CreditCardCommercial
    {
        [Description("Yes")] YES,
        [Description("No")] NO,
        [Description("Unknown")] UNKNOWN
    }

    public enum CreditCardHealthcare
    {
        [Description("Yes")] YES,
        [Description("No")] NO,
        [Description("Unknown")] UNKNOWN
    }

    public enum CreditCardDurbinRegulated
    {
        [Description("Yes")] YES,
        [Description("No")] NO,
        [Description("Unknown")] UNKNOWN
    }

    public enum CreditCardCardType
    {
        [Description("American Express")] AMEX,
        [Description("Carte Blanche")] CARTE_BLANCHE,
        [Description("China UnionPay")] CHINA_UNION_PAY,
        [Description("UnionPay")] UNION_PAY,
        [Description("Diners Club")] DINERS_CLUB_INTERNATIONAL,
        [Description("Discover")] DISCOVER,
        [Description("Elo")] ELO,
        [Description("JCB")] JCB,
        [Description("Laser")] LASER,
        [Description("UK Maestro")] UK_MAESTRO,
        [Description("Maestro")] MAESTRO,
        [Description("MasterCard")] MASTER_CARD,
        [Description("Solo")] SOLO,
        [Description("Switch")] SWITCH,
        [Description("Visa")] VISA,
        [Description("Unknown")] UNKNOWN,
        [Description("Unrecognized")] UNRECOGNIZED
    }

    /// <summary>
    /// A credit card returned by the Braintree Gateway
    /// </summary>
    /// <remarks>
    /// A credit card can belong to:
    /// <ul>
    ///   <li>a <see cref="Customer"/> as a stored credit card</li>
    ///   <li>a <see cref="Transaction"/> as the credit card used for the transaction</li>
    /// </ul>
    /// </remarks>
    /// <example>
    /// Credit Cards can be retrieved via the gateway using the associated credit card token:
    /// <code>
    ///     CreditCard creditCard = gateway.CreditCard.Find("token");
    /// </code>
    /// For more information about Credit Cards, see <a href="https://developers.braintreepayments.com/reference/response/credit-card/dotnet" target="_blank">https://developers.braintreepayments.com/reference/response/credit-card/dotnet</a><br />
    /// For more information about Credit Card Verifications, see <a href="https://developers.braintreepayments.com/reference/response/credit-card-verification/dotnet" target="_blank">https://developers.braintreepayments.com/reference/response/credit-card-verification/dotnet</a>
    /// </example>
    public class CreditCard : PaymentMethod
    {
        public static readonly string CountryOfIssuanceUnknown = "Unknown";
        public static readonly string IssuingBankUnknown = "Unknown";
        public static readonly string ProductIdUnknown = "Unknown";

        public virtual string Bin { get; protected set; }
        public virtual string CardholderName { get; protected set; }
        public virtual CreditCardCardType CardType { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual string CustomerId { get; protected set; }
        public virtual bool? IsDefault { get; protected set; }
        public virtual bool? IsVenmoSdk { get; protected set; }
        public virtual bool? IsExpired { get; protected set; }
        public virtual bool? IsNetworkTokenized { get; protected set; }
        public virtual CreditCardCustomerLocation CustomerLocation { get; protected set; }
        public virtual string LastFour { get; protected set; }
        public virtual string UniqueNumberIdentifier { get; protected set; }
        public virtual Subscription[] Subscriptions { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual Address BillingAddress { get; protected set; }
        public virtual string ExpirationMonth { get; protected set; }
        public virtual string ExpirationYear { get; protected set; }
        public virtual CreditCardPrepaid Prepaid { get; protected set; }
        public virtual CreditCardPayroll Payroll { get; protected set; }
        public virtual CreditCardDebit Debit { get; protected set; }
        public virtual CreditCardCommercial Commercial { get; protected set; }
        public virtual CreditCardHealthcare Healthcare { get; protected set; }
        public virtual CreditCardDurbinRegulated DurbinRegulated { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual CreditCardVerification Verification { get; protected set; }
        public virtual string AccountType { get; protected set; }

        private string _CountryOfIssuance;

        public virtual string CountryOfIssuance
        {
            get
            {
                if (_CountryOfIssuance == "")
                {
                    return CountryOfIssuanceUnknown;
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
                    return IssuingBankUnknown;
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
                    return ProductIdUnknown;
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

        protected internal CreditCard(NodeWrapper node, IBraintreeGateway gateway)
        {
            if (node == null) return;

            Bin = node.GetString("bin");
            CardholderName = node.GetString("cardholder-name");
            CardType = node.GetEnum("card-type", CreditCardCardType.UNRECOGNIZED);
            CustomerId = node.GetString("customer-id");
            IsDefault = node.GetBoolean("default");
            IsVenmoSdk = node.GetBoolean("venmo-sdk");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            IsExpired = node.GetBoolean("expired");
            IsNetworkTokenized = node.GetBoolean("is-network-tokenized");
            CustomerLocation = node.GetEnum("customer-location", CreditCardCustomerLocation.UNRECOGNIZED);
            LastFour = node.GetString("last-4");
            UniqueNumberIdentifier = node.GetString("unique-number-identifier");
            Token = node.GetString("token");
            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");
            BillingAddress = new Address(node.GetNode("billing-address"));
            Prepaid = node.GetEnum("prepaid", CreditCardPrepaid.UNKNOWN);
            Payroll = node.GetEnum("payroll", CreditCardPayroll.UNKNOWN);
            DurbinRegulated = node.GetEnum("durbin-regulated", CreditCardDurbinRegulated.UNKNOWN);
            Debit = node.GetEnum("debit", CreditCardDebit.UNKNOWN);
            Commercial = node.GetEnum("commercial", CreditCardCommercial.UNKNOWN);
            Healthcare = node.GetEnum("healthcare", CreditCardHealthcare.UNKNOWN);
            AccountType = node.GetString("account-type");
            _CountryOfIssuance = node.GetString("country-of-issuance");
            _IssuingBank = node.GetString("issuing-bank");
            _ProductId = node.GetString("product-id");
            ImageUrl = node.GetString("image-url");

            var subscriptionXmlNodes = node.GetList("subscriptions/subscription");
            Subscriptions = new Subscription[subscriptionXmlNodes.Count];
            for (int i = 0; i < subscriptionXmlNodes.Count; i++)
            {
                Subscriptions[i] = new Subscription(subscriptionXmlNodes[i], gateway);
            }

            var verificationNodes = node.GetList("verifications/verification");
            Verification = FindLatestVerification(verificationNodes, gateway);
        }

        [Obsolete("Mock Use Only")]
        protected internal CreditCard() { }

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
