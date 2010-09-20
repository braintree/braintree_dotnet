#pragma warning disable 1591

using System;
using System.Text;

namespace Braintree
{
    public class CreditCardAddressOptionsRequest : Request
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
