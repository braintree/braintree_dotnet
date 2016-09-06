#pragma warning disable 1591

using System;

namespace Braintree
{
    public class CreditCardVerificationOptionsRequest : Request
    {
        public string Amount { get; set; }
        public string MerchantAccountId { get; set; }

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
            return new RequestBuilder(root).
                AddElement("amount", Amount).
                AddElement("merchant-account-id", MerchantAccountId);
        }
    }
}
