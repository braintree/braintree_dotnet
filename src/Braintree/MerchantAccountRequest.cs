namespace Braintree
{
    public class MerchantAccountRequest : Request
    {
        public string Id { get; set; }

        public override string ToXml()
        {
            return ToXml("merchant-account");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString()
        {
            return ToQueryString("merchant-account");
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("id", Id);

        }
    }
}
