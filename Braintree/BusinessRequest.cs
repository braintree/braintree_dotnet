#pragma warning disable 1591

using System;

namespace Braintree
{
    public class BusinessRequest : Request
    {
      public string DbaName { get; set; }
      public string LegalName { get; set; }
      public AddressRequest Address { get; set; }
      public string TaxId { get; set; }

      public override string ToXml()
      {
        return ToXml("business");
      }

      public override string ToXml(string root)
      {
        return BuildRequest(root).ToXml();
      }

      public override string ToQueryString()
      {
        return ToQueryString("business");
      }

      public override string ToQueryString(string root)
      {
        return BuildRequest(root).ToQueryString();
      }

      protected virtual RequestBuilder BuildRequest(string root)
      {
        RequestBuilder builder = new RequestBuilder(root);

        builder.AddElement("dba-name", DbaName);
        builder.AddElement("legal-name", LegalName);
        builder.AddElement("address", Address);
        builder.AddElement("tax-id", TaxId);
        return builder;
      }
    }
}
