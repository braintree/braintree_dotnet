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
                var row = new Dictionary<String, String>();

                row["MerchantAccountId"] = result.GetString("merchant-account-id");
                row["CardType"] = result.GetString("card-type");
                row["Kind"] = result.GetString("kind");
                row["AmountSettled"] = result.GetString("amount-settled");
                row["Count"] = result.GetString("count");
                results.Add(row);
            }
        }

        public IList<IDictionary<String, String>> Results
        {
            get { return results; }
        }
    }
}

