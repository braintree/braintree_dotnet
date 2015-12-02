#pragma warning disable 1591

using System;

namespace Braintree
{
    public class Modification
    {
        public virtual decimal? Amount { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual int? CurrentBillingCycle { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual string Kind { get; protected set; }
        public virtual string MerchantId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual bool? NeverExpires { get; protected set; }
        public virtual int? NumberOfBillingCycles { get; protected set; }
        public virtual int? Quantity { get; protected set; }
        public virtual DateTime? UpdatedAt { get; protected set; }

        protected Modification(NodeWrapper node)
        {
            Amount = node.GetDecimal("amount");
            CreatedAt = node.GetDateTime("created-at");
            CurrentBillingCycle = node.GetInteger("current-billing-cycle");
            Description = node.GetString("description");
            Id = node.GetString("id");
            Kind = node.GetString("kind");
            MerchantId = node.GetString("merchant-id");
            Name = node.GetString("name");
            NeverExpires = node.GetBoolean("never-expires");
            NumberOfBillingCycles = node.GetInteger("number-of-billing-cycles");
            Quantity = node.GetInteger("quantity");
            UpdatedAt = node.GetDateTime("updated-at");
        }

        [Obsolete("Mock Use Only")]
        protected Modification() { }
    }
}
