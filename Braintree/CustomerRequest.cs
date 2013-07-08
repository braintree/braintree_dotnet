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
        public String BundledParams { get; set; }
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

        public override String Kind()
        {
            if (CustomerId == null)
            {
                return TransparentRedirectGateway.CREATE_CUSTOMER;
            }
            else
            {
                return TransparentRedirectGateway.UPDATE_CUSTOMER;
            }
        }

        public override String ToXml()
        {
            return ToXml("customer");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString()
        {
            return ToQueryString("customer");
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).
                AddTopLevelElement("customer_id", CustomerId).
                ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("id", Id).
                AddElement("first-name", FirstName).
                AddElement("last-name", LastName).
                AddElement("company", Company).
                AddElement("email", Email).
                AddElement("phone", Phone).
                AddElement("fax", Fax).
                AddElement("website", Website).
                AddElement("credit-card", CreditCard).
                AddElement("custom-fields", CustomFields);
        }
    }
}

