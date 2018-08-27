using System.Collections.Generic;

namespace Braintree
{
    public class UsBankAccountVerificationConfirmRequest : Request
    {
        public int[] DepositAmounts { get; set; }

        public override string ToXml()
        {
            return ToXml("us-bank-account-verification");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("deposit-amounts", DepositAmounts);
        }
    }
}
