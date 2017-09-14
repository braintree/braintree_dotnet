#pragma warning disable 1591

namespace Braintree
{
    public class SubscriptionTransactionOptionsRequest : Request
    {
        public bool? SubmitForSettlement { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("submit-for-settlement", SubmitForSettlement);
        }
    }
}
