#pragma warning disable 1591

using System;

namespace Braintree
{
    public class IndividualRequest : Request
    {
      public String FirstName { get; set; }
      public String LastName { get; set; }
      public String Email { get; set; }
      public String Phone { get; set; }
      public AddressRequest Address { get; set; }
      public String DateOfBirth { get; set; }
      public String Ssn { get; set; }

      public override String ToXml()
      {
        return ToXml("individual");
      }

      public override String ToXml(String root)
      {
        return BuildRequest(root).ToXml();
      }

      public override String ToQueryString()
      {
        return ToQueryString("individual");
      }

      public override String ToQueryString(String root)
      {
        return BuildRequest(root).ToQueryString();
      }

      protected virtual RequestBuilder BuildRequest(String root)
      {
        RequestBuilder builder = new RequestBuilder(root);

        builder.AddElement("first-name", FirstName);
        builder.AddElement("last-name", LastName);
        builder.AddElement("email", Email);
        builder.AddElement("phone", Phone);
        builder.AddElement("address", Address);
        builder.AddElement("date-of-birth", DateOfBirth);
        builder.AddElement("ssn", Ssn);
        return builder;
      }
    }
}
