#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public abstract class SearchRequest : Request
    {
        private Dictionary<String, List<SearchCriteria>> Criteria;
        private Dictionary<String, SearchCriteria> MultipleValueCriteria;
        private Dictionary<String, String> KeyValueCriteria;

        protected SearchRequest()
        {
            Criteria = new Dictionary<String, List<SearchCriteria>>();
            MultipleValueCriteria = new Dictionary<String, SearchCriteria>();
            KeyValueCriteria = new Dictionary<String, String>();
        }

        internal virtual void AddCriteria(String name, SearchCriteria criteria)
        {
            if (!Criteria.ContainsKey(name))
            {
                Criteria.Add(name, new List<SearchCriteria>());
            }
            Criteria[name].Add(criteria);
        }

        internal virtual void AddMultipleValueCriteria(String name, SearchCriteria criteria)
        {
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
            foreach (KeyValuePair<String, List<SearchCriteria>> pair in Criteria)
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
