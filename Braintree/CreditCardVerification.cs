#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
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
        public String Status { get; protected set; }

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
            Status = node.GetString("status");
        }
    }
}
