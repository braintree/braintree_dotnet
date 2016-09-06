namespace Braintree
{
    public class MerchantAccountRequest : Request
    {
        public string Id { get; set; }
        public ApplicantDetailsRequest ApplicantDetails { get; set; }
        public BusinessRequest Business { get; set; }
        public FundingRequest Funding { get; set; }
        public IndividualRequest Individual { get; set; }
        public bool? TosAccepted { get; set; }
        public string MasterMerchantAccountId { get; set; }

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
                AddElement("id", Id).
                AddElement("applicant-details", ApplicantDetails).
                AddElement("individual", Individual).
                AddElement("funding", Funding).
                AddElement("business", Business).
                AddElement("tos-accepted", TosAccepted).
                AddElement("master-merchant-account-id", MasterMerchantAccountId);
        }
    }
}
