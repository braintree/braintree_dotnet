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

        public RequestBuilder Append(string node, bool? content)
        {
            if (content.HasValue)
            {
                Nodes.Add(node, content.ToString().ToLower());
            }

            return this;
        }

        public RequestBuilder Append(string node, object content)
        {
            if (content == null) return this;

            var value = content.ToString().Trim();

            if (value != "")
            {
                Nodes.Add(node, value);
            }

            return this;
        }

        public abstract override string ToString();
    }
}
