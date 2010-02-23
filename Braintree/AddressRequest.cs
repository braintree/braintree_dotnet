using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class AddressRequest : Request
    {
        public String CustomerID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Company { get; set; }
        public String StreetAddress { get; set; }
        public String ExtendedAddress { get; set; }
        public String Locality { get; set; }
        public String Region { get; set; }
        public String PostalCode { get; set; }
        public String CountryName { get; set; }

        internal override String ToXml()
        {
            return ToXml("address");
        }

        internal override String ToXml(String rootElement)
        {
            var builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            builder.Append(BuildXMLElement("first-name", FirstName));
            builder.Append(BuildXMLElement("last-name", LastName));
            builder.Append(BuildXMLElement("company", Company));
            builder.Append(BuildXMLElement("street-address", StreetAddress));
            builder.Append(BuildXMLElement("extended-address", ExtendedAddress));
            builder.Append(BuildXMLElement("locality", Locality));
            builder.Append(BuildXMLElement("region", Region));
            builder.Append(BuildXMLElement("postal-code", PostalCode));
            builder.Append(BuildXMLElement("country-name", CountryName));
            builder.Append(String.Format("</{0}>", rootElement));

            return builder.ToString();
        }

        public override String ToQueryString()
        {
            return ToQueryString("address");
        }

        public override String ToQueryString(String root)
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
                Append(ParentBracketChildString(root, "country_name"), CountryName).
                ToString();
        }
    }
}
