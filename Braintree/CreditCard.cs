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
        public String Bin { get; protected set; }
        public String CardholderName { get; protected set; }
        public CreditCardCardType CardType { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public String CustomerId { get; protected set; }
        public Boolean? IsDefault { get; protected set; }
        public Boolean? IsExpired { get; protected set; }
        public CreditCardCustomerLocation CustomerLocation { get; protected set; }
        public String LastFour { get; protected set; }
        public String NumberUniqueIdentifier { get; protected set; }
        public Subscription[] Subscriptions { get; protected set; }
        public String Token { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public Address BillingAddress { get; protected set; }
        public String ExpirationMonth { get; protected set; }
        public String ExpirationYear { get; protected set; }
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
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            IsExpired = node.GetBoolean("expired");
            CustomerLocation = (CreditCardCustomerLocation)CollectionUtil.Find(CreditCardCustomerLocation.ALL, node.GetString("customer-location"), CreditCardCustomerLocation.UNRECOGNIZED);
            LastFour = node.GetString("last-4");
            NumberUniqueIdentifier = node.GetString("number-unique-identifier");
            Token = node.GetString("token");
            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");
            BillingAddress = new Address(node.GetNode("billing-address"));

            var subscriptionXmlNodes = node.GetList("subscriptions/subscription");
            Subscriptions = new Subscription[subscriptionXmlNodes.Count];
            for (int i = 0; i < subscriptionXmlNodes.Count; i++)
            {
                Subscriptions[i] = new Subscription(subscriptionXmlNodes[i], service);
            }
        }
    }
}
