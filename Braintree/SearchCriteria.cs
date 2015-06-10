#pragma warning disable 1591

using System;
using System.Text;

namespace Braintree
{
    public class SearchCriteria : Request
    {
        private string Xml;

        public SearchCriteria(string type, string value)
        {
            Xml = BuildXMLElement(type, DefaultToEmptyString(value));
        }

        public SearchCriteria(string type, DateTime value)
        {
            Xml = BuildXMLElement(type, value);
        }

        public SearchCriteria(object[] items)
        {
            var builder = new StringBuilder();
            foreach (var item in items) {
                builder.Append(BuildXMLElement("item", item.ToString()));
            }
            Xml = builder.ToString();
        }

        public override string ToXml()
        {
            return Xml;
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
