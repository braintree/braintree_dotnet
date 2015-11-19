using System;

namespace Braintree
{
    public class CoinbaseDetails
    {
        public virtual string UserId { get; protected set; }
        public virtual string UserEmail { get; protected set; }
        public virtual string UserName { get; protected set; }
        public virtual string Token { get; protected set; }

        protected internal CoinbaseDetails(NodeWrapper node)
        {
            UserId = node.GetString("user-id");
            UserEmail = node.GetString("user-email");
            UserName = node.GetString("user-name");
            Token = node.GetString("token");
        }

        [Obsolete("Mock Use Only")]
        protected internal CoinbaseDetails() { }
    }
}
