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

        public RequestBuilder Append(string node, object content)
        {
            if (content == null || content.ToString() == "") return this;

            Nodes.Add(node, content.ToString());

            return this;
        }

        public abstract override string ToString();
    }
}
