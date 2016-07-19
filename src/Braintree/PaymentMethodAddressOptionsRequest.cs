using System;

namespace Braintree
{
    public class PaymentMethodAddressOptionsRequest : Request
    {
        public bool? UpdateExisting { get; set; }

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
            return new RequestBuilder(root).AddElement("update-existing", UpdateExisting);
        }
    }
}
