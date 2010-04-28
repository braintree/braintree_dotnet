#pragma warning disable 1591

using System;
using System.Text;

namespace Braintree
{
    public class SearchCriteria : Request
    {
        private String Xml;

        public SearchCriteria(String type, String value)
        {
            Xml = BuildXMLElement(type, value);
        }

        public SearchCriteria(String type, DateTime value)
        {
            Xml = BuildXMLElement(type, value);
        }

        public SearchCriteria(object[] items)
        {
            StringBuilder builder = new StringBuilder();
            foreach(object item in items) {
                builder.Append(BuildXMLElement("item", item.ToString()));
            }
            Xml = builder.ToString();
        }

        public override String ToXml()
        {
            return Xml;
        }

        public override String ToXml(String rootElement)
        {
            throw new NotImplementedException();
        }

        public override String ToQueryString()
        {
            throw new NotImplementedException();
        }

        public override String ToQueryString(String root)
        {
            throw new NotImplementedException();
        }
    }
}
