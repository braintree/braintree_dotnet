using System;
using System.Threading.Tasks;

namespace Braintree
{
    public class SettlementBatchSummaryGateway : ISettlementBatchSummaryGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

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

        public Task<Result<SettlementBatchSummary>> GenerateAsync(DateTime settlementDate)
        {
            var request = new SettlementBatchSummaryRequest
            {
                SettlementDate = settlementDate
            };
            return GetSummaryAsync(request);
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

        public Task<Result<SettlementBatchSummary>> GenerateAsync(DateTime settlementDate, string groupByCustomField)
        {
            var request = new SettlementBatchSummaryRequest
            {
                SettlementDate = settlementDate,
                GroupByCustomField = groupByCustomField
            };
            return GetSummaryAsync(request);
        }

        private Result<SettlementBatchSummary> GetSummary(SettlementBatchSummaryRequest request)
        {
            var response = new NodeWrapper(service.Post(service.MerchantPath() + "/settlement_batch_summary", request));
            return new ResultImpl<SettlementBatchSummary>(response, gateway);
        }

        private async Task<Result<SettlementBatchSummary>> GetSummaryAsync(SettlementBatchSummaryRequest request)
        {
            var response = new NodeWrapper(await service.PostAsync(service.MerchantPath() + "/settlement_batch_summary", request).ConfigureAwait(false));
            return new ResultImpl<SettlementBatchSummary>(response, gateway);
        }
    }
}

