#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Security.Cryptography;

namespace Braintree
{
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
    /// </example>
    public class CreditCard
    {
        public String Bin { get; protected set; }
        public String CardholderName { get; protected set; }
        public String CardType { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public String CustomerId { get; protected set; }
        public Boolean? Default { get; protected set; }
        public String CustomerLocation { get; protected set; }
        public String LastFour { get; protected set; }
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

        internal CreditCard(NodeWrapper node)
        {
            if (node == null) return;

            Bin = node.GetString("bin");
            CardholderName = node.GetString("cardholder-name");
            CardType = node.GetString("card-type");
            CustomerId = node.GetString("customer-id");
            Default = node.GetBoolean("default");
            ExpirationMonth = node.GetString("expiration-month");
            ExpirationYear = node.GetString("expiration-year");
            CustomerLocation = node.GetString("customer-location");
            LastFour = node.GetString("last-4");
            Token = node.GetString("token");
            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");
            BillingAddress = new Address(node.GetNode("billing-address"));
        }
    }
}
