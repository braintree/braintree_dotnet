namespace Braintree
{
    public class AmountBreakdownRequest : Request
    {
        public decimal Discount { get; set; }
        public decimal Handling { get; set; }
        public decimal Insurance { get; set; }
        public decimal ItemTotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal ShippingDiscount { get; set; }
        public decimal TaxTotal { get; set; }

        public override string ToXml()
        {
            return ToXml("amount-breakdown");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("amount-breakdown");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("discount", Discount).
                AddElement("handling", Handling).
                AddElement("insurance", Insurance).
                AddElement("item-total", ItemTotal).
                AddElement("shipping", Shipping).
                AddElement("shipping-discount", ShippingDiscount).
                AddElement("tax-total", TaxTotal);
        }
    }
}