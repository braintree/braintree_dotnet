namespace Braintree
{
    public class PayPalAccountRequest : Request
    {
        public string Token { get; set; }
        public PayPalOptionsRequest Options { get; set; }

        public override string ToXml()
        {
            return ToXml("paypal-account");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("options", Options).
                AddElement("token", Token);
        }
    }
}
