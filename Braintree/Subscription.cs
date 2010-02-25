using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public enum SubscriptionDurationUnit
    {
        DAY, MONTH, UNRECOGNIZED
    }

    public enum SubscriptionStatus
    {
        ACTIVE,
        CANCELED,
        PAST_DUE,
        UNRECOGNIZED
    }

    public class Subscription
    {
        public DateTime? BillingPeriodEndDate { get; protected set; }
        public DateTime? BillingPeriodStartDate { get; protected set; }
        public Int32? FailureCount { get; protected set; }
        public DateTime? FirstBillingDate { get; protected set; }
        public Boolean? HasTrialPeriod { get; protected set; }
        public String Id { get; protected set; }
        public DateTime? NextBillingDate { get; protected set; }
        public String PaymentMethodToken { get; protected set; }
        public String PlanId { get; protected set; }
        public Decimal? Price { get; protected set; }
        public SubscriptionStatus Status { get; protected set; }
        public List<Transaction> Transactions { get; protected set; }
        public Int32? TrialDuration { get; protected set; }
        public SubscriptionDurationUnit TrialDurationUnit { get; protected set; }

        public Subscription(NodeWrapper node)
        {
            BillingPeriodEndDate = node.GetDateTime("billing-period-end-date");
            BillingPeriodStartDate = node.GetDateTime("billing-period-start-date");
            FailureCount = node.GetInteger("failure-count");
            FirstBillingDate = node.GetDateTime("first-billing-date");
            Id = node.GetString("id");
            NextBillingDate = node.GetDateTime("next-billing-date");
            PaymentMethodToken = node.GetString("payment-method-token");
            PlanId = node.GetString("plan-id");
            Price = node.GetDecimal("price");
            Status = (SubscriptionStatus)EnumUtil.Find(typeof(SubscriptionStatus), node.GetString("status"), "unrecognized");
            HasTrialPeriod = node.GetBoolean("trial-period");
            TrialDuration = node.GetInteger("trial-duration");
            String trialDurationUnitStr = node.GetString("trial-duration-unit");
            if (trialDurationUnitStr != null)
            {
                TrialDurationUnit = (SubscriptionDurationUnit)EnumUtil.Find(typeof(SubscriptionDurationUnit), trialDurationUnitStr, "unrecognized");
            }
            Transactions = new List<Transaction>();
            foreach (NodeWrapper transactionResponse in node.GetList("transactions/transaction"))
            {
                Transactions.Add(new Transaction(transactionResponse));
            }
        }

    }
}
