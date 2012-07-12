#pragma warning disable 1591

using System;

namespace Braintree
{
    [Serializable]
    public class Modification
    {
        public Decimal? Amount { get; protected set; }
        public DateTime? CreatedAt { get; protected set; }
        public String Description { get; protected set; }
        public String Id { get; protected set; }
        public String Kind { get; protected set; }
        public String MerchantId { get; protected set; }
        public String Name { get; protected set; }
        public Boolean? NeverExpires { get; protected set; }
        public Int32? NumberOfBillingCycles { get; protected set; }
        public Int32? Quantity { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }

        protected Modification(NodeWrapper node)
        {
            Amount = node.GetDecimal("amount");
            CreatedAt = node.GetDateTime("created-at");
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
    }
}
