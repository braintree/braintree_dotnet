using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class PartnerConfigurationTest
    {
        [Test]
        [Category("Unit")]
        public void MerchantId_SetProperly()
        {
            Configuration config = new PartnerConfiguration
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "partnerid",
                PublicKey = "publickey",
                PrivateKey = "privatekey"
            };

            Assert.AreEqual("partnerid", config.MerchantId);
        }
    }
}
