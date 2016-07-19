#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Threading;

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
        public string GroupByCustomField { get; set; }

        public override string ToXml()
        {
            return BuildRequest("settlement-batch-summary").ToXml();
        }

        public virtual RequestBuilder BuildRequest(string root)
        {
            CultureInfo originalCulture = CultureInfo.CurrentCulture;
#if netcoreapp10
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
#else
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
#endif
      
            var builder = new RequestBuilder(root);
            builder.AddElement("settlement-date", SettlementDate.ToString("d"));

#if netcoreapp10
            CultureInfo.CurrentCulture = originalCulture;
#else
            Thread.CurrentThread.CurrentCulture = originalCulture;
#endif

            if (GroupByCustomField != null)
            {
                builder.AddElement("group-by-custom-field", GroupByCustomField);
            }
            return builder;
        }
    }
}
