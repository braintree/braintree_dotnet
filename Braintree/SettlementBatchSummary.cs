using System;
using System.Collections.Generic;

namespace Braintree
{
    [Serializable]
    public class SettlementBatchSummary
    {
        private IList<IDictionary<String, String>> records;

        protected internal SettlementBatchSummary (NodeWrapper node)
        {
            records = new List<IDictionary<String, String>>();

            foreach (var record in node.GetList("records/record"))
            {
                records.Add(record.GetDictionary("."));
            }
        }

        public IList<IDictionary<String, String>> Records
        {
            get { return records; }
        }
    }
}

