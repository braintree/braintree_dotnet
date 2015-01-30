using System;

namespace Braintree
{
    public class CoinbaseDetails
    {
        public String UserId { get; protected set; }
        public String UserEmail { get; protected set; }
        public String UserName { get; protected set; }
        public String Token { get; protected set; }

        protected internal CoinbaseDetails(NodeWrapper node)
        {
            UserId = node.GetString("user-id");
            UserEmail = node.GetString("user-email");
            UserName = node.GetString("user-name");
            Token = node.GetString("token");
        }

    }
}
