#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class CreditCardOptionsRequest : Request
    {
        public Boolean VerifyCard { get; set; }
        public Boolean MakeDefault { get; set; }
        public String VerificationMerchantAccountId { get; set; }
        public String UpdateExistingToken { get; set; }

        public override String ToXml()
        {
            return ToXml("options");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString()
        {
            return ToQueryString("options");
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            RequestBuilder builder = new RequestBuilder(root);

            if (MakeDefault)
            {
                builder.AddElement("make-default", MakeDefault.ToString().ToLower());
            }

            builder.AddElement("verification-merchant-account-id", VerificationMerchantAccountId);
            builder.AddElement("verify-card", VerifyCard.ToString().ToLower());
            builder.AddElement("update-existing-token", UpdateExistingToken);

            return builder;
        }
    }
}
