#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class CreditCardVerificationOptionsRequest : Request
    {
        public String Amount { get; set; }
        public String MerchantAccountId { get; set; }

        public override string ToXml()
        {
            return ToXml("options");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);
            builder.AddElement("amount", Amount);
            builder.AddElement("merchant-account-id", MerchantAccountId);

            return builder;
        }
    }
}
