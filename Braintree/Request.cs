#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public abstract class Request
    {
        public virtual string ToXml()
        {
            throw new NotImplementedException();
        }

        public virtual string ToXml(string rootElement)
        {
            throw new NotImplementedException();
        }

        public virtual string ToQueryString()
        {
            throw new NotImplementedException();
        }

        public virtual string ToQueryString(string root)
        {
            throw new NotImplementedException();
        }

        internal virtual string BuildXMLElement(string name, object value)
        {
            return RequestBuilder.BuildXMLElement(name, value);
        }

        public virtual string Kind()
        {
            return null;
        }

        protected virtual string ParentBracketChildString(string parent, string child)
        {
            return RequestBuilder.ParentBracketChildString(parent, child);
        }
    }
}
