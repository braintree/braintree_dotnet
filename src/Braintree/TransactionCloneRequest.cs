#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
 
    public class TransactionCloneRequest : Request
    {
        public decimal Amount { get; set; }
        public string Channel { get; set; }
        public TransactionOptionsCloneRequest Options { get; set; }

        public override string ToXml()
        {
            return ToXml("transaction-clone");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);
            builder.AddElement("amount", Amount);
            builder.AddElement("channel", Channel);
            builder.AddElement("options", Options);

            return builder;
        }
    }
}
