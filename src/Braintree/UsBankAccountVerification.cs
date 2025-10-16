#pragma warning disable 1591

using System;
using System.ComponentModel;

namespace Braintree
{
    public enum UsBankAccountVerificationStatus
    {
        [Description("failed")] FAILED,
        [Description("gateway_rejected")] GATEWAY_REJECTED,
        [Description("independent_check")] INDEPENDENT_CHECK,
        [Description("pending")] PENDING,
        [Description("processor_declined")] PROCESSOR_DECLINED,
        [Description("unrecognized")] UNRECOGNIZED,
        [Description("verified")] VERIFIED
    }

    public enum UsBankAccountVerificationMethod
    {
        [Description("independent_check")] INDEPENDENT_CHECK,
        [Description("instant_verification_account_validation")] INSTANT_VERIFICATION_ACCOUNT_VALIDATION,
        [Description("micro_transfers")] MICRO_TRANSFERS,
        [Description("network_check")] NETWORK_CHECK,
        [Description("tokenized_check")] TOKENIZED_CHECK,
        [Description("unrecognized")] UNRECOGNIZED
    }

    public static class UsBankAccountVerificationMethodExtensions
    {
        public static string GetDescription(this UsBankAccountVerificationMethod method)
        {
            var field = method.GetType().GetField(method.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? method.ToString().ToLowerInvariant();
        }

        public static UsBankAccountVerificationMethod FromString(string name)
        {
            switch (name?.ToLowerInvariant())
            {
                case "independent_check":
                    return UsBankAccountVerificationMethod.INDEPENDENT_CHECK;
                case "instant_verification_account_validation":
                    return UsBankAccountVerificationMethod.INSTANT_VERIFICATION_ACCOUNT_VALIDATION;
                case "micro_transfers":
                    return UsBankAccountVerificationMethod.MICRO_TRANSFERS;
                case "network_check":
                    return UsBankAccountVerificationMethod.NETWORK_CHECK;
                case "tokenized_check":
                    return UsBankAccountVerificationMethod.TOKENIZED_CHECK;
                default:
                    return UsBankAccountVerificationMethod.UNRECOGNIZED;
            }
        }
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
            VerificationMethod = UsBankAccountVerificationMethodExtensions.FromString(node.GetString("verification-method"));
        }

        [Obsolete("Mock Use Only")]
        protected internal UsBankAccountVerification() { }
    }
}
