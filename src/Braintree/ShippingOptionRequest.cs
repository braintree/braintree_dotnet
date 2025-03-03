namespace Braintree
{
    public class ShippingOptionRequest : Request
    {
        public decimal Amount { get; set; }
        public string Id { get; set; }
        public string Label { get; set; }
        public bool? Selected { get; set; }
        public string Type { get; set; }

        public override string ToXml()
        {
            return ToXml("shipping-option");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("shipping-option");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("amount", Amount).
                AddElement("id", Id).
                AddElement("label", Label).
                AddElement("selected", Selected).
                AddElement("type", Type);
        }
    }
}