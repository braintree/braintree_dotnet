using System;

namespace Braintree
{
    public class ConnectedMerchantStatusTransitioned
    {
        public virtual string MerchantPublicId { get; private set; }
        public virtual string MerchantId => MerchantPublicId;
        public virtual string OAuthApplicationClientId { get; private set; }
        public virtual string Status { get; private set; }

        protected internal ConnectedMerchantStatusTransitioned(NodeWrapper node)
        {
            MerchantPublicId = node.GetString("merchant-public-id");
            OAuthApplicationClientId = node.GetString("oauth-application-client-id");
            Status = node.GetString("status");
        }
    }
}

