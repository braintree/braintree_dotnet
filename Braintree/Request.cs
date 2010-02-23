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

        internal String BuildXMLElement(Request request)
        {
            if (request == null) return "";

            return request.ToXml();
        }

        internal String BuildXMLElement(String tagName, Request request)
        {
            if (request == null) return "";

            return request.ToXml(tagName);
        }

        internal String BuildXMLElement(String rootElement, Dictionary<String, String> elements)
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

        internal String BuildXMLElement(String tagName, String value)
        {
            // TODO: xml escape
            if (value == null) return "";

            return String.Format("<{0}>{1}</{0}>", tagName, value);
        }

        protected String ParentBracketChildString(String parent, String child)
        {
            return String.Format("{0}[{1}]", parent, child);
        }
    }
}
