using System.Collections.Generic;

namespace Braintree
{
    public class TransactionOptionsCreditCardRequest : Request
    {
        public string AccountType { get; set; }
        public bool? ProcessDebitAsCredit { get; set; }

        public TransactionOptionsCreditCardRequest()
        {
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        private RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root).
                AddElement("account-type", AccountType).
                AddElement("process-debit-as-credit",ProcessDebitAsCredit);

            return builder;
        }
    }
}
