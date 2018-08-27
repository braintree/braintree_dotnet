#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace Braintree
{
    public class CreditCardCustomerLocation : Enumeration
    {
        public static readonly CreditCardCustomerLocation US = new CreditCardCustomerLocation("us");
        public static readonly CreditCardCustomerLocation INTERNATIONAL = new CreditCardCustomerLocation("international");
        public static readonly CreditCardCustomerLocation UNRECOGNIZED = new CreditCardCustomerLocation("unrecognized");

        public static readonly CreditCardCustomerLocation[] ALL = {US, INTERNATIONAL};

        protected CreditCardCustomerLocation(string name) : base(name) {}
    }

    public class CreditCardPrepaid : Enumeration
    {
        public static readonly CreditCardPrepaid YES = new CreditCardPrepaid("Yes");
        public static readonly CreditCardPrepaid NO = new CreditCardPrepaid("No");
        public static readonly CreditCardPrepaid UNKNOWN = new CreditCardPrepaid("Unknown");

        public static readonly CreditCardPrepaid[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardPrepaid(string name) : base(name) {}
    }

    public class CreditCardPayroll : Enumeration
    {
        public static readonly CreditCardPayroll YES = new CreditCardPayroll("Yes");
        public static readonly CreditCardPayroll NO = new CreditCardPayroll("No");
        public static readonly CreditCardPayroll UNKNOWN = new CreditCardPayroll("Unknown");

        public static readonly CreditCardPayroll[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardPayroll(string name) : base(name) {}
    }

    public class CreditCardDebit : Enumeration
    {
        public static readonly CreditCardDebit YES = new CreditCardDebit("Yes");
        public static readonly CreditCardDebit NO = new CreditCardDebit("No");
        public static readonly CreditCardDebit UNKNOWN = new CreditCardDebit("Unknown");

        public static readonly CreditCardDebit[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardDebit(string name) : base(name) {}
    }

    public class CreditCardCommercial : Enumeration
    {
        public static readonly CreditCardCommercial YES = new CreditCardCommercial("Yes");
        public static readonly CreditCardCommercial NO = new CreditCardCommercial("No");
        public static readonly CreditCardCommercial UNKNOWN = new CreditCardCommercial("Unknown");

        public static readonly CreditCardCommercial[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardCommercial(string name) : base(name) {}
    }

    public class CreditCardHealthcare : Enumeration
    {
        public static readonly CreditCardHealthcare YES = new CreditCardHealthcare("Yes");
        public static readonly CreditCardHealthcare NO = new CreditCardHealthcare("No");
        public static readonly CreditCardHealthcare UNKNOWN = new CreditCardHealthcare("Unknown");

        public static readonly CreditCardHealthcare[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardHealthcare(string name) : base(name) {}
    }

    public class CreditCardDurbinRegulated : Enumeration
    {
        public static readonly CreditCardDurbinRegulated YES = new CreditCardDurbinRegulated("Yes");
        public static readonly CreditCardDurbinRegulated NO = new CreditCardDurbinRegulated("No");
        public static readonly CreditCardDurbinRegulated UNKNOWN = new CreditCardDurbinRegulated("Unknown");

        public static readonly CreditCardDurbinRegulated[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardDurbinRegulated(string name) : base(name) {}
    }

    public class CreditCardCardType : Enumeration
    {
        public static readonly CreditCardCardType AMEX = new CreditCardCardType("American Express");
        public static readonly CreditCardCardType CARTE_BLANCHE = new CreditCardCardType("Carte Blanche");
        public static readonly CreditCardCardType CHINA_UNION_PAY = new CreditCardCardType("China UnionPay");
        public static readonly CreditCardCardType DINERS_CLUB_INTERNATIONAL = new CreditCardCardType("Diners Club");
        public static readonly CreditCardCardType DISCOVER = new CreditCardCardType("Discover");
        public static readonly CreditCardCardType JCB = new CreditCardCardType("JCB");
        public static readonly CreditCardCardType LASER = new CreditCardCardType("Laser");
        public static readonly CreditCardCardType UK_MAESTRO = new CreditCardCardType("UK Maestro");
        public static readonly CreditCardCardType MAESTRO = new CreditCardCardType("Maestro");
        public static readonly CreditCardCardType MASTER_CARD = new CreditCardCardType("MasterCard");
        public static readonly CreditCardCardType SOLO = new CreditCardCardType("Solo");
        public static readonly CreditCardCardType SWITCH = new CreditCardCardType("Switch");
        public static readonly CreditCardCardType VISA = new CreditCardCardType("Visa");
        public static readonly CreditCardCardType UNKNOWN = new CreditCardCardType("Unknown");
        public static readonly CreditCardCardType UNRECOGNIZED = new CreditCardCardType("Unrecognized");

        public static readonly CreditCardCardType[] ALL = {
            AMEX, CARTE_BLANCHE, CHINA_UNION_PAY, DINERS_CLUB_INTERNATIONAL, DISCOVER,
            JCB, LASER, UK_MAESTRO, MAESTRO, MASTER_CARD, SOLO, SWITCH, VISA, UNKNOWN
        };

        protected CreditCardCardType(string name) : base(name) {}
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
    /// For more information about Credit Cards, see <a href="http://www.braintreepayments.com/gateway/credit-card-api" target="_blank">http://www.braintreepaymentsolutions.com/gateway/credit-card-api</a><br />
    /// For more information about Credit Card Verifications, see <a href="http://www.braintreepayments.com/gateway/credit-card-verification-api" target="_blank">http://www.braintreepaymentsolutions.com/gateway/credit-card-verification-api</a>
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
            CardType = (CreditCardCardType)CollectionUtil.Find(CreditCardCardType.ALL, node.GetString("card-type"), CreditCardCardType.UNRECOGNIZED);
            CustomerId = node.GetString("customer-id");
            IsDefault = node.GetBoolean("default");
            IsVenmoSdk = node.GetBoolean("venmo-sdk");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            IsExpired = node.GetBoolean("expired");
            CustomerLocation = (CreditCardCustomerLocation)CollectionUtil.Find(CreditCardCustomerLocation.ALL, node.GetString("customer-location"), CreditCardCustomerLocation.UNRECOGNIZED);
            LastFour = node.GetString("last-4");
            UniqueNumberIdentifier = node.GetString("unique-number-identifier");
            Token = node.GetString("token");
            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");
            BillingAddress = new Address(node.GetNode("billing-address"));
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
