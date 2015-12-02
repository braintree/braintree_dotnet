#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Braintree
{
    /// <summary>
    /// A customer returned by the Braintree Gateway
    /// </summary>
    /// <example>
    /// Customers can be retrieved via the gateway using the associated customer id:
    /// <code>
    ///     Customer customer = gateway.Customer.Find("customerId");
    /// </code>
    /// For more information about Customers, see <a href="http://www.braintreepayments.com/gateway/customer-api" target="_blank">http://www.braintreepaymentsolutions.com/gateway/customer-api</a>
    /// </example>
    public class Customer
    {
        public virtual string Id { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string Company { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string Phone { get; protected set; }
        public virtual string Fax { get; protected set; }
        public virtual string Website { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual CreditCard[] CreditCards { get; protected set; }
        public virtual PayPalAccount[] PayPalAccounts { get; protected set; }
        public virtual ApplePayCard[] ApplePayCards { get; protected set; }
        public virtual AndroidPayCard[] AndroidPayCards { get; protected set; }
        public virtual AmexExpressCheckoutCard[] AmexExpressCheckoutCards { get; protected set; }
        public virtual CoinbaseAccount[] CoinbaseAccounts { get; protected set; }
        public virtual VenmoAccount[] VenmoAccounts { get; protected set; }
        public virtual PaymentMethod[] PaymentMethods { get; protected set; }
        public virtual Address[] Addresses { get; protected set; }
        public virtual Dictionary<string, string> CustomFields { get; protected set; }
        public PaymentMethod DefaultPaymentMethod
        {
            get
            {
                foreach (PaymentMethod paymentMethod in PaymentMethods)
                {
                    if (paymentMethod.IsDefault.Value)
                    {
                        return paymentMethod;
                    }
                }
                return null;
            }
        }

        protected internal Customer(NodeWrapper node, IBraintreeGateway gateway)
        {
            if (node == null) return;

            Id = node.GetString("id");
            FirstName = node.GetString("first-name");
            LastName = node.GetString("last-name");
            Company = node.GetString("company");
            Email = node.GetString("email");
            Phone = node.GetString("phone");
            Fax = node.GetString("fax");
            Website = node.GetString("website");
            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");

            var creditCardXmlNodes = node.GetList("credit-cards/credit-card");
            CreditCards = new CreditCard[creditCardXmlNodes.Count];
            for (int i = 0; i < creditCardXmlNodes.Count; i++)
            {
                CreditCards[i] = new CreditCard(creditCardXmlNodes[i], gateway);
            }

            var paypalXmlNodes = node.GetList("paypal-accounts/paypal-account");
            PayPalAccounts = new PayPalAccount[paypalXmlNodes.Count];
            for (int i = 0; i < paypalXmlNodes.Count; i++)
            {
                PayPalAccounts[i] = new PayPalAccount(paypalXmlNodes[i], gateway);
            }

            var applePayXmlNodes = node.GetList("apple-pay-cards/apple-pay-card");
            ApplePayCards = new ApplePayCard[applePayXmlNodes.Count];
            for (int i = 0; i < applePayXmlNodes.Count; i++)
            {
                ApplePayCards[i] = new ApplePayCard(applePayXmlNodes[i], gateway);
            }

            var androidPayCardXmlNodes = node.GetList("android-pay-cards/android-pay-card");
            AndroidPayCards = new AndroidPayCard[androidPayCardXmlNodes.Count];
            for (int i = 0; i < androidPayCardXmlNodes.Count; i++)
            {
                AndroidPayCards[i] = new AndroidPayCard(androidPayCardXmlNodes[i], gateway);
            }

            var amexExpressCheckoutCardXmlNodes = node.GetList("amex-express-checkout-cards/amex-express-checkout-card");
            AmexExpressCheckoutCards = new AmexExpressCheckoutCard[amexExpressCheckoutCardXmlNodes.Count];
            for (int i = 0; i < amexExpressCheckoutCardXmlNodes.Count; i++)
            {
                AmexExpressCheckoutCards[i] = new AmexExpressCheckoutCard(amexExpressCheckoutCardXmlNodes[i], gateway);
            }

            var coinbaseXmlNodes = node.GetList("coinbase-accounts/coinbase-account");
            CoinbaseAccounts = new CoinbaseAccount[coinbaseXmlNodes.Count];
            for (int i = 0; i < coinbaseXmlNodes.Count; i++)
            {
                CoinbaseAccounts[i] = new CoinbaseAccount(coinbaseXmlNodes[i], gateway);
            }

            var venmoAccountXmlNodes = node.GetList("venmo-accounts/venmo-account");
            VenmoAccounts = new VenmoAccount[venmoAccountXmlNodes.Count];
            for (int i = 0; i < venmoAccountXmlNodes.Count; i++)
            {
                VenmoAccounts[i] = new VenmoAccount(venmoAccountXmlNodes[i], gateway);
            }

            PaymentMethods = new PaymentMethod[
                CreditCards.Length
                + PayPalAccounts.Length
                + ApplePayCards.Length
                + CoinbaseAccounts.Length
                + AndroidPayCards.Length
                + AmexExpressCheckoutCards.Length
                + VenmoAccounts.Length
            ];

            CreditCards.CopyTo(PaymentMethods, 0);
            PayPalAccounts.CopyTo(PaymentMethods, CreditCards.Length);
            ApplePayCards.CopyTo(PaymentMethods, CreditCards.Length + PayPalAccounts.Length);
            CoinbaseAccounts.CopyTo(PaymentMethods, CreditCards.Length + PayPalAccounts.Length + ApplePayCards.Length);
            AndroidPayCards.CopyTo(PaymentMethods, CreditCards.Length + PayPalAccounts.Length + ApplePayCards.Length + CoinbaseAccounts.Length);
            AmexExpressCheckoutCards.CopyTo(PaymentMethods, CreditCards.Length + PayPalAccounts.Length + ApplePayCards.Length + CoinbaseAccounts.Length + AndroidPayCards.Length);
            VenmoAccounts.CopyTo(PaymentMethods, CreditCards.Length + PayPalAccounts.Length + ApplePayCards.Length + CoinbaseAccounts.Length + AndroidPayCards.Length + AmexExpressCheckoutCards.Length);

            var addressXmlNodes = node.GetList("addresses/address");
            Addresses = new Address[addressXmlNodes.Count];
            for (int i = 0; i < addressXmlNodes.Count; i++)
            {
                Addresses[i] = new Address(addressXmlNodes[i]);
            }

            CustomFields = node.GetDictionary("custom-fields");
        }

        [Obsolete("Mock Use Only")]
        protected internal Customer() { }
    }
}
