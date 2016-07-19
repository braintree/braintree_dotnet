using System;
using System.Collections.Generic;

namespace Braintree
{
    public class SettlementBatchSummary
    {
        private IList<IDictionary<string, string>> records;

        protected internal SettlementBatchSummary (NodeWrapper node)
        {
            records = new List<IDictionary<string, string>>();

            foreach (var record in node.GetList("records/record"))
            {
                records.Add(record.GetDictionary("."));
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal SettlementBatchSummary() { }

        public virtual IList<IDictionary<string, string>> Records
        {
            get { return records; }
        }
    }
}

