#pragma warning disable 1591

using System;
using System.Threading.Tasks;

namespace Braintree
{
    public interface ISettlementBatchSummaryGateway
    {
        Result<SettlementBatchSummary> Generate(DateTime settlementDate);
        Task<Result<SettlementBatchSummary>> GenerateAsync(DateTime settlementDate);
        Result<SettlementBatchSummary> Generate(DateTime settlementDate, string groupByCustomField);
        Task<Result<SettlementBatchSummary>> GenerateAsync(DateTime settlementDate, string groupByCustomField);
    }
}
