using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public abstract class Request
    {
        internal abstract String ToXml();
        internal abstract String ToXml(String rootElement);
        public abstract String ToQueryString();
        public abstract String ToQueryString(String root);

        internal virtual String BuildXMLElement(Request request)
        {
            if (request == null) return "";

            return request.ToXml();
        }

        internal virtual String BuildXMLElement(String tagName, Request request)
        {
            if (request == null) return "";

            return request.ToXml(tagName);
        }

        internal virtual String BuildXMLElement(String rootElement, Dictionary<String, String> elements)
        {
            if (elements == null) return "";

            var builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", rootElement));

            foreach (KeyValuePair<String, String> element in elements)
            {
                builder.Append(String.Format("<{0}>{1}</{0}>", element.Key, element.Value));
            }

            builder.Append(String.Format("</{0}>", rootElement));

            return builder.ToString();
        }


        internal virtual String BuildXMLElement(String tagName, Boolean value)
        {
            return String.Format("<{0}>{1}</{0}>", tagName, value.ToString().ToLower());
        }

        internal virtual String BuildXMLElement(String tagName, String value)
        {
            // TODO: xml escape
            if (value == null) return "";

            return String.Format("<{0}>{1}</{0}>", tagName, value);
        }

        protected virtual String ParentBracketChildString(String parent, String child)
        {
            return String.Format("{0}[{1}]", parent, child);
        }
    }
}
