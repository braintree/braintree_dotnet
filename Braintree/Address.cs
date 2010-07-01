#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
    /// <summary>
    /// An address returned by the Braintree Gateway
    /// </summary>
    /// <remarks>
    /// An address can belong to:
    /// <ul>
    ///   <li>a <see cref="CreditCard"/> as the billing address</li>
    ///   <li>a <see cref="Customer"/> as an address</li>
    ///   <li>a <see cref="Transaction"/> as a billing or shipping address</li>
    /// </ul>
    /// </remarks>
    /// <example>
    /// Addresses can be retrieved via the gateway using the associated customer Id and address Id:
    /// <code>
    ///     Address address = gateway.Address.Find("customerId", "addressId");
    /// </code>
    /// </example>
    public class Address
    {
        public String Id { get; protected set; }
        public String CustomerId { get; protected set; }
        public String FirstName { get; protected set; }
        public String LastName { get; protected set; }
        public String Company { get; protected set; }
        public String StreetAddress { get; protected set; }
        public String ExtendedAddress { get; protected set; }
        public String Locality { get; protected set; }
        public String Region { get; protected set; }
        public String PostalCode { get; protected set; }
        public String CountryCodeAlpha2 { get; protected set; }
        public String CountryCodeAlpha3 { get; protected set; }
        public String CountryCodeNumeric { get; protected set; }
        public String CountryName { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        internal Address(NodeWrapper node)
        {
            if (node == null) return;

            Id = node.GetString("id");
            CustomerId = node.GetString("customer-id");
            FirstName = node.GetString("first-name");
            LastName = node.GetString("last-name");
            Company = node.GetString("company");
            StreetAddress = node.GetString("street-address");
            ExtendedAddress = node.GetString("extended-address");
            Locality = node.GetString("locality");
            Region = node.GetString("region");
            PostalCode = node.GetString("postal-code");
            CountryCodeAlpha2 = node.GetString("country-code-alpha2");
            CountryCodeAlpha3 = node.GetString("country-code-alpha3");
            CountryCodeNumeric = node.GetString("country-code-numeric");
            CountryName = node.GetString("country-name");
            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");
        }
    }
}
