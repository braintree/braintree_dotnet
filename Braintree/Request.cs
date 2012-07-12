#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    [Serializable]
    public abstract class Request
    {
        public virtual String ToXml()
        {
            throw new NotImplementedException();
        }

        public virtual String ToXml(String rootElement)
        {
            throw new NotImplementedException();
        }

        public virtual String ToQueryString()
        {
            throw new NotImplementedException();
        }

        public virtual String ToQueryString(String root)
        {
            throw new NotImplementedException();
        }

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
