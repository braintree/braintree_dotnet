using System;
using System.Collections.Generic;

namespace Braintree
{
    public class SettlementBatchSummary
    {
        private IList<IDictionary<String, String>> records;

        internal SettlementBatchSummary (NodeWrapper node)
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

