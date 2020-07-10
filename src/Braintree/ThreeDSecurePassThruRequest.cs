#pragma warning disable 1591

namespace Braintree
{
    public class ThreeDSecurePassThruRequest : Request
    {
        public string EciFlag { get; set; }
        public string Cavv { get; set; }
        public string Xid { get; set; }
        public string AuthenticationResponse { get; set; }
        public string DirectoryResponse { get; set; }
        public string CavvAlgorithm { get; set; }
        public string DsTransactionId { get; set; }
        public string ThreeDSecureVersion { get; set; }

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
                AddElement("xid", Xid).
                AddElement("three-d-secure-version", ThreeDSecureVersion).
                AddElement("authentication_response", AuthenticationResponse).
                AddElement("directory_response", DirectoryResponse).
                AddElement("cavv_algorithm", CavvAlgorithm).
                AddElement("ds_transaction_id", DsTransactionId);
        }
    }
}
