#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Text;
using System.Xml;

namespace Braintree
{
    public class RequestBuilder
    {
        private string Parent;
        private List<KeyValuePair<string, string>> topLevelElements;
        private List<KeyValuePair<string, object>> elements;

        public RequestBuilder() : this("")
        {
        }

        public RequestBuilder(string parent)
        {
            Parent = parent;
            topLevelElements = new List<KeyValuePair<string, string>>();
            elements = new List<KeyValuePair<string, object>>();
        }

        public RequestBuilder AddElement(string name, object value)
        {
            elements.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        public RequestBuilder AddTopLevelElement(string name, string value)
        {
            topLevelElements.Add(new KeyValuePair<string, string>(name, value));
            return this;
        }

        public string ToXml()
        {
            var builder = new StringBuilder();

            builder.Append(string.Format("<{0}>", Parent));
            foreach (var pair in elements)
            {
                builder.Append(BuildXMLElement(pair.Key, pair.Value));
            }
            builder.Append(string.Format("</{0}>", Parent));

            return builder.ToString();
        }

        public string ToQueryString()
        {
            string underscoredParent = Parent.Replace("-", "_");

            var qs = new QueryString();
            foreach (var pair in topLevelElements)
            {
                qs.Append(pair);
            }
            foreach (var pair in elements)
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
            if (value is Array)
            {
                var xml = "";
                foreach (var item in (Array)value)
                {
                    xml += BuildXMLElement("item", item);
                }
                return FormatAsArrayXML(name, xml);
            }
            if (value is Dictionary<string, string>)
            {
                return FormatAsXml(name, (Dictionary<string, string>) value);
            }
            if (value is decimal)
            {
                return FormatAsXml(name, ((decimal) value).ToString(System.Globalization.CultureInfo.InvariantCulture));
            }

            return FormatAsXml(name, value.ToString());
        }

        private static string FormatAsArrayXML(string name, string value)
        {
            return string.Format("<{0} type=\"array\">{1}</{0}>", EncodeXml(name), value);
        }

        private static string FormatAsXml(string name, string value)
        {
            return string.Format("<{0}>{1}</{0}>", EncodeXml(name), EncodeXml(value));
        }

        private static string FormatAsXml(string name, DateTime value)
        {
            return string.Format("<{0} type=\"datetime\">{1:u}</{0}>", EncodeXml(name), value.ToUniversalTime());
        }

        private static string FormatAsXml(string root, Dictionary<string, string> elements)
        {
            if (elements == null) return "";

            var builder = new StringBuilder();
            builder.Append(string.Format("<{0}>", root));

            foreach (var element in elements)
            {
                builder.Append(BuildXMLElement(element.Key, element.Value));
            }

            builder.Append(string.Format("</{0}>", root));

            return builder.ToString();
        }

        private static string EncodeXml(string input)
        {
            var sb = new StringBuilder(input);

            input.Replace("&", "&amp;");
            input.Replace("<", "$lt;");
            input.Replace(">", "&gt;");
            input.Replace("\"", "&quote;");
            input.Replace("'", "&apos;");

            return input.ToString();
        }

        internal static string ParentBracketChildString(string parent, string child)
        {
            return string.Format("{0}[{1}]", parent, child);
        }
    }
}
