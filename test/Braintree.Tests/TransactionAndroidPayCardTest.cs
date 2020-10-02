using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionAndroidPayCardTest
    {
        [Test]
        public void ToXml_IncludesAllProperties()
        {
            TransactionAndroidPayCardRequest request = new TransactionAndroidPayCardRequest
            {
                Cryptogram = "some-cryptogram",
                EciIndicator = "some-eci-indicator",
                ExpirationMonth = "some-month",
                ExpirationYear = "some-year",
                GoogleTransactionId = "some-id",
                Number = "some-number",
                SourceCardLastFour = "some-last-four",
                SourceCardType = "some-card-type"
            };

            StringAssert.Contains("<cryptogram>some-cryptogram</cryptogram>", request.ToXml());
            StringAssert.Contains("<eci-indicator>some-eci-indicator</eci-indicator>", request.ToXml());
            StringAssert.Contains("<expiration-month>some-month</expiration-month>", request.ToXml());
            StringAssert.Contains("<expiration-year>some-year</expiration-year>", request.ToXml());
            StringAssert.Contains("<google-transaction-id>some-id</google-transaction-id>", request.ToXml());
            StringAssert.Contains("<number>some-number</number>", request.ToXml());
            StringAssert.Contains("<source-card-last-four>some-last-four</source-card-last-four>", request.ToXml());
            StringAssert.Contains("<source-card-type>some-card-type</source-card-type>", request.ToXml());
        }
    }
}
