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
            Scope = node.GetString("scope");
            ExpiresAt = node.GetDateTime("expires-at");
        }

        public string AccessToken
        {
            get; set;
        }

        public string RefreshToken
        {
            get; set;
        }

        public string TokenType
        {
            get; set;
        }

        public string Scope
        {
            get; set;
        }

        public DateTime? ExpiresAt
        {
            get; set;
        }
    }
}
