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
    }
}
