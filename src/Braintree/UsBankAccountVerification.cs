#pragma warning disable 1591

using System;

namespace Braintree
{
    public class UsBankAccountVerificationStatus : Enumeration
    {
        public static readonly UsBankAccountVerificationStatus FAILED = new UsBankAccountVerificationStatus("failed");
        public static readonly UsBankAccountVerificationStatus GATEWAY_REJECTED = new UsBankAccountVerificationStatus("gateway_rejected");
        public static readonly UsBankAccountVerificationStatus PROCESSOR_DECLINED = new UsBankAccountVerificationStatus("processor_declined");
        public static readonly UsBankAccountVerificationStatus UNRECOGNIZED = new UsBankAccountVerificationStatus("unrecognized");
        public static readonly UsBankAccountVerificationStatus VERIFIED = new UsBankAccountVerificationStatus("verified");
        public static readonly UsBankAccountVerificationStatus PENDING = new UsBankAccountVerificationStatus("pending");


        public static readonly UsBankAccountVerificationStatus[] ALL = {
            FAILED, GATEWAY_REJECTED, PROCESSOR_DECLINED, VERIFIED, PENDING
        };

        protected UsBankAccountVerificationStatus(string name) : base(name) {}
    }

    public class UsBankAccountVerificationMethod : Enumeration
    {
        public static readonly UsBankAccountVerificationMethod INDEPENDENT_CHECK = new UsBankAccountVerificationMethod("independent_check");
        public static readonly UsBankAccountVerificationMethod NETWORK_CHECK = new UsBankAccountVerificationMethod("network_check");
        public static readonly UsBankAccountVerificationMethod TOKENIZED_CHECK = new UsBankAccountVerificationMethod("tokenized_check");
        public static readonly UsBankAccountVerificationMethod MICRO_TRANSFERS = new UsBankAccountVerificationMethod("micro_transfers");
        public static readonly UsBankAccountVerificationMethod UNRECOGNIZED = new UsBankAccountVerificationMethod("unrecognized");

        public static readonly UsBankAccountVerificationMethod[] ALL = {
            INDEPENDENT_CHECK, NETWORK_CHECK, TOKENIZED_CHECK, MICRO_TRANSFERS
        };

        protected UsBankAccountVerificationMethod(string name) : base(name) {}
    }

    public class UsBankAccountVerification
    {
        public virtual UsBankAccountVerificationMethod VerificationMethod { get; protected set; }
        public virtual DateTime? VerificationDeterminedAt { get; protected set; }
        public virtual TransactionGatewayRejectionReason GatewayRejectionReason { get; protected set; }
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

            VerificationMethod = (UsBankAccountVerificationMethod)CollectionUtil.Find(
                UsBankAccountVerificationMethod.ALL,
                node.GetString("verification-method"),
                UsBankAccountVerificationMethod.UNRECOGNIZED
            );
            VerificationDeterminedAt = node.GetDateTime("verification-determined-at");
            GatewayRejectionReason = null;
            ProcessorResponseCode = node.GetString("processor-response-code");
            ProcessorResponseText = node.GetString("processor-response-text");
            MerchantAccountId = node.GetString("merchant-account-id");
            Status = (UsBankAccountVerificationStatus)CollectionUtil.Find(
                UsBankAccountVerificationStatus.ALL,
                node.GetString("status"),
                UsBankAccountVerificationStatus.UNRECOGNIZED
            );
            Id = node.GetString("id");

            UsBankAccount = new UsBankAccount(node.GetNode("us-bank-account"));
            CreatedAt = node.GetDateTime("created-at");
        }

        [Obsolete("Mock Use Only")]
        protected internal UsBankAccountVerification() { }
    }
}
