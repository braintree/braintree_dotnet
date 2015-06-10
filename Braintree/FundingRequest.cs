#pragma warning disable 1591

using System;

namespace Braintree
{
    public class FundingRequest : Request
    {
      public FundingDestination Destination { get; set; }
      public string Email { get; set; }
      public string MobilePhone { get; set; }
      public string RoutingNumber { get; set; }
      public string AccountNumber { get; set; }
      public string Descriptor { get; set; }

      public override string ToXml()
      {
        return ToXml("funding");
      }

      public override string ToXml(string root)
      {
        return BuildRequest(root).ToXml();
      }

      public override string ToQueryString()
      {
        return ToQueryString("funding");
      }

      public override string ToQueryString(string root)
      {
        return BuildRequest(root).ToQueryString();
      }

      protected virtual RequestBuilder BuildRequest(string root)
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
