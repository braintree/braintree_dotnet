#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class PayPalOptionsRequest : Request
    {
        public bool? MakeDefault { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            RequestBuilder builder = new RequestBuilder(root);
            builder.AddElement("make-default", MakeDefault);
            return builder;
        }
    }
}
