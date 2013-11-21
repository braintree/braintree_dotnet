#pragma warning disable 1591

using System;

namespace Braintree
{
    public class BusinessRequest : Request
    {
      public String DbaName { get; set; }
      public String LegalName { get; set; }
      public AddressRequest Address { get; set; }
      public String TaxId { get; set; }

      public override String ToXml()
      {
        return ToXml("business");
      }

      public override String ToXml(String root)
      {
        return BuildRequest(root).ToXml();
      }

      public override String ToQueryString()
      {
        return ToQueryString("business");
      }

      public override String ToQueryString(String root)
      {
        return BuildRequest(root).ToQueryString();
      }

      protected virtual RequestBuilder BuildRequest(String root)
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
