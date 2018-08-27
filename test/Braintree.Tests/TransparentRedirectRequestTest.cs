using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Net;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransparentRedirectRequestTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            service = new BraintreeService(gateway.Configuration);
        }

        [Test]
        public void TransparentRedirectURL_ReturnsCorrectValue()
        {
            Assert.AreEqual(service.BaseMerchantURL() + "/transparent_redirect_requests",
                    gateway.TransparentRedirect.Url);
        }

        [Test]
        public void Constructor_RaisesForgedQueryStringExceptionIfGivenInvalidQueryString()
        {
            Assert.Throws<ForgedQueryStringException>(() => new TransparentRedirectRequest("http_status=200&id=7kdj469tw7yck32j&hash=99c9ff20cd7910a1c1e793ff9e3b2d15586dc6b9" + "this makes it invalid", service));
        }

        [Test]
        public void Constructor_RaisesNotFoundExceptionIfStatusIs500()
        {
            Assert.Throws<ServerException>(() => new TransparentRedirectRequest("http_status=500&id=6kdj469tw7yck32j&hash=a839a44ca69d59a3d6f639c294794989676632dc", service));
        }

        [Test]
        public void Constructor_RaisesNotFoundExceptionIfStatusIs500WithLeadQuestionMark()
        {
           Assert.Throws<ServerException>(() => new TransparentRedirectRequest("?http_status=500&id=6kdj469tw7yck32j&hash=a839a44ca69d59a3d6f639c294794989676632dc", service));
        }

        [Test]
        public void Constructor_RaisesAuthorizationExceptionIfStatusIs403()
        {
            Assert.Throws<AuthorizationException>(() => new TransparentRedirectRequest("http_status=403&id=6kdj469tw7yck32j&hash=a839a44ca69d59a3d6f639c294794989676632dc", service));
        }

        [Test]
        public void Constructor_RaisesAuthorizationExceptionWithMessageIfStatusIs403AndBtMessageIsInQueryString()
        {
            string message = "Invalid params: transaction[bad]";
            Exception exception = null;

            try {
                new TransparentRedirectRequest(string.Format("http_status=403&bt_message={0}&id=6kdj469tw7yck32j&hash=a839a44ca69d59a3d6f639c294794989676632dc", WebUtility.UrlEncode(message)), service);
            } catch (Exception localException) {
               exception = localException;
            }

            Assert.IsNotNull(exception);
            Assert.IsInstanceOf(typeof(AuthorizationException),exception);
            Assert.AreEqual(message, exception.Message);
        }
    }
}
