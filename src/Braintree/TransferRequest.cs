#pragma warning disable 1591

using System.Collections.Generic;
using System;

namespace Braintree 
{

    public class TransferRequest : Request
    {

        public string Type { get; set; }

        public override string ToXml()
        {
            return ToXml("transfer");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("transfer");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);
            if (Type != null) {
                builder.AddElement("type", Type);
            }
            return builder;
        }
    }
}
