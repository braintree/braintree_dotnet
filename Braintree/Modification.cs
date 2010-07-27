#pragma warning disable 1591

using System;

namespace Braintree
{
    public class Modification
    {
        public Boolean? NeverExpires { get; protected set; }
        public Decimal? Amount { get; protected set; }
        public Int32? NumberOfBillingCycles { get; protected set; }
        public Int32? Quantity { get; protected set; }
        public string Id { get; protected set; }

        internal Modification(NodeWrapper node)
        {
            Amount = node.GetDecimal("amount");
            Id = node.GetString("id");
            NeverExpires = node.GetBoolean("never-expires");
            NumberOfBillingCycles = node.GetInteger("number-of-billing-cycles");
            Quantity = node.GetInteger("quantity");
        }
    }
}
