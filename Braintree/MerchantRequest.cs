using System;

namespace Braintree
{
    public class MerchantRequest : Request
    {
        public string Email { get; set; }
        public string CountryCodeAlpha3 { get; set; }
        public string[] PaymentMethods { get; set; }

        public override string ToXml()
        {
            return new RequestBuilder("merchant")
                .AddElement("email", Email)
                .AddElement("country_code_alpha3", CountryCodeAlpha3)
                .AddElement("payment_methods", PaymentMethods)
                .ToXml();
        }
    }
}
