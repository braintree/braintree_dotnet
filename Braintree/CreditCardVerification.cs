#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    [Serializable]
    public class VerificationStatus : Enumeration
    {
        public static readonly VerificationStatus FAILED = new VerificationStatus("failed");
        public static readonly VerificationStatus GATEWAY_REJECTED = new VerificationStatus("gateway_rejected");
        public static readonly VerificationStatus PROCESSOR_DECLINED = new VerificationStatus("processor_declined");
        public static readonly VerificationStatus UNRECOGNIZED = new VerificationStatus("unrecognized");
        public static readonly VerificationStatus VERIFIED = new VerificationStatus("verified");


        public static readonly VerificationStatus[] ALL = {
            FAILED, GATEWAY_REJECTED, PROCESSOR_DECLINED, VERIFIED
        };

        protected VerificationStatus(String name) : base(name) {}
    }

    [Serializable]
    public class CreditCardVerification
    {
        public String AvsErrorResponseCode { get; protected set; }
        public String AvsPostalCodeResponseCode { get; protected set; }
        public String AvsStreetAddressResponseCode { get; protected set; }
        public String CvvResponseCode { get; protected set; }
        public TransactionGatewayRejectionReason GatewayRejectionReason { get; protected set; }
        public String ProcessorResponseCode { get; protected set; }
        public String ProcessorResponseText { get; protected set; }
        public String MerchantAccountId { get; protected set; }
        public VerificationStatus Status { get; protected set; }

        public CreditCardVerification(NodeWrapper node)
        {
            if (node == null) return;

            AvsErrorResponseCode = node.GetString("avs-error-response-code");
            AvsPostalCodeResponseCode = node.GetString("avs-postal-code-response-code");
            AvsStreetAddressResponseCode = node.GetString("avs-street-address-response-code");
            CvvResponseCode = node.GetString("cvv-response-code");
            GatewayRejectionReason = (TransactionGatewayRejectionReason)CollectionUtil.Find(
                TransactionGatewayRejectionReason.ALL,
                node.GetString("gateway-rejection-reason"),
                null
            );
            ProcessorResponseCode = node.GetString("processor-response-code");
            ProcessorResponseText = node.GetString("processor-response-text");
            MerchantAccountId = node.GetString("merchant-account-id");
            Status = (VerificationStatus)CollectionUtil.Find(VerificationStatus.ALL, node.GetString("status"), VerificationStatus.UNRECOGNIZED);
        }
    }
}
