using System;
using System.Collections.Generic;

namespace Braintree
{
    public class SettlementBatchSummary
    {
        private IList<IDictionary<String, String>> results;

        internal SettlementBatchSummary (NodeWrapper node)
        {
            results = new List<IDictionary<String, String>>();

            foreach (var result in node.GetList("records/record"))
            {
                results.Add(result.GetDictionary("."));
            }
        }

        public IList<IDictionary<String, String>> Results
        {
            get { return results; }
        }
    }
}

