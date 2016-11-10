namespace Braintree
{
    public class PaymentMethodGrantRevokeRequest : Request
    {
        internal string SharedPaymentMethodToken { get; set; }

        public override string ToXml()
        {
            return BuildRequest("payment-method").ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("shared-payment-method-token", SharedPaymentMethodToken);
        }
    }
}
