using System;
using System.Collections.Generic;

namespace Braintree
{
    public class SettlementBatchSummaryGateway
    {
        private BraintreeService service;
        private BraintreeGateway gateway;

        protected internal SettlementBatchSummaryGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            this.gateway = gateway;
            service = new BraintreeService(gateway.Configuration);
        }

        public Result<SettlementBatchSummary> Generate(DateTime settlementDate)
        {
            var request = new SettlementBatchSummaryRequest
            {
                SettlementDate = settlementDate
            };
            return GetSummary(request);
        }

        public Result<SettlementBatchSummary> Generate(DateTime settlementDate, string groupByCustomField)
        {
            var request = new SettlementBatchSummaryRequest
            {
                SettlementDate = settlementDate,
                GroupByCustomField = groupByCustomField
            };
            return GetSummary(request);
        }

        private Result<SettlementBatchSummary> GetSummary(SettlementBatchSummaryRequest request)
        {
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/settlement_batch_summary", request));
            return new ResultImpl<SettlementBatchSummary>(response, gateway);
        }
    }
}

