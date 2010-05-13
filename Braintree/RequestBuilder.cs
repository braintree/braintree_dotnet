#pragma warning disable 1591

using System;
using System.Collections.Generic;

namespace Braintree
{
    public abstract class RequestBuilder
    {
        protected string Parent { get; set; }
        protected Dictionary<string, string> Nodes { get; set; }

        public RequestBuilder (string parent)
         {
            Parent = parent;
            Nodes = new Dictionary<string, string>();
        }

        public RequestBuilder Append(string node, string content)
        {
            if (content != null && content.Trim() != "")
            {
                Nodes.Add(node, content);
            }

            return this;
        }

        public RequestBuilder Append(string node, decimal content)
        {
            Nodes.Add(node, content.ToString());

            return this;
        }

        public RequestBuilder Append(string node, bool? content)
        {
            if (content.HasValue)
            {
                Nodes.Add(node, content.Value.ToString().ToLower());
            }

            return this;
        }

        public abstract override string ToString();
    }
}
