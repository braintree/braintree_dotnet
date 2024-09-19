using NUnit.Framework;
using System;

namespace Braintree.Tests
{
    [TestFixture]
    public class NetworkTokenizationAttributesRequestTest
    {
        [Test]
        public void ToXml_IncludesAllProperties()
        {
            var request = new NetworkTokenizationAttributesRequest()
            {
                Cryptogram = "validcryptogram",
                EcommerceIndicator = "05",
                TokenRequestorId = "123456"
            };
            
            Assert.IsTrue(request.ToXml("network-tokenization-attributes").Contains("<cryptogram>validcryptogram</cryptogram>"));
            Assert.IsTrue(request.ToXml("network-tokenization-attributes").Contains("<ecommerce-indicator>05</ecommerce-indicator>"));
            Assert.IsTrue(request.ToXml("network-tokenization-attributes").Contains("<token-requestor-id>123456</token-requestor-id>"));
        }
    }
}
