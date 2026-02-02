namespace Braintree
{
    public class TransactionOptionsUsBankAccountRequest : Request
    {
        public string AchType { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        private RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("ach-type", AchType);
        }
    }
}

