#pragma warning disable 1591

using System;
using System.Text;

namespace Braintree
{
    public class CreditCardAddressOptionsRequest : Request
    {
        public Boolean UpdateExisting { get; set; }

        public override String ToXml()
        {
            return ToXml("options");
        }

        public override String ToXml(String rootElement)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));
            builder.Append(BuildXMLElement("update-existing", UpdateExisting));
            builder.Append(String.Format("</{0}>", rootElement));
            
            return builder.ToString();
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
