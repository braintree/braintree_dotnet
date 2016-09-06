namespace Braintree
{
    public class UnknownPaymentMethod : PaymentMethod
    {
        public virtual string Token { get; protected set; }
        public virtual bool? IsDefault { get; protected set; }
        public virtual string ImageUrl { get; protected set; }
        public virtual string CustomerId { get; protected set; }

        public UnknownPaymentMethod(NodeWrapper node)
        {
            Token = node.GetString("token");
            IsDefault = node.GetBoolean("default");
            ImageUrl = "https://assets.braintreegateway.com/payment_method_logo/unknown.png";
            CustomerId = node.GetString("customer-id");
        }

        public UnknownPaymentMethod()
        {
        }
    }
}
