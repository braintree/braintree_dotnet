#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public abstract class Request
    {
        public abstract String ToXml();
        public abstract String ToXml(String rootElement);
        public abstract String ToQueryString();
        public abstract String ToQueryString(String root);

        internal virtual string BuildXMLElement(string name, object value)
        {
            return RequestBuilder.BuildXMLElement(name, value);
        }

        public virtual String Kind()
        {
            return null;
        }

        protected virtual String ParentBracketChildString(String parent, String child)
        {
            return RequestBuilder.ParentBracketChildString(parent, child);
        }
    }
}
