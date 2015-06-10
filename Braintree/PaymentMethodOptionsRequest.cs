#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class PaymentMethodOptionsRequest : Request
    {
        public Boolean? MakeDefault { get; set; }
        public Boolean? VerifyCard { get; set; }
        public Boolean? FailOnDuplicatePaymentMethod { get; set; }
        public string VerificationMerchantAccountId { get; set; }

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
            RequestBuilder builder = new RequestBuilder(root);
            builder.AddElement("make-default", MakeDefault);
            builder.AddElement("verification-merchant-account-id", VerificationMerchantAccountId);
            builder.AddElement("verify-card", VerifyCard);
            builder.AddElement("fail-on-duplicate-payment-method", FailOnDuplicatePaymentMethod);
            return builder;
        }
    }
}
