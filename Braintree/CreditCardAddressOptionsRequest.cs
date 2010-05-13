#pragma warning disable 1591

using System;
using System.Text;

namespace Braintree
{
    public class CreditCardAddressOptionsRequest : Request
    {
        public bool? UpdateExisting { get; set; }

        public override String ToXml()
        {
            return Build(new XmlRequestBuilder()).ToString();
        }

        public override String ToXml(String rootElement)
        {
            return Build(new XmlRequestBuilder(rootElement)).ToString();
        }

        protected virtual RequestBuilder Build(RequestBuilder builder)
        {
            return builder.Append("update_existing", UpdateExisting);
        }

        public override String ToQueryString()
        {
            return ToQueryString("options");
        }

        public override String ToQueryString(String root)
        {
            return new QueryString().
                Append(ParentBracketChildString(root, "update_existing"), UpdateExisting).
                ToString();
        }

    }
}
