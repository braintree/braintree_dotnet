#pragma warning disable 1591

using System;
using System.ComponentModel;

namespace Braintree
{

    public enum VerificationStatus
    {
        [Description("gateway_rejected")] GATEWAY_REJECTED,
        [Description("failed")] FAILED,
        [Description("processor_declined")] PROCESSOR_DECLINED,
        [Description("unrecognized")] UNRECOGNIZED,
        [Description("verified")] VERIFIED
    }

    public class CreditCardVerification
    {
        public virtual decimal? Amount { get; protected set; }
        public virtual string AniFirstNameResponseCode { get; protected set; }
        public virtual string AniLastNameResponseCode { get; protected set; }
        public virtual string AvsErrorResponseCode { get; protected set; }
        public virtual string AvsPostalCodeResponseCode { get; protected set; }
        public virtual string AvsStreetAddressResponseCode { get; protected set; }
        public virtual Address BillingAddress { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual CreditCard CreditCard { get; protected set; }
        public virtual string CurrencyIsoCode { get; protected set; }
        public virtual string CvvResponseCode { get; protected set; }
        public virtual TransactionGatewayRejectionReason? GatewayRejectionReason { get; protected set; }
        public virtual string GraphQLId { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual string MerchantAccountId { get; protected set; }
        public virtual string NetworkResponseCode { get; protected set; }
        public virtual string NetworkResponseText { get; protected set; }
        public virtual string NetworkTransactionId { get; protected set; }
        public virtual string ProcessorResponseCode { get; protected set; }
        public virtual string ProcessorResponseText { get; protected set; }
        public virtual ProcessorResponseType ProcessorResponseType { get; protected set; }
        public virtual RiskData RiskData { get; protected set; }
        public virtual VerificationStatus? Status { get; protected set; }
        public virtual ThreeDSecureInfo ThreeDSecureInfo { get; protected set; }
        
        public CreditCardVerification(NodeWrapper node, IBraintreeGateway gateway)
        {
            if (node == null) return;

            Amount = node.GetDecimal("amount");
            AniFirstNameResponseCode = node.GetString("ani-first-name-response-code");
            AniLastNameResponseCode = node.GetString("ani-last-name-response-code");
            AvsErrorResponseCode = node.GetString("avs-error-response-code");
            AvsPostalCodeResponseCode = node.GetString("avs-postal-code-response-code");
            AvsStreetAddressResponseCode = node.GetString("avs-street-address-response-code");
            CurrencyIsoCode = node.GetString("currency-iso-code");
            CvvResponseCode = node.GetString("cvv-response-code");
            GatewayRejectionReason = node.GetEnum<TransactionGatewayRejectionReason>("gateway-rejection-reason");
            ProcessorResponseCode = node.GetString("processor-response-code");
            ProcessorResponseText = node.GetString("processor-response-text");
            ProcessorResponseType = node.GetEnum("processor-response-type", ProcessorResponseType.UNRECOGNIZED);
            NetworkResponseCode = node.GetString("network-response-code");
            NetworkResponseText = node.GetString("network-response-text");
            NetworkTransactionId = node.GetString("network-transaction-id");
            MerchantAccountId = node.GetString("merchant-account-id");
            Status = node.GetEnum("status", VerificationStatus.UNRECOGNIZED);
            GraphQLId = node.GetString("global-id");
            Id = node.GetString("id");
            BillingAddress = new Address(node.GetNode("billing"));
            CreditCard = new CreditCard(node.GetNode("credit-card"), gateway);
            CreatedAt = node.GetDateTime("created-at");

            var riskDataNode = node.GetNode("risk-data");
            if (riskDataNode != null)
            {
                RiskData = new RiskData(riskDataNode);
            }

            var threeDSecureInfoNode = node.GetNode("three-d-secure-info");
            if (threeDSecureInfoNode != null && !threeDSecureInfoNode.IsEmpty())
            {
                ThreeDSecureInfo = new ThreeDSecureInfo(threeDSecureInfoNode);
            }
            NetworkTransactionId = node.GetString("network-transaction-id");
        }
        
        [Obsolete("Mock Use Only")]
        protected internal CreditCardVerification() { }
    }
}
