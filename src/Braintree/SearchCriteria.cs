#pragma warning disable 1591

using System;
using System.Text;

namespace Braintree
{
    public class SearchCriteria : Request
    {
        private string xml;

        public SearchCriteria(string type, string value)
        {
            xml = BuildXMLElement(type, DefaultToEmptyString(value));
        }

        public SearchCriteria(string type, DateTime value)
        {
            xml = BuildXMLElement(type, value);
        }

        public SearchCriteria(object[] items)
        {
            var builder = new StringBuilder();
            foreach (var item in items) {
                builder.Append(BuildXMLElement("item", item.ToString()));
            }
            xml = builder.ToString();
        }

        public override string ToXml()
        {
            return xml;
        }

        public override string ToXml(string rootElement)
        {
            throw new NotImplementedException();
        }

        public override string ToQueryString()
        {
            throw new NotImplementedException();
        }

        public override string ToQueryString(string root)
        {
            throw new NotImplementedException();
        }

        private string DefaultToEmptyString(string value)
        {
            if (value == null) {
                return "";
            } else {
                return value;
            }
        }
    }
}
