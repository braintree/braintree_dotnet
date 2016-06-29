using System;

namespace Braintree
{
    public class AccountUpdaterDailyReport
    {
        public virtual DateTime? ReportDate { get; protected set; }
        public virtual string ReportUrl { get; protected set; }

        protected internal AccountUpdaterDailyReport(NodeWrapper node)
        {
            ReportDate = node.GetDateTime("report-date");
            ReportUrl = node.GetString("report-url");
        }

        [Obsolete("Mock Use Only")]
        protected internal AccountUpdaterDailyReport() { }
    }
}

