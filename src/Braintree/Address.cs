#pragma warning disable 1591

using System;

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
        public virtual string Company { get; protected set; }
        public virtual string CountryCodeAlpha2 { get; protected set; }
        public virtual string CountryCodeAlpha3 { get; protected set; }
        public virtual string CountryCodeNumeric { get; protected set; }
        public virtual string CountryName { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual string CustomerId { get; protected set; }
        public virtual string ExtendedAddress { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual InternationalPhone InternationalPhone { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string Locality { get; protected set; }
        public virtual string PhoneNumber { get; protected set; }
        public virtual string PostalCode { get; protected set; }
        public virtual string Region { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual string StreetAddress { get; protected set; }

        protected internal Address(NodeWrapper node)
        {
            if (node == null) return;

            Company = node.GetString("company");
            CountryCodeAlpha2 = node.GetString("country-code-alpha2");
            CountryCodeAlpha3 = node.GetString("country-code-alpha3");
            CountryCodeNumeric = node.GetString("country-code-numeric");
            CountryName = node.GetString("country-name");
            CreatedAt = node.GetDateTime("created-at");
            CustomerId = node.GetString("customer-id");
            ExtendedAddress = node.GetString("extended-address");
            FirstName = node.GetString("first-name");
            Id = node.GetString("id");
            var internationalPhoneNode = node.GetNode("international-phone");
            if (internationalPhoneNode != null)
            {
                InternationalPhone = new InternationalPhone(internationalPhoneNode);
            }
            LastName = node.GetString("last-name");
            Locality = node.GetString("locality");
            PhoneNumber = node.GetString("phone-number");
            PostalCode = node.GetString("postal-code");
            Region = node.GetString("region");
            StreetAddress = node.GetString("street-address");
            UpdatedAt = node.GetDateTime("updated-at");
        }

        [Obsolete("Mock Use Only")]
        protected Address() { }
    }
}
