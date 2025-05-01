#pragma warning disable 1591

using System.Collections.Generic;
using System;

namespace Braintree 
{
    public class PaymentFacilitatorRequest : Request
    {
        public string PaymentFacilitatorId { get; set; }
        public TransactionSubMerchantRequest SubMerchant { get; set; }


        public override string ToXml()
        {
            return ToXml("payment-facilitator");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("payment-facilitator");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root)
                .AddElement("payment-facilitator-id", PaymentFacilitatorId)
                .AddElement("sub-merchant", SubMerchant);
        }
    }
}