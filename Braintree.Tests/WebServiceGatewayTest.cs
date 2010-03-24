using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class WebServiceGatewayTest
    {
        [Test]
        public void SslCertificateSuccessful()
        {
            Configuration.Environment = Environment.QA;
            Configuration.MerchantId = "test_merchant_id";
            Configuration.PublicKey = "test_public_key";
            Configuration.PrivateKey = "test_private_key";

            WebServiceGateway.Get("/customers");
        }
    }
}
