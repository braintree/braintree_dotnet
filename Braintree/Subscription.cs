#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    /// <summary>
    /// The available duration units for <see cref="Subscription"/>
    /// </summary>
    public class SubscriptionDurationUnit : Enumeration
    {
        /// <summary>
        /// A duration unit used for subscription periods measured in days
        /// </summary>
        public static readonly SubscriptionDurationUnit DAY = new SubscriptionDurationUnit("day");
        /// <summary>
        /// A duration unit used for subscription periods measured in months
        /// </summary>
        public static readonly SubscriptionDurationUnit MONTH = new SubscriptionDurationUnit("month");
        /// <summary>
        /// A placeholder for unrecognized duration units, implemented for future compatibility
        /// </summary>
        public static readonly SubscriptionDurationUnit UNRECOGNIZED = new SubscriptionDurationUnit("unrecognized");

        public static readonly SubscriptionDurationUnit[] ALL = { DAY, MONTH };

        protected SubscriptionDurationUnit(String name) : base(name) {}
    }

    /// <summary>
    /// The possible statuses for <see cref="Subscription"/>
    /// </summary>
    public class SubscriptionStatus : Enumeration
    {
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> is currently active and in good standing
        /// </summary>
        public static readonly SubscriptionStatus ACTIVE = new SubscriptionStatus("Active");
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> has been canceled and will not be billed
        /// </summary>
        public static readonly SubscriptionStatus CANCELED = new SubscriptionStatus("Canceled");
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> has reached the end of the specified billing cycles
        /// </summary>
        public static readonly SubscriptionStatus EXPIRED = new SubscriptionStatus("Expired");
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> is currently active but past due
        /// </summary>
        public static readonly SubscriptionStatus PAST_DUE = new SubscriptionStatus("Past Due");
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> is currently pending
        /// </summary>
        public static readonly SubscriptionStatus PENDING = new SubscriptionStatus("Pending");
        /// <summary>
        /// A placeholder for unrecognized subscription statuses, implemented for future compatibility
        /// </summary>
        public static readonly SubscriptionStatus UNRECOGNIZED = new SubscriptionStatus("Unrecognized");

        public static readonly SubscriptionStatus[] STATUSES = {ACTIVE, CANCELED, EXPIRED, PAST_DUE, PENDING};

        protected SubscriptionStatus(String name) : base(name) {}
    }

    /// <summary>
    /// A subscription returned by the Braintree Gateway
    /// </summary>
    /// <example>
    /// Subscriptions can be retrieved via the gateway using the associated subscription id:
    /// <code>
    ///     Subscription subscription = gateway.Subscription.Find("subscriptionId");
    /// </code>
    /// For more information about Subscriptions, see <a href="http://www.braintreepayments.com/gateway/subscription-api" target="_blank">http://www.braintreepaymentsolutions.com/gateway/subscription-api</a>
    /// </example>
    public class Subscription
    {
        public Decimal? Balance { get; protected set; }
        public List<AddOn> AddOns { get; protected set; }
        public Int32? BillingDayOfMonth { get; protected set; }
        public DateTime? BillingPeriodEndDate { get; protected set; }
        public DateTime? BillingPeriodStartDate { get; protected set; }
        public Int32? DaysPastDue { get; protected set; }
        public Descriptor Descriptor { get; protected set; }
        public List<Discount> Discounts { get; protected set; }
        public Int32? FailureCount { get; protected set; }
        public DateTime? FirstBillingDate { get; protected set; }
        public Boolean? HasTrialPeriod { get; protected set; }
        public String Id { get; protected set; }
        public Boolean? NeverExpires { get; protected set; }
        public Decimal? NextBillAmount { get; protected set; }
        public DateTime? NextBillingDate { get; protected set; }
        public Decimal? NextBillingPeriodAmount { get; protected set; }
        public Int32? NumberOfBillingCycles { get; protected set; }
        public DateTime? PaidThroughDate { get; protected set; }
        public String PaymentMethodToken { get; protected set; }
        public String PlanId { get; protected set; }
        public Decimal? Price { get; protected set; }
        public SubscriptionStatus Status { get; protected set; }
        public List<Transaction> Transactions { get; protected set; }
        public Int32? TrialDuration { get; protected set; }
        public SubscriptionDurationUnit TrialDurationUnit { get; protected set; }
        public String MerchantAccountId { get; protected set; }

        public Subscription(NodeWrapper node, BraintreeService service)
        {
            Balance = node.GetDecimal("balance");
            BillingDayOfMonth = node.GetInteger("billing-day-of-month");
            BillingPeriodEndDate = node.GetDateTime("billing-period-end-date");
            BillingPeriodStartDate = node.GetDateTime("billing-period-start-date");
            DaysPastDue = node.GetInteger("days-past-due");
            Descriptor = new Descriptor(node.GetNode("descriptor"));
            FailureCount = node.GetInteger("failure-count");
            FirstBillingDate = node.GetDateTime("first-billing-date");
            Id = node.GetString("id");
            NextBillAmount = node.GetDecimal("next-bill-amount");
            NextBillingDate = node.GetDateTime("next-billing-date");
            NextBillingPeriodAmount = node.GetDecimal("next-billing-period-amount");
            NeverExpires = node.GetBoolean("never-expires");
            NumberOfBillingCycles = node.GetInteger("number-of-billing-cycles");
            PaymentMethodToken = node.GetString("payment-method-token");
            PaidThroughDate = node.GetDateTime("paid-through-date");
            PlanId = node.GetString("plan-id");
            Price = node.GetDecimal("price");
            Status = (SubscriptionStatus)CollectionUtil.Find(SubscriptionStatus.STATUSES, node.GetString("status"), SubscriptionStatus.UNRECOGNIZED);
            HasTrialPeriod = node.GetBoolean("trial-period");
            TrialDuration = node.GetInteger("trial-duration");
            String trialDurationUnitStr = node.GetString("trial-duration-unit");
            if (trialDurationUnitStr != null) {
                TrialDurationUnit = (SubscriptionDurationUnit)CollectionUtil.Find(SubscriptionDurationUnit.ALL, trialDurationUnitStr, SubscriptionDurationUnit.UNRECOGNIZED);
            }
            MerchantAccountId = node.GetString("merchant-account-id");

            AddOns = new List<AddOn> ();
            foreach (NodeWrapper addOnResponse in node.GetList("add-ons/add-on")) {
                AddOns.Add(new AddOn(addOnResponse));
            }
            Discounts = new List<Discount> ();
            foreach (NodeWrapper discountResponse in node.GetList("discounts/discount")) {
                Discounts.Add(new Discount(discountResponse));
            }
            Transactions = new List<Transaction> ();
            foreach (NodeWrapper transactionResponse in node.GetList("transactions/transaction")) {
                Transactions.Add(new Transaction(transactionResponse, service));
            }
        }
    }
}
