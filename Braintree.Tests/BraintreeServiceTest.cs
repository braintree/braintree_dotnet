using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class BraintreeServiceTest
    {
        [Test]
        public void QASSLCertificateSuccessful()
        {
            try {
                new BraintreeService(new Configuration(Environment.QA, "dummy", "dummy", "dummy")).Get("/");
                Assert.Fail ("Expected an AuthenticationException but none was thrown.");
            } catch (Braintree.Exceptions.AuthenticationException) {
                // expected
            }
        }

        [Test]
        public void SandboxSSLCertificateSuccessful()
        {
            try {
                new BraintreeService(new Configuration(Environment.SANDBOX, "dummy", "dummy", "dummy")).Get("/");
                Assert.Fail ("Expected an AuthenticationException but none was thrown.");
            } catch (Braintree.Exceptions.AuthenticationException) {
                // expected
            }
        }

        [Test]
        public void ProductionSSLCertificateSuccessful()
        {
            try {
                new BraintreeService(new Configuration(Environment.PRODUCTION, "dummy", "dummy", "dummy")).Get("/");
                Assert.Fail ("Expected an AuthenticationException but none was thrown.");
            } catch (Braintree.Exceptions.AuthenticationException) {
                // expected
            }
        }

        [Test]
        public void ThrowExceptionIfErrorStatusCodeIsUpgradeRequired()
        {
            try {
                BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 426, null);
                Assert.Fail ("Expected an AuthenticationException but none was thrown.");
            } catch (Braintree.Exceptions.UpgradeRequiredException) {
                // expected
            }
        }
    }
}
