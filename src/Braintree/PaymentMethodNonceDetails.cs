using System;

namespace Braintree
{
    public class PaymentMethodNonceDetails
    {
        public virtual string LastTwo { get; protected set; }
        public virtual string CardType { get; protected set; }

        protected internal PaymentMethodNonceDetails(NodeWrapper node)
        {
            CardType = node.GetString("card-type");
            LastTwo = node.GetString("last-two");
        }

        [Obsolete("Mock Use Only")]
        protected internal PaymentMethodNonceDetails() { }
    }
}
