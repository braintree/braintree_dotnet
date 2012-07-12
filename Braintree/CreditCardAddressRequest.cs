#pragma warning disable 1591

using System;

namespace Braintree
{
    [Serializable]
    public class CreditCardAddressRequest : AddressRequest
    {
        public CreditCardAddressOptionsRequest Options { get; set; }

        protected override RequestBuilder BuildRequest(String root)
        {
            return base.BuildRequest(root).AddElement("options", Options);
        }
    }
}
