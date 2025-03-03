namespace Braintree
{
    public class PayPalPaymentResourceRequest : Request
    {
        public decimal Amount { get; set; }
        public AmountBreakdownRequest AmountBreakdown { get; set; }
        public string CurrencyIsoCode { get; set; }
        public string CustomField { get; set; }
        public string Description { get; set; }
        public TransactionLineItemRequest[] LineItems { get; set; }
        public string OrderId  { get; set; }
        public string PayeeEmail { get; set; }
        public string PaymentMethodNonce { get; set; }
        public AddressRequest Shipping { get; set; }
        public ShippingOptionRequest[] ShippingOptions { get; set; }

        public override string ToXml()
        {
            return ToXml("paypal-payment-resource");
        }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            var builder = new RequestBuilder(root);
            builder.AddElement("amount", Amount);
            if (AmountBreakdown != null) {
                builder.AddElement("amount-breakdown", AmountBreakdown);
            }
            builder.AddElement("currency-iso-code", CurrencyIsoCode);
            builder.AddElement("custom-field", CustomField);
            builder.AddElement("description", Description);
            if (LineItems != null)
            {
                builder.AddElement("line-items", LineItems);
            }
            builder.AddElement("order-id", OrderId);
            builder.AddElement("payee-email", PayeeEmail);
            builder.AddElement("payment-method-nonce", PaymentMethodNonce);
            builder.AddElement("shipping", Shipping);
            if (ShippingOptions != null) {
                builder.AddElement("shipping-options", ShippingOptions);
            }
            return builder;
        }
    }
}
