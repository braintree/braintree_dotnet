using System;

namespace Braintree
{
    public class PaymentMethodAddressOptionsRequest : Request
    {
        public Boolean? UpdateExisting { get; set; }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).AddElement("update-existing", UpdateExisting);
        }
    }
}
