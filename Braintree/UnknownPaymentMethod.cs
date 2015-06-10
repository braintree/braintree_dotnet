using System;

namespace Braintree
{
    public class UnknownPaymentMethod : PaymentMethod
    {
        public string Token { get; protected set; }
        public Boolean? IsDefault { get; protected set; }
        public string ImageUrl { get; protected set; }

        public UnknownPaymentMethod(NodeWrapper node)
        {
            Token = node.GetString("token");
            IsDefault = node.GetBoolean("default");
            ImageUrl = "https://assets.braintreegateway.com/payment_method_logo/unknown.png";
        }

        public UnknownPaymentMethod()
        {
        }
    }
}
