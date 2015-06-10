#pragma warning disable 1591

using System;

namespace Braintree
{
    public class ApplicantDetailsRequest : Request
    {
      public string CompanyName { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Email { get; set; }
      public string Phone { get; set; }
      public AddressRequest Address { get; set; }
      public string DateOfBirth { get; set; }
      public string Ssn { get; set; }
      public string TaxId { get; set; }
      public string RoutingNumber { get; set; }
      public string AccountNumber { get; set; }

      public override string ToXml()
      {
        return ToXml("applicant-details");
      }

      public override string ToXml(string root)
      {
        return BuildRequest(root).ToXml();
      }

      public override string ToQueryString()
      {
        return ToQueryString("applicant-details");
      }

      public override string ToQueryString(string root)
      {
        return BuildRequest(root).ToQueryString();
      }

      protected virtual RequestBuilder BuildRequest(string root)
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
