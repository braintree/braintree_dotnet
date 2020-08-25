#pragma warning disable 1591

using System;
using System.ComponentModel;

namespace Braintree
{
    public enum UsBankAccountVerificationStatus
    {
        [Description("failed")] FAILED,
        [Description("gateway_rejected")] GATEWAY_REJECTED,
        [Description("processor_declined")] PROCESSOR_DECLINED,
        [Description("unrecognized")] UNRECOGNIZED,
        [Description("verified")] VERIFIED,
        [Description("pending")] PENDING
    }

    public enum UsBankAccountVerificationMethod
    {
        [Description("independent_check")] INDEPENDENT_CHECK,
        [Description("network_check")] NETWORK_CHECK,
        [Description("tokenized_check")] TOKENIZED_CHECK,
        [Description("micro_transfers")] MICRO_TRANSFERS,
        [Description("unrecognized")] UNRECOGNIZED
    }

    public class UsBankAccountVerification
    {
        public virtual UsBankAccountVerificationMethod VerificationMethod { get; protected set; }
        public virtual DateTime? VerificationDeterminedAt { get; protected set; }
        public virtual TransactionGatewayRejectionReason? GatewayRejectionReason { get; protected set; }
        public virtual string ProcessorResponseCode { get; protected set; }
        public virtual string ProcessorResponseText { get; protected set; }
        public virtual string MerchantAccountId { get; protected set; }
        public virtual UsBankAccountVerificationStatus Status { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual UsBankAccount UsBankAccount { get; protected set; }
        public virtual DateTime? CreatedAt { get; protected set; }

        public UsBankAccountVerification(NodeWrapper node)
        {
            if (node == null) return;

            VerificationMethod = node.GetEnum("verification-method", UsBankAccountVerificationMethod.UNRECOGNIZED);
            VerificationDeterminedAt = node.GetDateTime("verification-determined-at");
            GatewayRejectionReason = null;
            ProcessorResponseCode = node.GetString("processor-response-code");
            ProcessorResponseText = node.GetString("processor-response-text");
            MerchantAccountId = node.GetString("merchant-account-id");
            Status = node.GetEnum("status", UsBankAccountVerificationStatus.UNRECOGNIZED);
            Id = node.GetString("id");

            UsBankAccount = new UsBankAccount(node.GetNode("us-bank-account"));
            CreatedAt = node.GetDateTime("created-at");
        }

        [Obsolete("Mock Use Only")]
        protected internal UsBankAccountVerification() { }
    }
}
