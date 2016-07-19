using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class TrUtilTest
    {
        private BraintreeService service;

        [SetUp]
        public void Setup()
        {
            service = new BraintreeService(new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            ));
        }

        [Test]
        [Category("Unit")]
        public void IncludesApiVersion()
        {
            string tr_data = TrUtil.BuildTrData(new TransactionRequest(), "example.com", service);
            TestHelper.AssertIncludes("api_version=4", tr_data);
        }

        [Test]
        [Category("Unit")]
        public void IncludesKind()
        {
            string tr_data = TrUtil.BuildTrData(new TransactionRequest(), "example.com", service);
            TestHelper.AssertIncludes("kind=create_transaction", tr_data);
        }

        [Test]
        [Category("Unit")]
        public void IsValidTrQueryString_ForValidString()
        {
            string queryString = "http_status=200&id=6kdj469tw7yck32j&hash=99c9ff20cd7910a1c1e793ff9e3b2d15586dc6b9";
            Assert.IsTrue(TrUtil.IsValidTrQueryString(queryString, service));
        }

        [Test]
        [Category("Unit")]
        public void IsValidTrQueryString_ForValidStringWithQuestionMarke()
        {
            string queryString = "?http_status=200&id=6kdj469tw7yck32j&hash=99c9ff20cd7910a1c1e793ff9e3b2d15586dc6b9";
            Assert.IsTrue(TrUtil.IsValidTrQueryString(queryString, service));
        }

        [Test]
        [Category("Unit")]
        public void IsValidTrQueryString_ForInvalidString()
        {
            string queryString = "http_status=200&id=6kdj469tw7yck32j&hash=99c9ff20cd7910a1c1e793ff9e3b2d15586dc6b8";
            Assert.IsFalse(TrUtil.IsValidTrQueryString(queryString, service));
        }
    }
}
