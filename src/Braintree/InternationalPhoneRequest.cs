using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class InternationalPhoneRequest: Request
    {
        public string CountryCode { get; set; }
        public string NationalNumber { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);
            builder.AddElement("country-code", CountryCode);
            builder.AddElement("national-number", NationalNumber);

            return builder;
        }
    }
}
