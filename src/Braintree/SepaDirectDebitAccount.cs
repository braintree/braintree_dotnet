using System;
using System.ComponentModel;

namespace Braintree
{
    public enum MandateType
    {
        [Description("one_off")] ONE_OFF,
        [Description("recurrent")] RECURRENT
    }

    public class SepaDirectDebitAccount : PaymentMethod
    {
        public DateTime? CreatedAt { get; protected set; }
        public DateTime? UpdatedAt { get; protected set; }
        public MandateType MandateType { get; protected set; }
        public bool? IsDefault { get; protected set; }
        public string BankReferenceToken { get; protected set; }
        public string CustomerGlobalId { get; protected set; }
        public string CustomerId { get; protected set; }
        public string GlobalId { get; protected set; }
        public string ImageUrl { get; protected set; }
        public string Last4 { get; protected set; }
        public string MerchantAccountId { get; protected set; }
        public string MerchantOrPartnerCustomerId { get; protected set; }
        public string Token { get; protected set; }
        public string ViewMandateUrl { get; protected set; }

        protected internal SepaDirectDebitAccount(NodeWrapper node, IBraintreeGateway gateway)
        {
            BankReferenceToken = node.GetString("bank-reference-token");
            CreatedAt = node.GetDateTime("created-at");
            CustomerGlobalId = node.GetString("customer-global-id");
            CustomerId = node.GetString("customer-id");
            GlobalId = node.GetString("global-id");
            ImageUrl = node.GetString("image-url");
            IsDefault = node.GetBoolean("default");
            Last4 = node.GetString("last-4");
            MandateType = node.GetEnum<MandateType>("mandate-type", MandateType.ONE_OFF);
            MerchantAccountId = node.GetString("merchant-account-id");
            MerchantOrPartnerCustomerId = node.GetString("merchant-or-partner-customer-id");
            Token = node.GetString("token");
            UpdatedAt = node.GetDateTime("updated-at");
            ViewMandateUrl = node.GetString("view-mandate-url");
        }

        [Obsolete("Mock Use Only")]
        protected internal SepaDirectDebitAccount() { }
    }
}
