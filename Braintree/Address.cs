using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Braintree
{
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
        public String CountryName { get; protected set; }

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
            CountryName = node.GetString("country-name");
        }
    }
}
