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

        public override String ToXml(String rootElement)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            if (MakeDefault)
            {
                builder.Append(BuildXMLElement("make-default", MakeDefault));
            }
            builder.Append(BuildXMLElement("verification-merchant-account-id", VerificationMerchantAccountId));
            builder.Append(BuildXMLElement("verify-card", VerifyCard));
            builder.Append(BuildXMLElement("update-existing-token", UpdateExistingToken));
            builder.Append(String.Format("</{0}>", rootElement));
            
            return builder.ToString();
        }

        public override String ToQueryString()
        {
            return ToQueryString("options");
        }

        public override String ToQueryString(String root)
        {
            return new QueryString().
                Append(ParentBracketChildString(root, "verification_merchant_account_id"), VerificationMerchantAccountId).
                Append(ParentBracketChildString(root, "verify_card"), VerifyCard).
                Append(ParentBracketChildString(root, "make_default"), MakeDefault).
                Append(ParentBracketChildString(root, "update_existing_token"), UpdateExistingToken).
                ToString();
        }
    }
}
