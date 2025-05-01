using System;
using System.ComponentModel;

namespace Braintree
{

    public enum MerchantAccountStatus
    {
        [Description("pending")] PENDING,
        [Description("active")] ACTIVE,
        [Description("suspended")] SUSPENDED
    }

    public class MerchantAccount
    {
      public virtual string Id { get; protected set; }
      public virtual string CurrencyIsoCode { get; protected set; }
      public virtual MerchantAccountStatus? Status { get; protected set; }
      public virtual bool? IsDefault { get; protected set; }


      protected internal MerchantAccount(NodeWrapper node)
      {
        NodeWrapper merchantAccountNode = node.GetNode("merchant-account");

        if (merchantAccountNode != null) {
            node = merchantAccountNode;
        }

        Id = node.GetString("id");
        CurrencyIsoCode = node.GetString("currency-iso-code");
        Status = node.GetEnum<MerchantAccountStatus>("status");
        IsDefault = node.GetBoolean("default");
       
      }

      [Obsolete("Mock Use Only")]
      protected MerchantAccount() { }
    }
}
