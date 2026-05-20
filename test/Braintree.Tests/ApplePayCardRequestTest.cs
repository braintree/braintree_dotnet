using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class ApplePayCardRequestTest
    {
        [Test]
        public void ToXml_IncludesAllProperties()
        {
            var request = new ApplePayCardRequest()
            {
                Number = "4111111111111111",
                CardholderName = "John Doe",
                Cryptogram = "ApplePayCryptogram123",
                ExpirationMonth = "12",
                ExpirationYear = "2025",
                EciIndicator = "7",
                NetworkTransactionId = "test123",
                Token = "apple_pay_token_123"
            };

            Assert.IsTrue(request.ToXml().Contains("<number>4111111111111111</number>"));
            Assert.IsTrue(request.ToXml().Contains("<cardholder-name>John Doe</cardholder-name>"));
            Assert.IsTrue(request.ToXml().Contains("<cryptogram>ApplePayCryptogram123</cryptogram>"));
            Assert.IsTrue(request.ToXml().Contains("<expiration-month>12</expiration-month>"));
            Assert.IsTrue(request.ToXml().Contains("<expiration-year>2025</expiration-year>"));
            Assert.IsTrue(request.ToXml().Contains("<eci-indicator>7</eci-indicator>"));
            Assert.IsTrue(request.ToXml().Contains("<network-transaction-id>test123</network-transaction-id>"));
            Assert.IsTrue(request.ToXml().Contains("<token>apple_pay_token_123</token>"));
        }
    }
}