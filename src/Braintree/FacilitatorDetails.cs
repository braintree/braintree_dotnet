using System;

namespace Braintree
{
    public class FacilitatorDetails
    {
        public virtual string OauthApplicationClientId { get; protected set; }
        public virtual string OauthApplicationName { get; protected set; }
        public virtual string SourcePaymentMethodToken { get; protected set; }

        public FacilitatorDetails(NodeWrapper node)
        {
            if (node == null)
                return;

            OauthApplicationClientId = node.GetString("oauth-application-client-id");
            OauthApplicationName = node.GetString("oauth-application-name");
            SourcePaymentMethodToken = node.GetString("source-payment-method-token");
        }
    }
}

