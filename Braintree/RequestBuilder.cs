#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Braintree
{
    public abstract class RequestBuilder
    {
        protected string Parent { get; set; }
        protected Dictionary<string, string> Nodes { get; set; }

        protected abstract string Format(object content);

        internal virtual RequestBuilder Append(string node, object content)
        {
            if (content == null || content.ToString() == "") return this;

            Nodes.Add(SecurityElement.Escape(node), Format(content));

            return this;
        }

        internal RequestBuilder ()
        {
            Nodes = new Dictionary<string, string>();
        }

        internal RequestBuilder (string parent)
         {
            Parent = parent;
            Nodes = new Dictionary<string, string>();
        }

        internal virtual RequestBuilder Override(string original, string corrected, object content)
        {
            Nodes.Remove(original);

            return Append(corrected, content);
        }


        public abstract override string ToString();
    }
}
