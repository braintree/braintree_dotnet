#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Security.Cryptography;

namespace Braintree
{
    public abstract class CreditCardCustomerLocation
    {
        public const String US = "us";
        public const String INTERNATIONAL = "international";

        public static readonly String[] ALL = {US, INTERNATIONAL};
    }

    public abstract class CreditCardCardType
    {
        public const String AMEX = "American Express";
        public const String CARTE_BLANCHE = "Carte Blanche";
        public const String CHINA_UNION_PAY = "China UnionPay";
        public const String DINERS_CLUB_INTERNATIONAL = "Diners Club";
        public const String DISCOVER = "Discover";
        public const String JCB = "JCB";
        public const String LASER = "Laser";
        public const String MAESTRO = "Maestro";
        public const String MASTER_CARD = "MasterCard";
        public const String SOLO = "Solo";
        public const String SWITCH = "Switch";
        public const String VISA = "Visa";
        public const String UNKNOWN = "Unknown";

        public static readonly String[] ALL = {
            AMEX, CARTE_BLANCHE, CHINA_UNION_PAY, DINERS_CLUB_INTERNATIONAL, DISCOVER,
            JCB, LASER, MAESTRO, MASTER_CARD, SOLO, SWITCH, VISA, UNKNOWN
        };
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
    /// For more information about Credit Cards, see <a href="http://www.braintreepaymentsolutions.com/gateway/credit-card-api" target="_blank">http://www.braintreepaymentsolutions.com/gateway/credit-card-api</a><br />
    /// For more information about Credit Card Verifications, see <a href="http://www.braintreepaymentsolutions.com/gateway/credit-card-verification-api" target="_blank">http://www.braintreepaymentsolutions.com/gateway/credit-card-verification-api</a>
    /// </example>
    public class CreditCard
    {
        public String Bin { get; protected set; }
        public String CardholderName { get; protected set; }
        public String CardType { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public String CustomerId { get; protected set; }
        public Boolean? IsDefault { get; protected set; }
        public Boolean? IsExpired { get; protected set; }
        public String CustomerLocation { get; protected set; }
        public String LastFour { get; protected set; }
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

        internal CreditCard(NodeWrapper node, BraintreeService service)
        {
            if (node == null) return;

            Bin = node.GetString("bin");
            CardholderName = node.GetString("cardholder-name");
            CardType = node.GetString("card-type");
            CustomerId = node.GetString("customer-id");
            IsDefault = node.GetBoolean("default");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            IsExpired = node.GetBoolean("expired");
            CustomerLocation = node.GetString("customer-location");
            LastFour = node.GetString("last-4");
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
