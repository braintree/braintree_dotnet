#pragma warning disable 1591

using System.Collections.Generic;

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
        public string Id { get; set; }
        public string DeviceData { get; set; }
        public string CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string PaymentMethodNonce { get; set; }
        public string DefaultPaymentMethodToken { get; set; }
        public Dictionary<string, string> CustomFields { get; set; }
        public CreditCardRequest CreditCard { get; set; }
        public UsBankAccountRequest UsBankAccount { get; set; }
        public RiskDataRequest RiskData { get; set; }
        public CustomerOptionsRequest Options { get; set; }
        public TaxIdentifierRequest[] TaxIdentifiers { get; set; }

        public override string ToXml()
        {
            return ToXml("customer");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("customer");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).
                AddTopLevelElement("customer_id", CustomerId).
                ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var request = new RequestBuilder(root).
                AddElement("id", Id).
                AddElement("first-name", FirstName).
                AddElement("last-name", LastName).
                AddElement("company", Company).
                AddElement("email", Email).
                AddElement("phone", Phone).
                AddElement("fax", Fax).
                AddElement("website", Website).
                AddElement("payment-method-nonce", PaymentMethodNonce).
                AddElement("default-payment-method-token", DefaultPaymentMethodToken).
                AddElement("credit-card", CreditCard).
                AddElement("custom-fields", CustomFields).
                AddElement("risk-data", RiskData).
                AddElement("device-data", DeviceData).
                AddElement("options", Options);

            if (TaxIdentifiers != null && TaxIdentifiers.Length > 0)
            {
                request.AddElement("tax-identifiers", TaxIdentifiers);
            }

            return request;
        }
    }
}
