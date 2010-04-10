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
        [SetUp]
        public void Setup()
        {
            Configuration.Environment = Environment.DEVELOPMENT;
            Configuration.MerchantId = "integration_merchant_id";
            Configuration.PublicKey = "integration_public_key";
            Configuration.PrivateKey = "integration_private_key";
        }

        [Test]
        public void IsValidTrQueryString_ForValidString()
        {
            String queryString = "http_status=200&id=6kdj469tw7yck32j&hash=99c9ff20cd7910a1c1e793ff9e3b2d15586dc6b9";
            Assert.IsTrue(TrUtil.IsValidTrQueryString(queryString));
        }

        [Test]
        public void IsValidTrQueryString_ForValidStringWithQuestionMarke()
        {
            String queryString = "?http_status=200&id=6kdj469tw7yck32j&hash=99c9ff20cd7910a1c1e793ff9e3b2d15586dc6b9";
            Assert.IsTrue(TrUtil.IsValidTrQueryString(queryString));
        }

        [Test]
        public void IsValidTrQueryString_ForInvalidString()
        {
            String queryString = "http_status=200&id=6kdj469tw7yck32j&hash=99c9ff20cd7910a1c1e793ff9e3b2d15586dc6b8";
            Assert.IsFalse(TrUtil.IsValidTrQueryString(queryString));
        }
    }
}
