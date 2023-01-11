using System;

namespace Braintree
{
    public class PaymentMethodNonceDetailsSepaDirectDebit
    {
        public virtual MandateType MandateType { get; protected set; }
        public virtual string BankReferenceToken { get; protected set; }
        public virtual string CorrelationId { get; protected set; }
        public virtual string IbanLastChars { get; protected set; }
        public virtual string MerchantOrPartnerCustomerId { get; protected set; }

        protected internal PaymentMethodNonceDetailsSepaDirectDebit(NodeWrapper node)
        {
            BankReferenceToken = node.GetString("bank-reference-token");
            CorrelationId = node.GetString("correlation-id");
            IbanLastChars = node.GetString("iban-last-chars");
            MandateType = node.GetEnum<MandateType>("mandate-type", MandateType.ONE_OFF);
            MerchantOrPartnerCustomerId = node.GetString("merchant-or-partner-customer-id");
        }

        protected internal PaymentMethodNonceDetailsSepaDirectDebit(dynamic details)
        {
            BankReferenceToken = details.bankReferenceToken;
            CorrelationId = details.correlationId;
            IbanLastChars = details.ibanLastChars;
            MandateType = EnumHelper.FindEnum((string)details.mandateType, MandateType.ONE_OFF);
            MerchantOrPartnerCustomerId = details.merchantOrPartnerCustomerId;
        }

        [Obsolete("Mock Use Only")]
        protected internal PaymentMethodNonceDetailsSepaDirectDebit() { }
    }
}
