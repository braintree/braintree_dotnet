#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Security.Cryptography;

namespace Braintree
{
    public class CreditCardCustomerLocation : Enumeration
    {
        public static readonly CreditCardCustomerLocation US = new CreditCardCustomerLocation("us");
        public static readonly CreditCardCustomerLocation INTERNATIONAL = new CreditCardCustomerLocation("international");
        public static readonly CreditCardCustomerLocation UNRECOGNIZED = new CreditCardCustomerLocation("unrecognized");

        public static readonly CreditCardCustomerLocation[] ALL = {US, INTERNATIONAL};

        protected CreditCardCustomerLocation(String name) : base(name) {}
    }

    public class CreditCardPrepaid : Enumeration
    {
        public static readonly CreditCardPrepaid YES = new CreditCardPrepaid("Yes");
        public static readonly CreditCardPrepaid NO = new CreditCardPrepaid("No");
        public static readonly CreditCardPrepaid UNKNOWN = new CreditCardPrepaid("Unknown");

        public static readonly CreditCardPrepaid[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardPrepaid(String name) : base(name) {}
    }

    public class CreditCardPayroll : Enumeration
    {
        public static readonly CreditCardPayroll YES = new CreditCardPayroll("Yes");
        public static readonly CreditCardPayroll NO = new CreditCardPayroll("No");
        public static readonly CreditCardPayroll UNKNOWN = new CreditCardPayroll("Unknown");

        public static readonly CreditCardPayroll[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardPayroll(String name) : base(name) {}
    }

    public class CreditCardDebit : Enumeration
    {
        public static readonly CreditCardDebit YES = new CreditCardDebit("Yes");
        public static readonly CreditCardDebit NO = new CreditCardDebit("No");
        public static readonly CreditCardDebit UNKNOWN = new CreditCardDebit("Unknown");

        public static readonly CreditCardDebit[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardDebit(String name) : base(name) {}
    }

    public class CreditCardCommercial : Enumeration
    {
        public static readonly CreditCardCommercial YES = new CreditCardCommercial("Yes");
        public static readonly CreditCardCommercial NO = new CreditCardCommercial("No");
        public static readonly CreditCardCommercial UNKNOWN = new CreditCardCommercial("Unknown");

        public static readonly CreditCardCommercial[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardCommercial(String name) : base(name) {}
    }

    public class CreditCardHealthcare : Enumeration
    {
        public static readonly CreditCardHealthcare YES = new CreditCardHealthcare("Yes");
        public static readonly CreditCardHealthcare NO = new CreditCardHealthcare("No");
        public static readonly CreditCardHealthcare UNKNOWN = new CreditCardHealthcare("Unknown");

        public static readonly CreditCardHealthcare[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardHealthcare(String name) : base(name) {}
    }

    public class CreditCardDurbinRegulated : Enumeration
    {
        public static readonly CreditCardDurbinRegulated YES = new CreditCardDurbinRegulated("Yes");
        public static readonly CreditCardDurbinRegulated NO = new CreditCardDurbinRegulated("No");
        public static readonly CreditCardDurbinRegulated UNKNOWN = new CreditCardDurbinRegulated("Unknown");

        public static readonly CreditCardDurbinRegulated[] ALL = {YES, NO, UNKNOWN};

        protected CreditCardDurbinRegulated(String name) : base(name) {}
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
        public static readonly CreditCardCardType MAESTRO = new CreditCardCardType("Maestro");
        public static readonly CreditCardCardType MASTER_CARD = new CreditCardCardType("MasterCard");
        public static readonly CreditCardCardType SOLO = new CreditCardCardType("Solo");
        public static readonly CreditCardCardType SWITCH = new CreditCardCardType("Switch");
        public static readonly CreditCardCardType VISA = new CreditCardCardType("Visa");
        public static readonly CreditCardCardType UNKNOWN = new CreditCardCardType("Unknown");
        public static readonly CreditCardCardType UNRECOGNIZED = new CreditCardCardType("Unrecognized");

        public static readonly CreditCardCardType[] ALL = {
            AMEX, CARTE_BLANCHE, CHINA_UNION_PAY, DINERS_CLUB_INTERNATIONAL, DISCOVER,
            JCB, LASER, MAESTRO, MASTER_CARD, SOLO, SWITCH, VISA, UNKNOWN
        };

        protected CreditCardCardType(String name) : base(name) {}
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
    public class CreditCard
    {
        public static readonly String CountryOfIssuanceUnknown = "Unknown";
        public static readonly String IssuingBankUnknown = "Unknown";

        public String Bin { get; protected set; }
        public String CardholderName { get; protected set; }
        public CreditCardCardType CardType { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public String CustomerId { get; protected set; }
        public Boolean? IsDefault { get; protected set; }
        public Boolean? IsVenmoSdk { get; protected set; }
        public Boolean? IsExpired { get; protected set; }
        public CreditCardCustomerLocation CustomerLocation { get; protected set; }
        public String LastFour { get; protected set; }
        public String UniqueNumberIdentifier { get; protected set; }
        public Subscription[] Subscriptions { get; protected set; }
        public String Token { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public Address BillingAddress { get; protected set; }
        public String ExpirationMonth { get; protected set; }
        public String ExpirationYear { get; protected set; }
        public CreditCardPrepaid Prepaid { get; protected set; }
        public CreditCardPayroll Payroll { get; protected set; }
        public CreditCardDebit Debit { get; protected set; }
        public CreditCardCommercial Commercial { get; protected set; }
        public CreditCardHealthcare Healthcare { get; protected set; }
        public CreditCardDurbinRegulated DurbinRegulated { get; protected set; }
        public String ImageUrl { get; protected set; }

        private String _CountryOfIssuance;

        public String CountryOfIssuance
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

        private String _IssuingBank;

        public String IssuingBank
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

        public String ExpirationDate
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

        public String MaskedNumber
        {
            get
            {
                return String.Format("{0}******{1}", Bin, LastFour);
            }
        }

        protected internal CreditCard(NodeWrapper node, BraintreeService service)
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
            ImageUrl = node.GetString("image-url");

            var subscriptionXmlNodes = node.GetList("subscriptions/subscription");
            Subscriptions = new Subscription[subscriptionXmlNodes.Count];
            for (int i = 0; i < subscriptionXmlNodes.Count; i++)
            {
                Subscriptions[i] = new Subscription(subscriptionXmlNodes[i], service);
            }
        }
    }
}
