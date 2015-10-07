using System;

namespace Braintree
{
    public interface ISettlementBatchSummaryGateway
    {
        Result<SettlementBatchSummary> Generate(DateTime settlementDate);
        Result<SettlementBatchSummary> Generate(DateTime settlementDate, string groupByCustomField);
    }
}