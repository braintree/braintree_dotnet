#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class ClientTokenOptionsRequest : Request
    {
        public Boolean? MakeDefault { get; set; }
        public Boolean? VerifyCard { get; set; }
        public Boolean? FailOnDuplicatePaymentMethod { get; set; }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("make-default", MakeDefault).
                AddElement("verify-card", VerifyCard).
                AddElement("fail-on-duplicate-payment-method", FailOnDuplicatePaymentMethod);
        }
    }
}
