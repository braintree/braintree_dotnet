using System;

namespace Braintree
{
    public class PartnerMerchant
    {
        public virtual string MerchantPublicId { get; protected set; }
        public virtual string PublicKey { get; protected set; }
        public virtual string PrivateKey { get; protected set; }
        public virtual string PartnerMerchantId { get; protected set; }
        public virtual string ClientSideEncryptionKey { get; protected set; }

        protected internal PartnerMerchant(NodeWrapper node)
        {
            MerchantPublicId = node.GetString("merchant-public-id");
            PublicKey = node.GetString("public-key");
            PrivateKey = node.GetString("private-key");
            PartnerMerchantId = node.GetString("partner-merchant-id");
            ClientSideEncryptionKey = node.GetString("client-side-encryption-key");
        }

        [Obsolete("Mock Use Only")]
        protected internal PartnerMerchant() { }
    }
}

