using System;

namespace Braintree
{
    public class PaymentMethodNonceDetails
    {
        public virtual string Bin { get; protected set; }
        public virtual string LastTwo { get; protected set; }
        public virtual string LastFour { get; protected set; }
        public virtual string CardType { get; protected set; }
        public virtual string Username { get; protected set; }
        public virtual string VenmoUserId { get; protected set; }

        protected internal PaymentMethodNonceDetails(NodeWrapper node)
        {
            Bin = node.GetString("bin");
            CardType = node.GetString("card-type");
            LastTwo = node.GetString("last-two");
            LastFour = node.GetString("last-four");
            Username = node.GetString("username");
            VenmoUserId = node.GetString("venmo-user-id");
        }

        [Obsolete("Mock Use Only")]
        protected internal PaymentMethodNonceDetails() { }
    }
}
