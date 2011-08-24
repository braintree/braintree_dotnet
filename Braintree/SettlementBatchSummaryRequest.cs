#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    /// <summary>
    /// A class for building requests to pull settlement batch result summaries.
    /// </summary>
    /// <example>
    /// A settlement batch summary request can be constructed as follows:
    /// <code>
    /// SettlementBatchSummaryRequest request = new SettlementBatchSummaryRequest
    /// {
    ///     SettlementDate = DateTime,
    ///     GroupByCustomField = 'my_custom_field'
    /// };
    /// </code>
    /// </example>

    class SettlementBatchSummaryRequest : Request
    {
        public DateTime SettlementDate { get; set; }
        public String GroupByCustomField { get; set; }

        public override String ToXml()
        {
            return BuildRequest("settlement-batch-summary").ToXml();
        }

        public virtual RequestBuilder BuildRequest(String root)
        {
            var builder = new RequestBuilder(root);
            builder.AddElement("settlement-date", SettlementDate.ToShortDateString());
            if (GroupByCustomField != null)
            {
                builder.AddElement("group-by-custom-field", GroupByCustomField);
            }
            return builder;
        }
    }
}
