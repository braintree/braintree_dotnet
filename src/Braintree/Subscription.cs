#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        [Description("day")] DAY,
        /// <summary>
        /// A duration unit used for subscription periods measured in months
        /// </summary>
        [Description("month")] MONTH,
        /// <summary>
        /// A placeholder for unrecognized duration units, implemented for future compatibility
        /// </summary>
        [Description("unrecognized")] UNRECOGNIZED
    }

    /// <summary>
    /// The possible statuses for <see cref="Subscription"/>
    /// </summary>
    public enum SubscriptionStatus
    {
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> is currently active and in good standing
        /// </summary>
        [Description("Active")] ACTIVE,
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> has been canceled and will not be billed
        /// </summary>
        [Description("Canceled")] CANCELED,
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> has reached the end of the specified billing cycles
        /// </summary>
        [Description("Expired")] EXPIRED,
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> is currently active but past due
        /// </summary>
        [Description("Past Due")] PAST_DUE,
        /// <summary>
        /// Indicates that the <see cref="Subscription"/> is currently pending
        /// </summary>
        [Description("Pending")] PENDING,
        /// <summary>
        /// A placeholder for unrecognized subscription statuses, implemented for future compatibility
        /// </summary>
        [Description("Unrecognized")] UNRECOGNIZED
    }

    public enum SubscriptionSource
    {
        [Description("api")] API,
        [Description("control_panel")] CONTROL_PANEL,
        [Description("recurring")] RECURRING,
        [Description("unrecognized")] UNRECOGNIZED
    }

    /// <summary>
    /// A subscription returned by the Braintree Gateway
    /// </summary>
    /// <example>
    /// Subscriptions can be retrieved via the gateway using the associated subscription id:
    /// <code>
    ///     Subscription subscription = gateway.Subscription.Find("subscriptionId");
    /// </code>
    /// For more information about Subscriptions, see <a href="https://developers.braintreepayments.com/reference/response/subscription/dotnet" target="_blank">https://developers.braintreepayments.com/reference/response/subscription/dotnet</a>
    /// </example>
    public class Subscription
    {
        public virtual decimal? Balance { get; protected set; }
        public virtual List<AddOn> AddOns { get; protected set; }
        public virtual int? BillingDayOfMonth { get; protected set; }
        public virtual DateTime? BillingPeriodEndDate { get; protected set; }
        public virtual DateTime? BillingPeriodStartDate { get; protected set; }
        public virtual int? CurrentBillingCycle { get; protected set; }
        public virtual int? DaysPastDue { get; protected set; }
        public virtual Descriptor Descriptor { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual List<Discount> Discounts { get; protected set; }
        public virtual int? FailureCount { get; protected set; }
        public virtual DateTime? FirstBillingDate { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }
        public virtual bool? HasTrialPeriod { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual bool? NeverExpires { get; protected set; }
        public virtual decimal? NextBillAmount { get; protected set; }
        public virtual DateTime? NextBillingDate { get; protected set; }
        public virtual decimal? NextBillingPeriodAmount { get; protected set; }
        public virtual int? NumberOfBillingCycles { get; protected set; }
        public virtual DateTime? PaidThroughDate { get; protected set; }
        public virtual string PaymentMethodToken { get; protected set; }
        public virtual string PlanId { get; protected set; }
        public virtual decimal? Price { get; protected set; }
        public virtual SubscriptionStatusEvent[] StatusHistory { get; protected set; }
        public virtual SubscriptionStatus Status { get; protected set; }
        public virtual List<Transaction> Transactions { get; protected set; }
        public virtual int? TrialDuration { get; protected set; }
        public virtual SubscriptionDurationUnit TrialDurationUnit { get; protected set; }
        public virtual string MerchantAccountId { get; protected set; }

        public Subscription(NodeWrapper node, IBraintreeGateway gateway)
        {
            Balance = node.GetDecimal("balance");
            BillingDayOfMonth = node.GetInteger("billing-day-of-month");
            BillingPeriodEndDate = node.GetDateTime("billing-period-end-date");
            BillingPeriodStartDate = node.GetDateTime("billing-period-start-date");
            CurrentBillingCycle = node.GetInteger("current-billing-cycle");
            DaysPastDue = node.GetInteger("days-past-due");
            Descriptor = new Descriptor(node.GetNode("descriptor"));
            Description = node.GetString("description");
            FailureCount = node.GetInteger("failure-count");
            FirstBillingDate = node.GetDateTime("first-billing-date");
            CreatedAt = node.GetDateTime("created-at");
            UpdatedAt = node.GetDateTime("updated-at");
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
            Status = node.GetEnum("status", SubscriptionStatus.UNRECOGNIZED);
            List<NodeWrapper> statusNodes = node.GetList("status-history/status-event");
            StatusHistory = new SubscriptionStatusEvent[statusNodes.Count];
            for (int i = 0; i < statusNodes.Count; i++)
            {
                StatusHistory[i] = new SubscriptionStatusEvent(statusNodes[i]);
            }
            HasTrialPeriod = node.GetBoolean("trial-period");
            TrialDuration = node.GetInteger("trial-duration");
            TrialDurationUnit = node.GetEnum("trial-duration-unit", SubscriptionDurationUnit.UNRECOGNIZED);
            MerchantAccountId = node.GetString("merchant-account-id");

            AddOns = new List<AddOn> ();
            foreach (var addOnResponse in node.GetList("add-ons/add-on"))
            {
                AddOns.Add(new AddOn(addOnResponse));
            }
            Discounts = new List<Discount> ();
            foreach (var discountResponse in node.GetList("discounts/discount"))
            {
                Discounts.Add(new Discount(discountResponse));
            }
            Transactions = new List<Transaction> ();
            foreach (var transactionResponse in node.GetList("transactions/transaction"))
            {
                Transactions.Add(new Transaction(transactionResponse, gateway));
            }
        }

        protected internal Subscription() { }
    }
}
