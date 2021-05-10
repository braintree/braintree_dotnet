using System;

namespace Braintree
{
    public class DisputePayPalMessage
    {
        public virtual string Message { get; protected set; }
        public virtual string Sender { get; protected set; }
        public virtual DateTime? SentAt { get; protected set; }

        public DisputePayPalMessage(NodeWrapper node)
        {
            Message = node.GetString("message");
            Sender = node.GetString("sender");
            SentAt = node.GetDateTime("sent-at");
        }

        [Obsolete("Mock Use Only")]
        protected internal DisputePayPalMessage() { }
    }
}
