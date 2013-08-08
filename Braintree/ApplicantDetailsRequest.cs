#pragma warning disable 1591

using System;

namespace Braintree
{
    public class ApplicantDetailsRequest : Request
    {
      public String CompanyName { get; set; }
      public String FirstName { get; set; }
      public String LastName { get; set; }
      public String Email { get; set; }
      public String Phone { get; set; }
      public AddressRequest Address { get; set; }
      public String DateOfBirth { get; set; }
      public String Ssn { get; set; }
      public String TaxId { get; set; }
      public String RoutingNumber { get; set; }
      public String AccountNumber { get; set; }

      public override String ToXml()
      {
        return ToXml("applicant-details");
      }

      public override String ToXml(String root)
      {
        return BuildRequest(root).ToXml();
      }

      public override String ToQueryString()
      {
        return ToQueryString("applicant-details");
      }

      public override String ToQueryString(String root)
      {
        return BuildRequest(root).ToQueryString();
      }

      protected virtual RequestBuilder BuildRequest(String root)
      {
        RequestBuilder builder = new RequestBuilder(root);

        builder.AddElement("company-name", CompanyName);
        builder.AddElement("first-name", FirstName);
        builder.AddElement("last-name", LastName);
        builder.AddElement("email", Email);
        builder.AddElement("phone", Phone);
        builder.AddElement("address", Address);
        builder.AddElement("date-of-birth", DateOfBirth);
        builder.AddElement("ssn", Ssn);
        builder.AddElement("tax-id", TaxId);
        builder.AddElement("routing-number", RoutingNumber);
        builder.AddElement("account-number", AccountNumber);
        return builder;
      }
    }
}
