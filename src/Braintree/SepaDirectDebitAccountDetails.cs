using System;
using System.ComponentModel;

namespace Braintree
{
    public enum SettlementType
    {
        [Description("instant")] INSTANT,
        [Description("delayed")] DELAYED
    }

    public class SepaDirectDebitAccountDetails
    {
        public virtual MandateType MandateType { get; protected set; }
        public virtual SettlementType SettlementType { get; protected set; }
        public virtual string BankReferenceToken { get; protected set; }
        public virtual string CaptureId { get; protected set; }
        public virtual string DebugId { get; protected set; }
        public virtual string MerchantOrPartnerCustomerId { get; protected set; }
        public virtual string PayPalV2OrderId{ get; protected set; }
        public virtual string RefundFromTransactionFeeAmount { get; protected set; }
        public virtual string RefundFromTransactionFeeCurrencyIsoCode { get; protected set; }
        public virtual string RefundId { get; protected set; }
        public virtual string Token { get; protected set; }
        public virtual string TransactionFeeAmount { get; protected set; }
        public virtual string TransactionFeeCurrencyIsoCode { get; protected set; }

        protected internal SepaDirectDebitAccountDetails(NodeWrapper node)
        {
            BankReferenceToken = node.GetString("bank-reference-token");
            CaptureId = node.GetString("capture-id");
            DebugId = node.GetString("debug-id");
            MandateType = node.GetEnum<MandateType>("mandate-type", MandateType.ONE_OFF);
            MerchantOrPartnerCustomerId = node.GetString("merchant-or-partner-customer-id");
            PayPalV2OrderId = node.GetString("paypal-v2-order-id");
            RefundFromTransactionFeeAmount = node.GetString("refund-from-transaction-fee-amount");
            RefundFromTransactionFeeCurrencyIsoCode = node.GetString("refund-from-transaction-fee-currency-iso-code");
            RefundId = node.GetString("refund-id");
            SettlementType = node.GetEnum<SettlementType>("settlement-type", SettlementType.INSTANT);
            Token = node.GetString("token");
            TransactionFeeAmount = node.GetString("transaction-fee-amount");
            TransactionFeeCurrencyIsoCode = node.GetString("transaction-fee-currency-iso-code");
        }

        [Obsolete("Mock Use Only")]
        protected internal SepaDirectDebitAccountDetails() { }
    }
}
