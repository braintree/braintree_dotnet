#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    /// <summary>
    /// A class for building requests to manipulate <see cref="Address"/> records in the vault.
    /// </summary>
    /// <example>
    /// An address request can be constructed as follows:
    /// <code>
    ///  var addressRequest = new AddressRequest
    ///  {
    ///      FirstName = "Michael",
    ///      LastName = "Angelo",
    ///      Company = "Angelo Co.",
    ///      StreetAddress = "1 E Main St",
    ///      ExtendedAddress = "Apt 3",
    ///      Locality = "Chicago",
    ///      Region = "IL",
    ///      PostalCode = "60622",
    ///      CountryName = "United States of America"
    /// };
    /// </code>
    /// </example>
    public class AddressRequest : Request
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string StreetAddress { get; set; }
        public string ExtendedAddress { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string CountryCodeAlpha2 { get; set; }
        public string CountryCodeAlpha3 { get; set; }
        public string CountryCodeNumeric { get; set; }
        public string CountryName { get; set; }

        public override string ToXml()
        {
            return ToXml("address");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("address");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("first-name", FirstName).
                AddElement("last-name", LastName).
                AddElement("company", Company).
                AddElement("street-address", StreetAddress).
                AddElement("extended-address", ExtendedAddress).
                AddElement("locality", Locality).
                AddElement("region", Region).
                AddElement("postal-code", PostalCode).
                AddElement("country-code-alpha2", CountryCodeAlpha2).
                AddElement("country-code-alpha3", CountryCodeAlpha3).
                AddElement("country-code-numeric", CountryCodeNumeric).
                AddElement("country-name", CountryName);
        }
    }
}
