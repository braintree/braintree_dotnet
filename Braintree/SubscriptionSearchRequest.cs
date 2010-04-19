#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class SubscriptionSearchRequest : Request
    {
        private Dictionary<String, SearchCriteria> Criteria;
        private Dictionary<String, SearchCriteria> MultipleValueCriteria;

        public SubscriptionSearchRequest()
        {
            Criteria = new Dictionary<String, SearchCriteria>();
            MultipleValueCriteria = new Dictionary<String, SearchCriteria>();
        }

        public virtual TextNode PlanId()
        {
            return new TextNode("plan-id", this);
        }

        public virtual TextNode DaysPastDue()
        {
            return new TextNode("days-past-due", this);
        }

        public virtual MultipleValueNode Status()
        {
            return new MultipleValueNode("status", this);
        }

        internal virtual void AddCriteria(String name, SearchCriteria criteria)
        {
            Criteria.Add(name, criteria);
        }


        internal virtual void AddMultipleValueCriteria(String name, SearchCriteria criteria)
        {
            MultipleValueCriteria.Add(name, criteria);
        }

        public override String ToXml()
        {
            var builder = new StringBuilder();
            builder.Append("<search>");
            foreach (KeyValuePair<String, SearchCriteria> pair in Criteria)
            {
                builder.AppendFormat("<{0}>{1}</{0}>", pair.Key, pair.Value.ToXml());
            }
            foreach (KeyValuePair<String, SearchCriteria> pair in MultipleValueCriteria)
            {
                builder.Append(BuildXMLElement(pair.Key, pair.Value.ToXml(), "array"));
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
