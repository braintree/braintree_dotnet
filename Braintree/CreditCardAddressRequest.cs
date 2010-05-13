#pragma warning disable 1591

using System;

namespace Braintree
{
    public class CreditCardAddressRequest : AddressRequest
    {
        public CreditCardAddressOptionsRequest Options { get; set; }

        protected override RequestBuilder Build(RequestBuilder builder)
        {
            return base.Build(builder).Append("options", Options);
        }

        protected override QueryString QueryStringBody(String root)
        {
             return base.QueryStringBody(root)
                .Append(ParentBracketChildString(root, "options"), Options);
        }
    }
}
