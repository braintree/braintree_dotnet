using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionCreditCardRequestTest
    {
        [Test]
        public void ToXml_Includes_Token()
        {
            TransactionCreditCardRequest request = new TransactionCreditCardRequest();
            request.Token = "my-token";
            Assert.IsTrue(request.ToXml().Contains("my-token"));
        }

        [Test]
        public void ToXml_Includes_NetworkTokenizationAttributesRequest()
        {
            TransactionCreditCardRequest request = new TransactionCreditCardRequest();
            request.NetworkTokenizationAttributes = new NetworkTokenizationAttributesRequest();
            request.NetworkTokenizationAttributes.Cryptogram = "validcryptogram";
            request.NetworkTokenizationAttributes.EcommerceIndicator = "05";
            request.NetworkTokenizationAttributes.TokenRequestorId = "123456";

            Assert.IsTrue(request.ToXml().Contains("validcryptogram"));
            Assert.IsTrue(request.ToXml().Contains("05"));
            Assert.IsTrue(request.ToXml().Contains("123456"));
        }
    }
}
