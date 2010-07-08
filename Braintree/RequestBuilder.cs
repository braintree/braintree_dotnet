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
                qs.Append(pair);
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

        internal static string BuildXMLElement(string name, object value)
        {
            if (value == null)
            {
                return "";
            }
            if (value is Request)
            {
                return ((Request) value).ToXml(name);
            }
            if (value is DateTime)
            {
                return FormatAsXml(name, (DateTime) value);
            }
            if (value is bool)
            {
                return FormatAsXml(name, value.ToString().ToLower());
            }
            else if (value is Dictionary<string, string>)
            {
                return FormatAsXml(name, (Dictionary<string, string>) value);
            }

            return FormatAsXml(name, value.ToString());
        }

        private static string FormatAsXml(string name, string value)
        {
            return String.Format("<{0}>{1}</{0}>", SecurityElement.Escape(name), SecurityElement.Escape(value));
        }

        private static string FormatAsXml(string name, DateTime value)
        {
            return String.Format("<{0} type=\"datetime\">{1:u}</{0}>", SecurityElement.Escape(name), value.ToUniversalTime());
        }

        private static String FormatAsXml(string root, Dictionary<String, String> elements)
        {
            if (elements == null) return "";

            var builder = new StringBuilder();
            builder.Append(String.Format("<{0}>", root));

            foreach (KeyValuePair<String, String> element in elements)
            {
                builder.Append(BuildXMLElement(element.Key, element.Value));
            }

            builder.Append(String.Format("</{0}>", root));

            return builder.ToString();
        }

        internal static String ParentBracketChildString(String parent, String child)
        {
            return String.Format("{0}[{1}]", parent, child);
        }
    }
}
