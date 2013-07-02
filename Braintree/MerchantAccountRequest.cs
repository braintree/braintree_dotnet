using System;

namespace Braintree
{
  public class MerchantAccountRequest : Request
  {
    public String Id { get; set; }
    public ApplicantDetailsRequest ApplicantDetails { get; set; }
    public Boolean? TosAccepted { get; set; }
    public String MasterMerchantAccountId { get; set; }

    public override String ToXml()
    {
      return ToXml("merchant-account");
    }

    public override String ToXml(String root)
    {
      return BuildRequest(root).ToXml();
    }

    public override String ToQueryString()
    {
      return ToQueryString("merchant-account");
    }

    public override String ToQueryString(String root)
    {
      return BuildRequest(root).ToQueryString();
    }

    protected virtual RequestBuilder BuildRequest(String root)
    {
      RequestBuilder builder = new RequestBuilder(root);

      builder.AddElement("id", Id);
      builder.AddElement("applicant-details", ApplicantDetails);
      builder.AddElement("tos-accepted", TosAccepted);
      builder.AddElement("master-merchant-account-id", MasterMerchantAccountId);
      return builder;
    }
  }
}
