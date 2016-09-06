#pragma warning disable 1591

namespace Braintree
{
    public class TransactionThreeDSecurePassThruRequest : Request
    {
        public string EciFlag { get; set; }
        public string Cavv { get; set; }
        public string Xid { get; set; }

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
            return new RequestBuilder(root).
                AddElement("eci-flag", EciFlag).
                AddElement("cavv", Cavv).
                AddElement("xid", Xid);
        }
    }
}
