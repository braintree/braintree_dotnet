using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Braintree.Tests
{
    //NOTE: good
    [TestFixture]
    public class BraintreeServiceTest
    {
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
            ServicePointManager.ServerCertificateValidationCallback = TrustAllCertificates;
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

        private static bool TrustAllCertificates(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
        {
           // to work around this bug which is yet to be released in mono
           //  https://bugzilla.novell.com/show_bug.cgi?id=606002
           return true;
        }

    }
}
