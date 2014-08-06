using System;

namespace Braintree
{
    public abstract class GenericAddressRequest<TOptions> : AddressRequest
        where TOptions : Request
    {
        public TOptions Options { get; set; }

        protected override RequestBuilder BuildRequest(string root)
        {
            return base.BuildRequest(root).AddElement("options", Options);
        }
    }
}
