using System;
using System.ComponentModel;

namespace Braintree
{
    public enum TransactionLineItemKind
    {
        [Description("credit")] CREDIT,
        [Description("debit")] DEBIT,
        [Description("unrecognized")] UNRECOGNIZED
    }

    public class TransactionLineItem
    {
        public virtual decimal? Quantity { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual TransactionLineItemKind Kind { get; protected set; }
        public virtual decimal? UnitAmount { get; protected set; }
        public virtual decimal? UnitTaxAmount { get; protected set; }
        public virtual decimal? TotalAmount { get; protected set; }
        public virtual decimal? DiscountAmount { get; protected set; }
        public virtual decimal? TaxAmount { get; protected set; }
        public virtual string UnitOfMeasure { get; protected set; }
        public virtual string ProductCode { get; protected set; }
        public virtual string CommodityCode { get; protected set; }
        public virtual string Url { get; protected set; }

        protected internal TransactionLineItem(NodeWrapper node)
        {
            Quantity = node.GetDecimal("quantity");
            Name = node.GetString("name");
            Description = node.GetString("description");
            Kind = node.GetEnum("kind", TransactionLineItemKind.UNRECOGNIZED);
            UnitAmount = node.GetDecimal("unit-amount");
            UnitTaxAmount = node.GetDecimal("unit-tax-amount");
            TotalAmount = node.GetDecimal("total-amount");
            DiscountAmount = node.GetDecimal("discount-amount");
            TaxAmount = node.GetDecimal("tax-amount");
            UnitOfMeasure = node.GetString("unit-of-measure");
            ProductCode = node.GetString("product-code");
            CommodityCode = node.GetString("commodity-code");
            Url = node.GetString("url");
        }

        [Obsolete("Mock Use Only")]
        protected internal TransactionLineItem() { }
    }
}
