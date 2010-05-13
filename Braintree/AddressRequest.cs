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
    ///      CustomerId = customer.Id,
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
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string StreetAddress { get; set; }
        public string ExtendedAddress { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string CountryName { get; set; }


        public override String ToXml()
        {
            return Build(new XmlRequestBuilder("address")).ToString();
        }

        public override String ToXml(String rootElement)
        {
            return Build(new XmlRequestBuilder(rootElement)).ToString();
        }

        protected virtual RequestBuilder Build(RequestBuilder builder)
        {
            return builder.
                Append("company", Company).
                Append("country_name", CountryName).
                Append("extended_address", ExtendedAddress).
                Append("first_name", FirstName).
                Append("last_name", LastName).
                Append("locality", Locality).
                Append("postal_code", PostalCode).
                Append("region", Region).
                Append("street_address", StreetAddress);
        }

        public override string ToQueryString()
        {
            return ToQueryString("address");
        }

        public override string ToQueryString(string root)
        {
            return QueryStringBody(root).ToString();
        }

        protected virtual QueryString QueryStringBody(string root)
        {
            return new QueryString().
                Append(ParentBracketChildString(root, "first_name"), FirstName).
                Append(ParentBracketChildString(root, "last_name"), LastName).
                Append(ParentBracketChildString(root, "company"), Company).
                Append(ParentBracketChildString(root, "street_address"), StreetAddress).
                Append(ParentBracketChildString(root, "extended_address"), ExtendedAddress).
                Append(ParentBracketChildString(root, "locality"), Locality).
                Append(ParentBracketChildString(root, "region"), Region).
                Append(ParentBracketChildString(root, "postal_code"), PostalCode).
                Append(ParentBracketChildString(root, "country_name"), CountryName);
        }
    }
}
