#pragma warning disable 1591

using System;

namespace Braintree
{
    public class FundingRequest : Request
    {
      public FundingDestination Destination { get; set; }
      public String Email { get; set; }
      public String MobilePhone { get; set; }
      public String RoutingNumber { get; set; }
      public String AccountNumber { get; set; }
      public String Descriptor { get; set; }

      public override String ToXml()
      {
        return ToXml("funding");
      }

      public override String ToXml(String root)
      {
        return BuildRequest(root).ToXml();
      }

      public override String ToQueryString()
      {
        return ToQueryString("funding");
      }

      public override String ToQueryString(String root)
      {
        return BuildRequest(root).ToQueryString();
      }

      protected virtual RequestBuilder BuildRequest(String root)
      {
        RequestBuilder builder = new RequestBuilder(root);

        builder.AddElement("destination", Destination);
        builder.AddElement("email", Email);
        builder.AddElement("mobile-phone", MobilePhone);
        builder.AddElement("routing-number", RoutingNumber);
        builder.AddElement("account-number", AccountNumber);
        builder.AddElement("descriptor", Descriptor);
        return builder;
      }
    }
}
