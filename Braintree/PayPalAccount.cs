using System;

namespace Braintree
{
    public class PayPalAccount : PaymentMethod
    {
        public String Email { get; protected set; }
        public String Token { get; protected set; }
        public Boolean? IsDefault { get; protected set; }
        public String ImageUrl { get; protected set; }

        protected internal PayPalAccount(NodeWrapper node)
        {
            Email = node.GetString("email");
            Token = node.GetString("token");
            IsDefault = node.GetBoolean("default");
            ImageUrl = node.GetString("image-url");
        }

    }
}
