#pragma warning disable 1591

using System;
using System.Text;

namespace Braintree
{
    public class ClientTokenOptionsRequest : Request
    {
        public bool? MakeDefault { get; set; }
        public bool? VerifyCard { get; set; }
        public bool? FailOnDuplicatePaymentMethod { get; set; }

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
            return new RequestBuilder(root).
                AddElement("make-default", MakeDefault).
                AddElement("verify-card", VerifyCard).
                AddElement("fail-on-duplicate-payment-method", FailOnDuplicatePaymentMethod);
        }
    }
}
