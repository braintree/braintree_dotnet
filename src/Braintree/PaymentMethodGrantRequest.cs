namespace Braintree
{
    public class PaymentMethodGrantRequest : Request
    {
        internal string SharedPaymentMethodToken { get; set; }
        public bool? AllowVaulting { get; set; }
        public bool? IncludeBillingPostalCode { get; set; }
        public string RevokeAfter { get; set; }

        public override string ToXml()
        {
            return BuildRequest("payment-method").ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("shared-payment-method-token", SharedPaymentMethodToken).
                AddElement("allow-vaulting", AllowVaulting).
                AddElement("include-billing-postal-code", IncludeBillingPostalCode).
                AddElement("revoke-after", RevokeAfter);
        }
    }
}


