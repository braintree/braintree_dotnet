namespace Braintree
{
    public class UsBankAccountRequest : Request
    {
        public string Token { get; set; }
        public TransactionOptionsRequest Options { get; set; }

        public override string ToXml()
        {
            return ToXml("us-bank-account");
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
