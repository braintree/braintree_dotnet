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

    }
}
