#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class CreditCardVerificationCreditCardRequest : BaseCreditCardRequest {
        public CreditCardAddressRequest BillingAddress { get; set; }
    }

    public class CreditCardVerificationRequest : Request {
        public CreditCardVerificationCreditCardRequest CreditCard { get; set; }
        public CreditCardVerificationOptionsRequest Options { get; set; }

        public override string ToXml()
        {
            return ToXml("verification");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);
            builder.AddElement("credit-card", CreditCard);
            builder.AddElement("options", Options);

            return builder;
        }
    }
}
