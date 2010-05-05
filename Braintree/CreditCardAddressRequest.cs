#pragma warning disable 1591

using System;

namespace Braintree
{
    public class CreditCardAddressRequest : AddressRequest
    {
        public CreditCardAddressOptionsRequest Options { get; set; }

        protected override String XmlBody()
        {
            return base.XmlBody() + BuildXMLElement("options", Options);
        }

        protected override QueryString QueryStringBody(String root)
        {
             return base.QueryStringBody(root)
                .Append(ParentBracketChildString(root, "options"), Options);
        }
    }
}
