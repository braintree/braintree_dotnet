#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Braintree
{
    public class XmlRequestBuilder
    {
        protected string Parent { get; set; }
        protected Dictionary<string, string> Nodes { get; set; }

        public XmlRequestBuilder (string parent)
         {
            Parent = parent;
            Nodes = new Dictionary<string, string>();
        }

        public XmlRequestBuilder Append(string node, string content)
        {
            if (content != null && content.Trim() != "")
            {
                Nodes.Add(node, content);
            }

            return this;
        }

        public XmlRequestBuilder Append(string node, decimal content)
        {
            Nodes.Add(node, content.ToString());

            return this;
        }

        public XmlRequestBuilder Append(string node, bool? content)
        {
            if (content.HasValue)
            {
                Nodes.Add(node, content.Value.ToString().ToLower());
            }

            return this;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("<{0}>", Parent);
            foreach (KeyValuePair<string, string> node in Nodes) {
                builder.AppendFormat("<{0}>{1}</{0}>", SecurityElement.Escape(Dasherize(node.Key)), SecurityElement.Escape(node.Value));
            }
            builder.AppendFormat("</{0}>", Parent);

            return builder.ToString();
        }

        protected string Dasherize(string value)
        {
            return value.Replace('_', '-');
        }
    }
}
