namespace Braintree
{
    public class TransactionOptionsVenmoRequest : Request
    {
        public string ProfileId { get; set; }

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
                AddElement("profile-id", ProfileId);
        }
    }
}
