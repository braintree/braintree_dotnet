#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Braintree
{
    public class RequestBuilder
    {
        private string Parent;
        private Dictionary<string, string> TopLevelElements;
        private Dictionary<string, object> Elements;

        public RequestBuilder() : this("")
        {
        }

        public RequestBuilder(string parent)
        {
            Parent = parent;
            TopLevelElements = new Dictionary<string, string>();
            Elements = new Dictionary<string, object>();
        }

        public RequestBuilder AddElement(string name, object value)
        {
            Elements.Add(name, value);
            return this;
        }

        public RequestBuilder AddTopLevelElement(string name, string value)
        {
            TopLevelElements.Add(name, value);
            return this;
        }

        public string ToXml()
        {
            var builder = new StringBuilder();

            builder.Append(string.Format("<{0}>", Parent));
            foreach (KeyValuePair<string, object> pair in Elements)
            {
                builder.Append(BuildXMLElement(pair.Key, pair.Value));
            }
            builder.Append(string.Format("</{0}>", Parent));

            return builder.ToString();
        }

        public string ToQueryString()
        {
            string underscoredParent = Parent.Replace("-", "_");

            QueryString qs = new QueryString();
            foreach (KeyValuePair<string, string> pair in TopLevelElements)
            {
                qs.Append(pair);
            }
            foreach (KeyValuePair<string, object> pair in Elements)
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
                string xml = "";
                foreach (object item in (Array)value)
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
            return string.Format("<{0} type=\"array\">{1}</{0}>", SecurityElement.Escape(name), value);
        }

        private static string FormatAsXml(string name, string value)
        {
            return string.Format("<{0}>{1}</{0}>", SecurityElement.Escape(name), SecurityElement.Escape(value));
        }

        private static string FormatAsXml(string name, DateTime value)
        {
            return string.Format("<{0} type=\"datetime\">{1:u}</{0}>", SecurityElement.Escape(name), value.ToUniversalTime());
        }

        private static string FormatAsXml(string root, Dictionary<string, string> elements)
        {
            if (elements == null) return "";

            var builder = new StringBuilder();
            builder.Append(string.Format("<{0}>", root));

            foreach (KeyValuePair<string, string> element in elements)
            {
                builder.Append(BuildXMLElement(element.Key, element.Value));
            }

            builder.Append(string.Format("</{0}>", root));

            return builder.ToString();
        }

        internal static string ParentBracketChildString(string parent, string child)
        {
            return string.Format("{0}[{1}]", parent, child);
        }
    }
}
