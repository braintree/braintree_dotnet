using System;

namespace Braintree
{
    public class CoinbaseDetails
    {
        public string UserId { get; protected set; }
        public string UserEmail { get; protected set; }
        public string UserName { get; protected set; }
        public string Token { get; protected set; }

        protected internal CoinbaseDetails(NodeWrapper node)
        {
            UserId = node.GetString("user-id");
            UserEmail = node.GetString("user-email");
            UserName = node.GetString("user-name");
            Token = node.GetString("token");
        }

    }
}
