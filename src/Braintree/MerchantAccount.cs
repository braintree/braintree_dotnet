using System;
using System.ComponentModel;

namespace Braintree
{
    public enum FundingDestination
    {
        [Description("bank")] BANK,
        [Description("mobile_phone")] MOBILE_PHONE,
        [Description("email")] EMAIL,
        [Description("unrecognized")] UNRECOGNIZED
    }

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
      public virtual MerchantAccount MasterMerchantAccount { get; protected set; }
      public virtual MerchantAccountIndividualDetails IndividualDetails { get; protected set; }
      public virtual MerchantAccountBusinessDetails BusinessDetails { get; protected set; }
      public virtual MerchantAccountFundingDetails FundingDetails { get; protected set; }
      public virtual bool? IsDefault { get; protected set; }

      public bool IsSubMerchant {
        get {
          return MasterMerchantAccount != null;
        }
      }

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

        NodeWrapper masterNode = node.GetNode("master-merchant-account");
        if (masterNode != null)
            MasterMerchantAccount = new MerchantAccount(masterNode);
        else
            MasterMerchantAccount = null;
        NodeWrapper individualNode = node.GetNode("individual");
        if (individualNode != null)
            IndividualDetails = new MerchantAccountIndividualDetails(individualNode);
        else
            IndividualDetails = null;
        NodeWrapper businessNode = node.GetNode("business");
        if (businessNode != null)
            BusinessDetails = new MerchantAccountBusinessDetails(businessNode);
        else
            BusinessDetails = null;
        NodeWrapper fundingNode = node.GetNode("funding");
        if (fundingNode != null)
            FundingDetails = new MerchantAccountFundingDetails(fundingNode);
        else
            FundingDetails = null;
      }

      [Obsolete("Mock Use Only")]
      protected MerchantAccount() { }
    }
}
