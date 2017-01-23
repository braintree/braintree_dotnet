using System;

namespace Braintree
{
    public class AchMandate
    {
        public virtual string Text { get; protected set; }
        public virtual DateTime? AcceptedAt { get; protected set; }

        protected internal AchMandate(NodeWrapper node)
        {
            if (node == null) return;

            Text = node.GetString("text");
            AcceptedAt = node.GetDateTime("accepted-at");
        }

        [Obsolete("Mock Use Only")]
        protected AchMandate() { }
    }
}
