using System;
using System.Collections.Generic;

namespace Braintree
{
    public class SettlementBatchSummaryGateway
    {

        private BraintreeService Service;

        protected internal SettlementBatchSummaryGateway(BraintreeService service)
        {
            Service = service;
        }

        public Result<SettlementBatchSummary> Generate(DateTime settlementDate)
        {
            SettlementBatchSummaryRequest request = new SettlementBatchSummaryRequest
            {
                SettlementDate = settlementDate
            };
            return GetSummary(request);
        }

        public Result<SettlementBatchSummary> Generate(DateTime settlementDate, string groupByCustomField)
        {
            SettlementBatchSummaryRequest request = new SettlementBatchSummaryRequest
            {
                SettlementDate = settlementDate,
                GroupByCustomField = groupByCustomField
            };
            return GetSummary(request);
        }

        private Result<SettlementBatchSummary> GetSummary(SettlementBatchSummaryRequest request)
        {
            NodeWrapper response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/settlement_batch_summary", request));
            return new ResultImpl<SettlementBatchSummary>(response, Service);
        }
    }
}

