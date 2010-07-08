#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Braintree
{
    public class RequestBuilder
    {
        private String Parent;
        private Dictionary<string, string> StringElements;
        private Dictionary<string, Request> RequestElements;

        public RequestBuilder(String parent)
        {
            Parent = parent;
            StringElements = new Dictionary<string, string>();
            RequestElements = new Dictionary<string, Request>();
        }

        public RequestBuilder AddElement(String name, String value)
        {
            StringElements.Add(name, value);
            return this;
        }

        public RequestBuilder AddElement(String name, Request request)
        {
            RequestElements.Add(name, request);
            return this;
        }

        public String ToXml()
        {
            var builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", Parent));
            foreach (KeyValuePair<String, String> pair in StringElements)
            {
                builder.Append(BuildXMLElement(pair.Key, pair.Value));
            }
            foreach (KeyValuePair<String, Request> pair in RequestElements)
            {
                builder.Append(BuildXMLElement(pair.Key, pair.Value));
            }
            builder.Append(String.Format("</{0}>", Parent));
            return builder.ToString();
        }

        public String ToQueryString()
        {
            QueryString qs = new QueryString();
            foreach (KeyValuePair<String, String> pair in StringElements)
            {
                qs.Append(ParentBracketChildString(Parent, pair.Key.Replace("-", "_")), pair.Value);
            }
            foreach (KeyValuePair<String, Request> pair in RequestElements)
            {
                qs.Append(ParentBracketChildString(Parent, pair.Key.Replace("-", "_")), pair.Value);
            }
            return qs.ToString();
        }

        private String BuildXMLElement(String tagName, String value)
        {
            if (value == null) return "";

            return String.Format("<{0}>{1}</{0}>", SecurityElement.Escape(tagName), SecurityElement.Escape(value));
        }

        private String BuildXMLElement(String tagName, Request request)
        {
            if (request == null) return "";

            return request.ToXml(tagName);
        }

        private String ParentBracketChildString(String parent, String child)
        {
            return String.Format("{0}[{1}]", parent, child);
        }
    }
}
