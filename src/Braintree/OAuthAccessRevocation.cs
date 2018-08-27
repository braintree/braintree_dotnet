namespace Braintree
{
    public class OAuthAccessRevocation
    {
        public virtual string MerchantId { get; protected set; }
        public virtual string OAuthApplicationClientId { get; protected set; }

        public OAuthAccessRevocation(NodeWrapper node)
        {
            MerchantId = node.GetString("merchant-id");
            OAuthApplicationClientId = node.GetString("oauth-application-client-id");
        }
    }
}
