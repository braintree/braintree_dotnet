#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{

    [Serializable]
    public class TransactionCloneRequest : Request
    {
        public Decimal Amount { get; set; }
        public TransactionOptionsCloneRequest Options { get; set; }

        public override String ToXml()
        {
            return ToXml("transaction-clone");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            RequestBuilder builder = new RequestBuilder(root);
            builder.AddElement("amount", Amount);
            builder.AddElement("options", Options);

            return builder;
        }
    }
}
