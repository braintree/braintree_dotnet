using System;

namespace Braintree
{
    public class ConnectedMerchantPayPalStatusChanged
    {
        public virtual string MerchantPublicId { get; private set; }
        public virtual string MerchantId {
            get { return MerchantPublicId; }
        }
        public virtual string OAuthApplicationClientId { get; private set; }
        public virtual string Action { get; private set; }

        protected internal ConnectedMerchantPayPalStatusChanged(NodeWrapper node)
        {
            MerchantPublicId = node.GetString("merchant-public-id");
            OAuthApplicationClientId = node.GetString("oauth-application-client-id");
            Action = node.GetString("action");
        }
    }
}

