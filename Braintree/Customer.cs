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
        public String Id { get; protected set; }
        public String FirstName { get; protected set; }
        public String LastName { get; protected set; }
        public String Company { get; protected set; }
        public String Email { get; protected set; }
        public String Phone { get; protected set; }
        public String Fax { get; protected set; }
        public String Website { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public CreditCard[] CreditCards { get; protected set; }
        public PayPalAccount[] PayPalAccounts { get; protected set; }
        public ApplePayCard[] ApplePayCards { get; protected set; }
        public CoinbaseAccount[] CoinbaseAccounts { get; protected set; }
        public PaymentMethod[] PaymentMethods { get; protected set; }
        public Address[] Addresses { get; protected set; }
        public Dictionary<String, String> CustomFields { get; protected set; }
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

        protected internal Customer(NodeWrapper node, BraintreeService service)
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
                CreditCards[i] = new CreditCard(creditCardXmlNodes[i], service);
            }

            var paypalXmlNodes = node.GetList("paypal-accounts/paypal-account");
            PayPalAccounts = new PayPalAccount[paypalXmlNodes.Count];
            for (int i = 0; i < paypalXmlNodes.Count; i++)
            {
                PayPalAccounts[i] = new PayPalAccount(paypalXmlNodes[i], service);
            }

            var applePayXmlNodes = node.GetList("apple-pay-cards/apple-pay-card");
            ApplePayCards = new ApplePayCard[applePayXmlNodes.Count];
            for (int i = 0; i < applePayXmlNodes.Count; i++)
            {
                ApplePayCards[i] = new ApplePayCard(applePayXmlNodes[i], service);
            }

            var coinbaseXmlNodes = node.GetList("coinbase-accounts/coinbase-account");
            CoinbaseAccounts = new CoinbaseAccount[coinbaseXmlNodes.Count];
            for (int i = 0; i < coinbaseXmlNodes.Count; i++)
            {
                CoinbaseAccounts[i] = new CoinbaseAccount(coinbaseXmlNodes[i], service);
            }

            PaymentMethods = new PaymentMethod[CreditCards.Length + PayPalAccounts.Length + ApplePayCards.Length + CoinbaseAccounts.Length];
            CreditCards.CopyTo(PaymentMethods, 0);
            PayPalAccounts.CopyTo(PaymentMethods, CreditCards.Length);
            ApplePayCards.CopyTo(PaymentMethods, CreditCards.Length + PayPalAccounts.Length);
            CoinbaseAccounts.CopyTo(PaymentMethods, CreditCards.Length + PayPalAccounts.Length + ApplePayCards.Length);

            var addressXmlNodes = node.GetList("addresses/address");
            Addresses = new Address[addressXmlNodes.Count];
            for (int i = 0; i < addressXmlNodes.Count; i++)
            {
                Addresses[i] = new Address(addressXmlNodes[i]);
            }

            CustomFields = node.GetDictionary("custom-fields");
        }
    }
}
