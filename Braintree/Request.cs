#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Braintree
{
    public abstract class Request
    {
        public abstract String ToQueryString();
        public abstract String ToQueryString(String root);

        protected virtual string GetNodeName()
        {
            return Underscore(this.GetType().Name.Replace("Request", ""));
        }

        public virtual String ToXml()
        {
            return Build(new XmlRequestBuilder(GetNodeName())).ToString();
        }

        public virtual String ToXml(String rootElement)
        {
            return Build(new XmlRequestBuilder(rootElement)).ToString();
        }

        public virtual String ToInnerXml()
        {
            return Build(new XmlRequestBuilder()).ToString();
        }

        protected virtual RequestBuilder Build(RequestBuilder builder)
        {
            foreach (var property in this.GetType().GetProperties()) {
                builder.Append(Underscore(property.Name), property.GetValue(this, null));
            }

            return builder;
        }

        protected string Underscore(string nodeName)
        {
            var result = new StringBuilder();

            foreach (char character in nodeName) {
                if (char.IsUpper(character))
                {
                    if (result.ToString().Length != 0) result.Append('_');
                    result.Append(char.ToLower(character));
                }
                else
                {
                    result.Append(character);
                }
            }

            return result.ToString();
        }

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

        internal virtual String BuildXMLElement(String tagName, DateTime value)
        {
            return String.Format("<{0} type=\"datetime\">{1:u}</{0}>", SecurityElement.Escape(tagName), value.ToUniversalTime());
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
