#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class PartnerConfiguration : Configuration
    {
        public PartnerConfiguration() {}

        public PartnerConfiguration(Environment environment, string partnerId, string publicKey, string privateKey) : base(environment, partnerId, publicKey, privateKey) {}
    }
}
