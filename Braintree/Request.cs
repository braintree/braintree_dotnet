#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Braintree
{
    public abstract class Request
    {
        public abstract String ToXml();
        public abstract String ToXml(String rootElement);
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
                builder.Append(BuildXMLElement(element.Key, element.Value));
            }

            builder.Append(String.Format("</{0}>", rootElement));

            return builder.ToString();
        }

        internal virtual String BuildXMLElement(String tagName, Boolean value)
        {
            return BuildXMLElement(tagName, value.ToString().ToLower());
        }

        internal virtual String BuildXMLElement(String tagName, String value)
        {
            if (value == null) return "";

            return String.Format("<{0}>{1}</{0}>", SecurityElement.Escape(tagName), SecurityElement.Escape(value));
        }

        protected virtual String ParentBracketChildString(String parent, String child)
        {
            return String.Format("{0}[{1}]", parent, child);
        }
    }
}
