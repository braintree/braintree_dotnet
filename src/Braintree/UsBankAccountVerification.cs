#pragma warning disable 1591

using System;
using System.ComponentModel;

namespace Braintree
{
    public enum UsBankAccountVerificationStatus
    {
        [Description("failed")] FAILED,
        [Description("gateway_rejected")] GATEWAY_REJECTED,
        [Description("pending")] PENDING,
        [Description("processor_declined")] PROCESSOR_DECLINED,
        [Description("unrecognized")] UNRECOGNIZED,
        [Description("verified")] VERIFIED
    }

    public enum UsBankAccountVerificationMethod
    {
        [Description("independent_check")] INDEPENDENT_CHECK,
        [Description("micro_transfers")] MICRO_TRANSFERS,
        [Description("network_check")] NETWORK_CHECK,
        [Description("tokenized_check")] TOKENIZED_CHECK,
        [Description("unrecognized")] UNRECOGNIZED
    }
    
    public enum VerificationAddOns
    {
        [Description("customer_verification")] CUSTOMER_VERIFICATION,
        [Description("unrecognized")] UNRECOGNIZED
    }

    public class UsBankAccountVerification
    {
        public virtual DateTime? CreatedAt { get; protected set; }
        public virtual DateTime? VerificationDeterminedAt { get; protected set; }
        public virtual TransactionGatewayRejectionReason? GatewayRejectionReason { get; protected set; }
        public virtual UsBankAccount UsBankAccount { get; protected set; }
        public virtual UsBankAccountVerificationMethod VerificationMethod { get; protected set; }
        public virtual UsBankAccountVerificationStatus Status { get; protected set; }
        public virtual VerificationAddOns VerificationAddOns { get; protected set; }
        public virtual string AdditionalProcessorResponse { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual string MerchantAccountId { get; protected set; }
        public virtual string ProcessorResponseCode { get; protected set; }
        public virtual string ProcessorResponseText { get; protected set; }

        public UsBankAccountVerification(NodeWrapper node)
        {
            if (node == null) return;

            AdditionalProcessorResponse = node.GetString("additional-processor-response");
            CreatedAt = node.GetDateTime("created-at");
            GatewayRejectionReason = null;
            Id = node.GetString("id");
            MerchantAccountId = node.GetString("merchant-account-id");
            ProcessorResponseCode = node.GetString("processor-response-code");
            ProcessorResponseText = node.GetString("processor-response-text");
            Status = node.GetEnum("status", UsBankAccountVerificationStatus.UNRECOGNIZED);
            UsBankAccount = new UsBankAccount(node.GetNode("us-bank-account"));
            VerificationAddOns = node.GetEnum("verification-add-ons", VerificationAddOns.UNRECOGNIZED);
            VerificationDeterminedAt = node.GetDateTime("verification-determined-at");
            VerificationMethod = node.GetEnum("verification-method", UsBankAccountVerificationMethod.UNRECOGNIZED);
        }

        [Obsolete("Mock Use Only")]
        protected internal UsBankAccountVerification() { }
    }
}
