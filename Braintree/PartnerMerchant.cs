using System;

namespace Braintree
{
    public class PartnerMerchant
    {
        public String MerchantPublicId { get; protected set; }
        public String PublicKey { get; protected set; }
        public String PrivateKey { get; protected set; }
        public String PartnerMerchantId { get; protected set; }
        public String ClientSideEncryptionKey { get; protected set; }

        protected internal PartnerMerchant(NodeWrapper node)
        {
            MerchantPublicId = node.GetString("merchant-public-id");
            PublicKey = node.GetString("public-key");
            PrivateKey = node.GetString("private-key");
            PartnerMerchantId = node.GetString("partner-merchant-id");
            ClientSideEncryptionKey = node.GetString("client-side-encryption-key");
        }
    }
}

