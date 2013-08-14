using System;

namespace Braintree
{
    public class PartnerCredentials
    {
      public String MerchantPublicId { get; protected set; }
      public String PublicKey { get; protected set; }
      public String PrivateKey { get; protected set; }
      public String PartnerUserId { get; protected set; }

      protected internal PartnerCredentials(NodeWrapper node)
      {
         MerchantPublicId = node.GetString("merchant-public-id");
         PublicKey = node.GetString("public-key");
         PrivateKey = node.GetString("private-key");
         PartnerUserId = node.GetString("partner-user-id");
      }
    }
}

