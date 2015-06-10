using System;
using System.Collections.Generic;

namespace Braintree
{
    public class SettlementBatchSummaryGateway
    {

        private BraintreeService Service;
        private BraintreeGateway Gateway;

        protected internal SettlementBatchSummaryGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasAccessTokenOrKeys();
            Gateway = gateway;
            Service = new BraintreeService(gateway.Configuration);
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
            var response = new NodeWrapper(Service.Post(Service.MerchantPath() + "/settlement_batch_summary", request));
            return new ResultImpl<SettlementBatchSummary>(response, Gateway);
        }
    }
}

