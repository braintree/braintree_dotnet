using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Braintree.Tests
{
    [TestFixture]
    public class BraintreeServiceTest
    {
        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(Braintree.Exceptions.AuthenticationException))]
        public void SandboxSSLCertificateSuccessful()
        {
            new BraintreeService(new Configuration(Environment.SANDBOX, "dummy", "dummy", "dummy")).Get("/");
        }

        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(Braintree.Exceptions.AuthenticationException))]
        public void ProductionSSLCertificateSuccessful()
        {
            ServicePointManager.ServerCertificateValidationCallback = TrustAllCertificates;
            new BraintreeService(new Configuration(Environment.PRODUCTION, "dummy", "dummy", "dummy")).Get("/");
        }

        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(Braintree.Exceptions.UpgradeRequiredException))]
        public void ThrowExceptionIfErrorStatusCodeIsUpgradeRequired()
        {
            BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 426, null);
        }

        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(Braintree.Exceptions.TooManyRequestsException))]
        public void ThrowExceptionIfErrorStatusCodeIsTooManyRequests()
        {
            BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 429, null);
        }

        [Test]
        [Category("Unit")]
        [ExpectedException(typeof(Braintree.Exceptions.DownForMaintenanceException))]
        public void ThrowExceptionIfErrorStatusCodeIsDownForMaintenance()
        {
            BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 503, null);
        }

        private static bool TrustAllCertificates(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
        {
           // to work around this bug which is yet to be released in mono
           //  https://bugzilla.novell.com/show_bug.cgi?id=606002
           return true;
        }

    }
}
