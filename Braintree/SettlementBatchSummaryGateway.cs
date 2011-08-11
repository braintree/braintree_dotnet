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
            return GetSummary(queryString);
        }

        public Result<SettlementBatchSummary> Generate(DateTime settlementDate, string groupByCustomField)
        {
            var queryString = new QueryString();
            queryString.Append("settlement_date", settlementDate.ToShortDateString());
            queryString.Append("group_by_custom_field", groupByCustomField);
            return GetSummary(queryString);
        }

        private Result<SettlementBatchSummary> GetSummary(QueryString queryString)
        {
            NodeWrapper response = new NodeWrapper(Service.Get(String.Format("/settlement_batch_summary?{0}", queryString.ToString())));
            return new Result<SettlementBatchSummary>(response, Service);
        }
    }
}

