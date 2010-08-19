#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public abstract class SearchRequest : Request
    {
        private Dictionary<String, SearchCriteria> Criteria;
        private Dictionary<String, SearchCriteria> MultipleValueCriteria;
        private Dictionary<String, List<SearchCriteria>> RangeCriteria;
        private Dictionary<String, String> KeyValueCriteria;

        protected SearchRequest()
        {
            Criteria = new Dictionary<String, SearchCriteria>();
            MultipleValueCriteria = new Dictionary<String, SearchCriteria>();
            RangeCriteria = new Dictionary<String, List<SearchCriteria>>();
            KeyValueCriteria = new Dictionary<String, String>();
        }

        internal virtual void AddCriteria(String name, SearchCriteria criteria)
        {
            Criteria.Add(name, criteria);
        }

        internal virtual void AddRangeCriteria(String name, SearchCriteria criteria)
        {
            if (!RangeCriteria.ContainsKey(name))
            {
                RangeCriteria.Add(name, new List<SearchCriteria>());
            }
            RangeCriteria[name].Add(criteria);
        }

        internal virtual void AddMultipleValueCriteria(String name, SearchCriteria criteria)
        {
            if (MultipleValueCriteria.ContainsKey(name))
            {
                MultipleValueCriteria.Remove(name);
            }
            MultipleValueCriteria.Add(name, criteria);
        }

        internal virtual void AddCriteria(String name, String value)
        {
            KeyValueCriteria.Add(name, value);
        }

        public override String ToXml()
        {
            var builder = new StringBuilder();
            builder.Append("<search>");
            foreach (KeyValuePair<String, SearchCriteria> pair in Criteria)
            {
                builder.AppendFormat("<{0}>{1}</{0}>", pair.Key, pair.Value.ToXml());
            }
            foreach (KeyValuePair<String, List<SearchCriteria>> pair in RangeCriteria)
            {
                builder.AppendFormat("<{0}>", pair.Key);
                foreach (SearchCriteria criteria in pair.Value)
                {
                    builder.Append(criteria.ToXml());
                }
                builder.AppendFormat("</{0}>", pair.Key);
            }
            foreach (KeyValuePair<String, SearchCriteria> pair in MultipleValueCriteria)
            {
                builder.AppendFormat("<{0} type=\"array\">{1}</{0}>", pair.Key, pair.Value.ToXml());
            }
            foreach (KeyValuePair<String, String> pair in KeyValueCriteria)
            {
                builder.Append(BuildXMLElement(pair.Key, pair.Value));
            }

            builder.Append("</search>");
            return builder.ToString();
        }
    }
}
