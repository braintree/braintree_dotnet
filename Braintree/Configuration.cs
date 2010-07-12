#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class Configuration
    {
        public Environment Environment { get; set; }
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        public Configuration() {}

        public Configuration(Environment environment, string merchantId, string publicKey, string privateKey)
        {
            Environment = environment;
            MerchantId = merchantId;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }
}
