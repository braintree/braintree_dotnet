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
        private Dictionary<string, string> TopLevelElements;
        private Dictionary<string, string> StringElements;
        private Dictionary<string, Request> RequestElements;
        private Dictionary<string, Dictionary<string, string>> CustomElements;

        public RequestBuilder(String parent)
        {
            Parent = parent;
            TopLevelElements = new Dictionary<string, string>();
            StringElements = new Dictionary<string, string>();
            RequestElements = new Dictionary<string, Request>();
            CustomElements = new Dictionary<string, Dictionary<string, string>>();
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

        public RequestBuilder AddElement(string name, Dictionary<string, string> value)
        {
            CustomElements.Add(name, value);
            return this;
        }

        public RequestBuilder AddTopLevelElement(string name, string value)
        {
            TopLevelElements.Add(name, value);
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
            foreach (KeyValuePair<String, Dictionary<string, string>> pair in CustomElements)
            {
                builder.Append(BuildXMLElement(pair.Key, pair.Value));
            }
            builder.Append(String.Format("</{0}>", Parent));
            return builder.ToString();
        }

        public String ToQueryString()
        {
            string underscoredParent = Parent.Replace("-", "_");

            QueryString qs = new QueryString();
            foreach (KeyValuePair<String, String> pair in TopLevelElements)
            {
                qs.Append(pair.Key.Replace("-", "_"), pair.Value);
            }
            foreach (KeyValuePair<String, String> pair in StringElements)
            {
                qs.Append(ParentBracketChildString(underscoredParent, pair.Key.Replace("-", "_")), pair.Value);
            }
            foreach (KeyValuePair<String, Request> pair in RequestElements)
            {
                qs.Append(ParentBracketChildString(underscoredParent, pair.Key.Replace("-", "_")), pair.Value);
            }
            foreach (KeyValuePair<String, Dictionary<string, string>> pair in CustomElements)
            {
                qs.Append(ParentBracketChildString(underscoredParent, pair.Key.Replace("-", "_")), pair.Value);
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

        private String BuildXMLElement(String rootElement, Dictionary<String, String> elements)
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

        private String ParentBracketChildString(String parent, String child)
        {
            return String.Format("{0}[{1}]", parent, child);
        }
    }
}
