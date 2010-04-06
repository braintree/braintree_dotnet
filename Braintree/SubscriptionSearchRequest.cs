#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class SubscriptionSearchRequest : Request
    {
        private Dictionary<String, SearchCriteria> Criteria;

        public SubscriptionSearchRequest()
        {
            Criteria = new Dictionary<String, SearchCriteria>();
        }

        public virtual TextNode PlanId()
        {
            return new TextNode("planId", this);
        }

        public virtual TextNode DaysPastDue()
        {
            return new TextNode("daysPastDue", this);
        }

        internal virtual void AddCriteria(String name, SearchCriteria criteria)
        {
            Criteria.Add(name, criteria);
        }

        public override String ToXml()
        {
            var builder = new StringBuilder();
            builder.Append("<search>");
            foreach (KeyValuePair<String, SearchCriteria> pair in Criteria)
            {
                builder.Append(BuildXMLElement(pair.Key, pair.Value.ToXml()));
            }
            builder.Append("</search>");
            return builder.ToString();
        }

        public override String ToXml(String rootElement)
        {
            return "";
        }

        public override String ToQueryString()
        {
            return "";
        }

        public override String ToQueryString(String root)
        {
            return "";
        }
    }
}
