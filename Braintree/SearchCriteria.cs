#pragma warning disable 1591

using System;

namespace Braintree
{
    public class SearchCriteria
    {
        private String Xml;

        public SearchCriteria (String type, String value)
        {
             Xml = String.Format("<{0}>{1}</{0}>", type, value);
        }

        public virtual String ToXml()
        {
            return Xml;
        }
    }
}
