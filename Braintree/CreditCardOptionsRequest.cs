#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class CreditCardOptionsRequest : Request
    {
        public bool? VerifyCard { get; set; }
        public bool? MakeDefault { get; set; }
        public string VerificationMerchantAccountId { get; set; }

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
                ToString();
        }
    }
}
