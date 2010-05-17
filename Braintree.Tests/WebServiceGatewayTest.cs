using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class WebServiceGatewayTest
    {

        [SetUp]
        public void Setup()
        {
            Configuration.MerchantId = "dummy";
            Configuration.PublicKey = "dummy";
            Configuration.PrivateKey = "dummy";
        }

        [Test]
        public void QASSLCertificateSuccessful()
        {
            try {
                Configuration.Environment = Environment.QA;
                WebServiceGateway.Get("/");
                Assert.Fail ("Expected an AuthenticationException but none was thrown.");
            } catch (Braintree.Exceptions.AuthenticationException) {
                // expected
            }
        }

        [Test]
        public void SandboxSSLCertificateSuccessful()
        {
            try {
                Configuration.Environment = Environment.SANDBOX;
                WebServiceGateway.Get("/");
                Assert.Fail ("Expected an AuthenticationException but none was thrown.");
            } catch (Braintree.Exceptions.AuthenticationException) {
                // expected
            }
        }

        [Test]
        public void ProductionSSLCertificateSuccessful()
        {
            try {
                Configuration.Environment = Environment.PRODUCTION;
                WebServiceGateway.Get("/");
                Assert.Fail ("Expected an AuthenticationException but none was thrown.");
            } catch (Braintree.Exceptions.AuthenticationException) {
                // expected
            }
        }

        [Test]
        public void ThrowExceptionIfErrorStatusCodeIsUpgradeRequired()
        {
            try {
                WebServiceGateway.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 426, null);
                Assert.Fail ("Expected an AuthenticationException but none was thrown.");
            } catch (Braintree.Exceptions.UpgradeRequiredException) {
                // expected
            }
        }
    }
}
