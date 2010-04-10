#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    /// <summary>
    /// A class for building requests to manipulate <see cref="Customer"/> records in the vault.
    /// </summary>
    /// <example>
    /// A customer request can be constructed as follows:
    /// <code>
    /// var createRequest = new CustomerRequest
    /// {
    ///     Id = id,
    ///     FirstName = "Michael",
    ///     LastName = "Angelo",
    ///     Company = "Some Company",
    ///     Email = "mike.a@example.com",
    ///     Phone = "312.555.1111",
    ///     Fax = "312.555.1112",
    ///     Website = "www.example.com",
    ///     CreditCard = new CreditCardRequest
    ///     {
    ///         Number = "5105105105105100",
    ///         ExpirationDate = "05/12",
    ///         CVV = "123",
    ///         CardholderName = "Michael Angelo",
    ///         BillingAddress = new AddressRequest()
    ///         {
    ///             FirstName = "Mike",
    ///             LastName = "Smith",
    ///             Company = "Smith Co.",
    ///             StreetAddress = "1 W Main St",
    ///             ExtendedAddress = "Suite 330",
    ///             Locality = "Chicago",
    ///             Region = "IL",
    ///             PostalCode = "60622",
    ///             CountryName = "United States of America"
    ///         }
    ///     }
    /// };
    /// </code>
    /// </example>
    public class CustomerRequest : Request
    {
        public String Id { get; set; }
        public String CustomerId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Company { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String Fax { get; set; }
        public String Website { get; set; }
        public Dictionary<String, String> CustomFields { get; set; }
        public CreditCardRequest CreditCard { get; set; }

        public override String ToXml()
        {
            return ToXml("customer");
        }

        public override String ToXml(String rootElement)
        {
            var builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            builder.Append(BuildXMLElement("id", Id));
            builder.Append(BuildXMLElement("first-name", FirstName));
            builder.Append(BuildXMLElement("last-name", LastName));
            builder.Append(BuildXMLElement("company", Company));
            builder.Append(BuildXMLElement("email", Email));
            builder.Append(BuildXMLElement("phone", Phone));
            builder.Append(BuildXMLElement("fax", Fax));
            builder.Append(BuildXMLElement("website", Website));
            builder.Append(BuildXMLElement(CreditCard));
            builder.Append(BuildXMLElement("custom-fields", CustomFields));
            builder.Append(String.Format("</{0}>", rootElement));

            return builder.ToString();
        }

        public override String ToQueryString()
        {
            return ToQueryString("customer");
        }

        public override String ToQueryString(String root)
        {
            return new QueryString().
                Append(ParentBracketChildString(root, "id"), Id).
                Append(ParentBracketChildString(root, "first_name"), FirstName).
                Append(ParentBracketChildString(root, "last_name"), LastName).
                Append(ParentBracketChildString(root, "company"), Company).
                Append(ParentBracketChildString(root, "email"), Email).
                Append(ParentBracketChildString(root, "phone"), Phone).
                Append(ParentBracketChildString(root, "fax"), Fax).
                Append(ParentBracketChildString(root, "website"), Website).
                Append(ParentBracketChildString(root, "credit_card"), CreditCard).
                Append(ParentBracketChildString(root, "custom_fields"), CustomFields).
                Append("customer_id", CustomerId).
                ToString();
        }
    }
}

