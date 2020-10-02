using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionApplePayCardRequestTest
    {
        [Test]
        public void ToXml_IncludesAllProperties()
        {
            var request = new TransactionApplePayCardRequest()
            {
                Number = "4111111111111111",
                CardholderName = "Dan Schulman",
                Cryptogram = "abc123",
                ExpirationMonth = "01",
                ExpirationYear = "19",
                EciIndicator = "07"
            };

            Assert.IsTrue(request.ToXml().Contains("<number>4111111111111111</number>"));
            Assert.IsTrue(request.ToXml().Contains("<cardholder-name>Dan Schulman</cardholder-name>"));
            Assert.IsTrue(request.ToXml().Contains("<cryptogram>abc123</cryptogram>"));
            Assert.IsTrue(request.ToXml().Contains("<expiration-month>01</expiration-month>"));
            Assert.IsTrue(request.ToXml().Contains("<expiration-year>19</expiration-year>"));
            Assert.IsTrue(request.ToXml().Contains("<eci-indicator>07</eci-indicator>"));
        }
    }
}
