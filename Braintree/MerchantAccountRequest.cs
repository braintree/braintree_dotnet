using System;

namespace Braintree
{
  public class MerchantAccountRequest : Request
  {
    public string Id { get; set; }
    public ApplicantDetailsRequest ApplicantDetails { get; set; }
    public BusinessRequest Business { get; set; }
    public FundingRequest Funding { get; set; }
    public IndividualRequest Individual { get; set; }
    public Boolean? TosAccepted { get; set; }
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
      RequestBuilder builder = new RequestBuilder(root);

      builder.AddElement("id", Id);
      builder.AddElement("applicant-details", ApplicantDetails);
      builder.AddElement("individual", Individual);
      builder.AddElement("funding", Funding);
      builder.AddElement("business", Business);
      builder.AddElement("tos-accepted", TosAccepted);
      builder.AddElement("master-merchant-account-id", MasterMerchantAccountId);
      return builder;
    }
  }
}
