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

      public bool IsSubMerchant => MasterMerchantAccount != null;

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
        MasterMerchantAccount = masterNode != null ? new MerchantAccount(masterNode) : null;
        NodeWrapper individualNode = node.GetNode("individual");
        IndividualDetails = individualNode != null ? new MerchantAccountIndividualDetails(individualNode) : null;
        NodeWrapper businessNode = node.GetNode("business");
        BusinessDetails = businessNode != null ? new MerchantAccountBusinessDetails(businessNode) : null;
        NodeWrapper fundingNode = node.GetNode("funding");
        FundingDetails = fundingNode != null ? new MerchantAccountFundingDetails(fundingNode) : null;
      }

      [Obsolete("Mock Use Only")]
      protected MerchantAccount() { }
    }
}
