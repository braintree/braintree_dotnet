#pragma warning disable 1591

using System;
using System.Text;

namespace Braintree
{
    public class SearchCriteria
    {
        private String Xml;

        public SearchCriteria(String type, String value)
        {
             Xml = String.Format("<{0}>{1}</{0}>", type, value);
        }

        public SearchCriteria(object[] items)
        {
            StringBuilder builder = new StringBuilder();
            foreach(object item in items) {
                builder.AppendFormat("<item>{0}</item>", item.ToString());
            }
            Xml = builder.ToString();
        }

        public virtual String ToXml()
        {
            return Xml;
        }
    }
}
