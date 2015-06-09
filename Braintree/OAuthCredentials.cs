using System;

namespace Braintree
{
    public class OAuthCredentials
    {

        public OAuthCredentials(NodeWrapper node)
        {
            if (node == null) return;

            AccessToken = node.GetString("access-token");
            RefreshToken = node.GetString("refresh-token");
            TokenType = node.GetString("token-type");
            ExpiresAt = node.GetDateTime("expires-at");
        }

        public String AccessToken
        {
            get; set;
        }

        public String RefreshToken
        {
            get; set;
        }

        public String TokenType
        {
            get; set;
        }

        public DateTime? ExpiresAt
        {
            get; set;
        }
    }
}
