#pragma warning disable 1591

using System.ComponentModel;

namespace Braintree
{
    public enum ShippingMethod
    {
        [Description("same_day")] SAME_DAY,
        [Description("next_day")] NEXT_DAY,
        [Description("priority")] PRIORITY,
        [Description("ground")] GROUND,
        [Description("electronic")] ELECTRONIC,
        [Description("ship_to_store")] SHIP_TO_STORE,
        [Description("pickup_in_store")] PICKUP_IN_STORE 
    }

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
    ///      Phone = "312.555.1111",
    ///      InternationalPhone = new InternationalPhone()
    ///      {
    ///         CountryCode = "1",
    ///         NationalNumber = "3121234567"
    ///      }
    ///      PostalCode = "60622",
    ///      CountryName = "United States of America"
    /// };
    /// </code>
    /// </example>
    public class AddressRequest : Request
    {
        public string Company { get; set; }
        public string CountryCodeAlpha2 { get; set; }
        public string CountryCodeAlpha3 { get; set; }
        public string CountryCodeNumeric { get; set; }
        public string CountryName { get; set; }
        public string ExtendedAddress { get; set; }
        public string FirstName { get; set; }
        public InternationalPhoneRequest InternationalPhone { get; set; }
        public string LastName { get; set; }
        public string Locality { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public ShippingMethod? ShippingMethod { get; set; }
        public string StreetAddress { get; set; }

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
                AddElement("company", Company).
                AddElement("country-code-alpha2", CountryCodeAlpha2).
                AddElement("country-code-alpha3", CountryCodeAlpha3).
                AddElement("country-code-numeric", CountryCodeNumeric).
                AddElement("country-name", CountryName).
                AddElement("extended-address", ExtendedAddress).
                AddElement("first-name", FirstName).
                AddElement("international-phone", InternationalPhone).
                AddElement("last-name", LastName).
                AddElement("locality", Locality).
                AddElement("phone-number", PhoneNumber).
                AddElement("postal-code", PostalCode).
                AddElement("region", Region).
                AddElement("shipping-method", ShippingMethod.GetDescription()).
                AddElement("street-address", StreetAddress);
        }
    }
}
