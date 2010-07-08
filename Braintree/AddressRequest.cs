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
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Company { get; set; }
        public String StreetAddress { get; set; }
        public String ExtendedAddress { get; set; }
        public String Locality { get; set; }
        public String Region { get; set; }
        public String PostalCode { get; set; }
        public String CountryCodeAlpha2 { get; set; }
        public String CountryCodeAlpha3 { get; set; }
        public String CountryCodeNumeric { get; set; }
        public String CountryName { get; set; }

        public override String ToXml()
        {
            return ToXml("address");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString()
        {
            return ToQueryString("address");
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
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
