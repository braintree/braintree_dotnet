namespace Braintree
{
    public class TransactionPayPalRequest : Request
    {
        public string PayeeId { get; set; }
        public string PayeeEmail { get; set; }

        public override string ToXml()
        {
            return ToXml("paypal-account");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("paypal-account");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);
            builder.AddElement("payee-id", PayeeId);
            builder.AddElement("payee-email", PayeeEmail);
            return builder;
        }
    }
}
