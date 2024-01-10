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
        public virtual TransactionLineItemKind Kind { get; protected set; }
        public virtual decimal? DiscountAmount { get; protected set; }
        public virtual decimal? Quantity { get; protected set; }
        public virtual decimal? TaxAmount { get; protected set; }
        public virtual decimal? TotalAmount { get; protected set; }
        public virtual decimal? UnitAmount { get; protected set; }
        public virtual decimal? UnitTaxAmount { get; protected set; }
        public virtual string CommodityCode { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string ProductCode { get; protected set; }
        public virtual string UnitOfMeasure { get; protected set; }
        public virtual string UpcCode { get; protected set; }
        public virtual string UpcType { get; protected set; }
        public virtual string Url { get; protected set; }

        protected internal TransactionLineItem(NodeWrapper node)
        {
            CommodityCode = node.GetString("commodity-code");
            Description = node.GetString("description");
            DiscountAmount = node.GetDecimal("discount-amount");
            ImageUrl = node.GetString("image-url");
            Kind = node.GetEnum("kind", TransactionLineItemKind.UNRECOGNIZED);
            Name = node.GetString("name");
            ProductCode = node.GetString("product-code");
            Quantity = node.GetDecimal("quantity");
            TaxAmount = node.GetDecimal("tax-amount");
            TotalAmount = node.GetDecimal("total-amount");
            UnitAmount = node.GetDecimal("unit-amount");
            UnitOfMeasure = node.GetString("unit-of-measure");
            UnitTaxAmount = node.GetDecimal("unit-tax-amount");
            UpcCode = node.GetString("upc-code");
            UpcType = node.GetString("upc-type");
            Url = node.GetString("url");
        }

        [Obsolete("Mock Use Only")]
        protected internal TransactionLineItem() { }
    }
}
