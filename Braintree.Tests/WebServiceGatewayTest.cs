using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Braintree
{
    [TestFixture]
    class WebServiceGatewayTest
    {
        [Test]
        public void SslCertificateSuccessful()
        {
            Configuration.Environment = Environment.QA;
            Configuration.MerchantId = "integration_merchant_id";
            Configuration.PublicKey = "integration_public_key";
            Configuration.PrivateKey = "integration_private_key";

            WebServiceGateway.Get("/customers");
        }
    }
}
