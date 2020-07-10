#pragma warning disable 1591

using Braintree.Exceptions;
using System.Collections.Generic;

namespace Braintree
{
    /// <summary>
    /// A class for building requests with authentication insight options..
    /// </summary>
    public class AuthenticationInsightOptionsRequest : Request
    {
        public decimal Amount { get; set; }

        public override string ToXml()
        {
            return ToXml("authentication-insight-options");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("authentication-insight-options");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);

            builder.AddElement("amount", Amount);

            return builder;
        }
    }
}
