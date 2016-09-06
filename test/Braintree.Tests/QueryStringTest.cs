using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class QueryStringTest
    {
        [Test]
        public void Append()
        {
            string actual = new QueryString().Append("foo", "f").Append("bar", "b").ToString();
            Assert.AreEqual("foo=f&bar=b", actual);
        }

        [Test]
        public void AppendEmptyStringOrNulls()
        {
            string actual = new QueryString().
                Append("foo", "f").
                Append("", "b").
                Append("bar", "").
                Append("boom", null).
                Append("", "c").ToString();

            Assert.AreEqual("foo=f&bar=", actual);
        }

        [Test]
        public void AppendOtherObjectsWithCanBeConvertedToStrings()
        {
            string actual = new QueryString().
                Append("foo", 10).
                Append("bar", "20.00").ToString();

            Assert.AreEqual("foo=10&bar=20.00", actual);
        }

        [Test]
        public void AppendWithRequest()
        {
            Request request = new CreditCardRequest
            {
                CVV = "123",
                CardholderName = "Drew"
            };

            string actual = new QueryString().Append("[credit_card]", request).ToString();
            Assert.AreEqual("%5Bcredit_card%5D%5Bcardholder_name%5D=Drew&%5Bcredit_card%5D%5Bcvv%5D=123", actual);
        }

        [Test]
        public void AppendWithNestedRequest()
        {
            Request request = new CreditCardRequest
            {
                CVV = "123",
                CardholderName = "Drew",
                Options = new CreditCardOptionsRequest
                {
                    VerifyCard = true,
                    MakeDefault = true
                }
            };
            
            string actual = new QueryString().Append("[credit_card]", request).ToString();
            Assert.AreEqual("%5Bcredit_card%5D%5Bcardholder_name%5D=Drew&%5Bcredit_card%5D%5Bcvv%5D=123&%5Bcredit_card%5D%5Boptions%5D%5Bmake_default%5D=true&%5Bcredit_card%5D%5Boptions%5D%5Bverify_card%5D=true", actual);
        }
    }
}
