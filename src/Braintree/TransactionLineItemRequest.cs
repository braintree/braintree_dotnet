#pragma warning disable 1591

using System.Collections.Generic;

namespace Braintree
{
    /// <summary>
    /// A class for building requests to manipulate <see cref="TransactionLineItem"/> records.
    /// </summary>
    /// <example>
    /// A transaction line item request can be constructed as follows:
    /// <code>
    /// var request = new TransactionLineItemRequest
    /// {
    ///     Quantity = 5,
    ///     Name = "Water bottle",
    ///     LineItemKind = TransactionLineItemKind.DEBIT,
    ///     UnitAmount = 2.00M,
    ///     TotalAmount = 10.00M
    /// };
    /// </code>
    /// </example>
    public class TransactionLineItemRequest : Request
    {
        public virtual decimal? Quantity { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual TransactionLineItemKind LineItemKind { get; set; }
        public virtual decimal? UnitAmount { get; set; }
        public virtual decimal? UnitTaxAmount { get; set; }
        public virtual decimal? TotalAmount { get; set; }
        public virtual decimal? DiscountAmount { get; set; }
        public virtual decimal? TaxAmount { get; set; }
        public virtual string UnitOfMeasure { get; set; }
        public virtual string ProductCode { get; set; }
        public virtual string CommodityCode { get; set; }
        public virtual string Url { get; set; }

        public TransactionLineItemRequest() {}

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);

            builder.AddElement("quantity", Quantity);
            builder.AddElement("name", Name);
            builder.AddElement("description", Description);
            builder.AddElement("kind", LineItemKind);
            builder.AddElement("unit-amount", UnitAmount);
            builder.AddElement("unit-tax-amount", UnitTaxAmount);
            builder.AddElement("total-amount", TotalAmount);
            builder.AddElement("discount-amount", DiscountAmount);
            builder.AddElement("tax-amount", TaxAmount);
            builder.AddElement("unit-of-measure", UnitOfMeasure);
            builder.AddElement("product-code", ProductCode);
            builder.AddElement("commodity-code", CommodityCode);
            builder.AddElement("url", Url);
            return builder;
        }
    }
}
