#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    /// <summary>
    /// The available duration units for <see cref="Subscription"/>
    /// </summary>
    public enum SubscriptionDurationUnit
    {
        /// <summary>
        /// A duration unit used for subscription periods measured in days
        /// </summary>
        DAY,
        /// <summary>
        /// A duration unit used for subscription periods measured in months
        /// </summary>
        MONTH,
        /// <summary>
        /// A placeholder for unrecognized duration units, implemented for future compatibility  
        /// </summary>
        UNRECOGNIZED
    }

    /// <summary>
    /// The possible statuses for <see cref="Subscription"/>
    /// </summary>
    public class SubscriptionStatus
    {
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> is currently active and in good standing
        /// </summary>
        public static SubscriptionStatus ACTIVE = new SubscriptionStatus("Active");
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> has been canceled and will not be billed
        /// </summary>
        public static SubscriptionStatus CANCELED = new SubscriptionStatus("Canceled");
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> is currently active but past due
        /// </summary>
        public static SubscriptionStatus PAST_DUE = new SubscriptionStatus("Past Due");
        /// <summary>
        /// A placeholder for unrecognized subscription statuses, implemented for future compatibility
        /// </summary>
        public static SubscriptionStatus UNRECOGNIZED = new SubscriptionStatus("Unrecognized");

        public static SubscriptionStatus[] STATUSES = {ACTIVE, CANCELED, PAST_DUE};

        private String Name;

        public SubscriptionStatus(String name)
        {
            Name = name;
        }

        public override String ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// A subscription returned by the Braintree Gateway
    /// </summary>
    /// <example>
    /// Subscriptions can be retrieved via the gateway using the associated subscription id:
    /// <code>
    ///     Subscription subscription = gateway.Subscription.Find("subscriptionId");
    /// </code>
    /// For more information about Subscriptions, see <a href="http://www.braintreepaymentsolutions.com/gateway/subscription-api" target="_blank">http://www.braintreepaymentsolutions.com/gateway/subscription-api</a>
    /// </example>
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
        public String MerchantAccountId { get; protected set; }

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
            Status = (SubscriptionStatus)CollectionUtil.Find(SubscriptionStatus.STATUSES, node.GetString("status"), SubscriptionStatus.UNRECOGNIZED);
            HasTrialPeriod = node.GetBoolean("trial-period");
            TrialDuration = node.GetInteger("trial-duration");
            String trialDurationUnitStr = node.GetString("trial-duration-unit");
            if (trialDurationUnitStr != null) {
                TrialDurationUnit = (SubscriptionDurationUnit)EnumUtil.Find(typeof(SubscriptionDurationUnit), trialDurationUnitStr, "unrecognized");
            }
            MerchantAccountId = node.GetString("merchant-account-id");
            Transactions = new List<Transaction> ();
            foreach (NodeWrapper transactionResponse in node.GetList("transactions/transaction")) {
                Transactions.Add(new Transaction(transactionResponse));
            }
        }
        
    }
}
