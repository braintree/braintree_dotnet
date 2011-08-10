using System;
using System.Collections.Generic;

namespace Braintree
{
    public class SettlementBatchSummaryGateway
    {

        private BraintreeService Service;

        internal SettlementBatchSummaryGateway(BraintreeService service)
        {
            Service = service;
        }

        public Result<SettlementBatchSummary> Generate(DateTime settlementDate)
        {
            var queryString = new QueryString();
            queryString.Append("settlement_date", settlementDate.ToShortDateString());
            NodeWrapper response = new NodeWrapper(Service.Get(String.Format("/settlement_batch_summary?{0}", queryString.ToString())));
            return new Result<SettlementBatchSummary>(response, Service);
        }
    }
}

