using System;

namespace Braintree
{
    public class PaymentMethodNonceDetailsPayerInfo
    {
        public virtual string Email { get; protected set; }
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string PayerId { get; protected set; }
        public virtual string CountryCode { get; protected set; }

        protected internal PaymentMethodNonceDetailsPayerInfo(NodeWrapper node)
        {
            Email = node.GetString("email");
            FirstName = node.GetString("first-name");
            LastName = node.GetString("last-name");
            PayerId = node.GetString("payer-id");
            CountryCode = node.GetString("country-code");
        }

        [Obsolete("Mock Use Only")]
        protected internal PaymentMethodNonceDetailsPayerInfo() { }
    }
}
