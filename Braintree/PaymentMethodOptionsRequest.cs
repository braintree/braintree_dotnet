#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class PaymentMethodOptionsRequest : Request
    {
        public bool? MakeDefault { get; set; }
        public bool? VerifyCard { get; set; }
        public bool? FailOnDuplicatePaymentMethod { get; set; }
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
            var builder = new RequestBuilder(root);
            builder.AddElement("make-default", MakeDefault);
            builder.AddElement("verification-merchant-account-id", VerificationMerchantAccountId);
            builder.AddElement("verify-card", VerifyCard);
            builder.AddElement("fail-on-duplicate-payment-method", FailOnDuplicatePaymentMethod);
            return builder;
        }
    }
}
