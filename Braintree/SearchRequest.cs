#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public abstract class SearchRequest : Request
    {
        private Dictionary<string, SearchCriteria> Criteria;
        private Dictionary<string, SearchCriteria> MultipleValueCriteria;
        private Dictionary<string, List<SearchCriteria>> RangeCriteria;
        private Dictionary<string, string> KeyValueCriteria;

        protected SearchRequest()
        {
            Criteria = new Dictionary<string, SearchCriteria>();
            MultipleValueCriteria = new Dictionary<string, SearchCriteria>();
            RangeCriteria = new Dictionary<string, List<SearchCriteria>>();
            KeyValueCriteria = new Dictionary<string, string>();
        }

        internal virtual void AddCriteria(string name, SearchCriteria criteria)
        {
            Criteria.Add(name, criteria);
        }

        internal virtual void AddRangeCriteria(string name, SearchCriteria criteria)
        {
            if (!RangeCriteria.ContainsKey(name))
            {
                RangeCriteria.Add(name, new List<SearchCriteria>());
            }
            RangeCriteria[name].Add(criteria);
        }

        internal virtual void AddMultipleValueCriteria(string name, SearchCriteria criteria)
        {
            if (MultipleValueCriteria.ContainsKey(name))
            {
                MultipleValueCriteria.Remove(name);
            }
            MultipleValueCriteria.Add(name, criteria);
        }

        internal virtual void AddCriteria(string name, string value)
        {
            KeyValueCriteria.Add(name, value);
        }

        public override string ToXml()
        {
            var builder = new StringBuilder();
            builder.Append("<search>");
            foreach (var pair in Criteria)
            {
                builder.AppendFormat("<{0}>{1}</{0}>", pair.Key, pair.Value.ToXml());
            }
            foreach (var pair in RangeCriteria)
            {
                builder.AppendFormat("<{0}>", pair.Key);
                foreach (var criteria in pair.Value)
                {
                    builder.Append(criteria.ToXml());
                }
                builder.AppendFormat("</{0}>", pair.Key);
            }
            foreach (var pair in MultipleValueCriteria)
            {
                builder.AppendFormat("<{0} type=\"array\">{1}</{0}>", pair.Key, pair.Value.ToXml());
            }
            foreach (var pair in KeyValueCriteria)
            {
                builder.Append(BuildXMLElement(pair.Key, pair.Value));
            }

            builder.Append("</search>");
            return builder.ToString();
        }
    }
}
