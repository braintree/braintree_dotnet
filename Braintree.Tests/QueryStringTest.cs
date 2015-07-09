using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;

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
            Assert.AreEqual("%5bcredit_card%5d%5bcardholder_name%5d=Drew&%5bcredit_card%5d%5bcvv%5d=123", actual);
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
            Assert.AreEqual("%5bcredit_card%5d%5bcardholder_name%5d=Drew&%5bcredit_card%5d%5bcvv%5d=123&%5bcredit_card%5d%5boptions%5d%5bmake_default%5d=true&%5bcredit_card%5d%5boptions%5d%5bverify_card%5d=true", actual);
        }
    }
}
