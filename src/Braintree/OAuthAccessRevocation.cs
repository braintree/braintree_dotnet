namespace Braintree
{
    public class OAuthAccessRevocation
    {
        public virtual string MerchantId { get; protected set; }

        public OAuthAccessRevocation(NodeWrapper node)
        {
            MerchantId = node.GetString("merchant-id");
        }
    }
}
