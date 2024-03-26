using NUnit.Framework;

using System;

namespace Braintree.Tests
{
    [TestFixture]
    public class ClientTokenRequestTest
    {
        [Test]
        [System.Obsolete]
        public void ToXml_Includes_All_Values()
        {
            var request = new ClientTokenRequest()
            {
                CustomerId = "abc123",
                Domains = new string[] {"example.com"},
                MerchantAccountId = "987654321",
                Version = 2
            };

            Assert.IsTrue(request.ToXml().Contains("<customer-id>abc123</customer-id>"));
            Assert.IsTrue(request.ToXml().Contains("<domains type=\"array\"><item>example.com</item></domains>"));
            Assert.IsTrue(request.ToXml().Contains("<merchant-account-id>987654321</merchant-account-id>"));
            Assert.IsTrue(request.ToXml().Contains("<version>2</version>"));
        }
    }
}