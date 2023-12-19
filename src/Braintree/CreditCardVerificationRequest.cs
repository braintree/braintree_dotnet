#pragma warning disable 1591

namespace Braintree
{
    public class CreditCardVerificationCreditCardRequest : BaseCreditCardRequest {
        public CreditCardAddressRequest BillingAddress { get; set; }
    }

    public class CreditCardVerificationRequest : Request {
        public CreditCardVerificationCreditCardRequest CreditCard { get; set; }
        public ExternalVaultRequest ExternalVault { get; set; }
        public string IntendedTransactionSource { get; set; }
        public CreditCardVerificationOptionsRequest Options { get; set; }
        public string PaymentMethodNonce { get; set; }
        public RiskDataRequest RiskData { get; set; }
        public string ThreeDSecureAuthenticationID { get; set; }
        public ThreeDSecurePassThruRequest ThreeDSecurePassThru { get; set; }

        public override string ToXml()
        {
            return ToXml("verification");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("credit-card", CreditCard).
                AddElement("external-vault", ExternalVault).
                AddElement("intendedTransactionSource", IntendedTransactionSource).
                AddElement("options", Options).
                AddElement("paymentMethodNonce", PaymentMethodNonce).
                AddElement("risk-data", RiskData).
                AddElement("threeDSecureAuthenticationID", ThreeDSecureAuthenticationID).
                AddElement("threeDSecurePassThru", ThreeDSecurePassThru);
        }
    }
}
