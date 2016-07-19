using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using System.Net.Security;
using Braintree.Exceptions;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace Braintree.Tests
{
    [TestFixture]
    public class BraintreeServiceTest
    {
        [Test]
        [Category("Unit")]
        public void SandboxSSLCertificateSuccessful()
        {
            Assert.Throws<AuthenticationException>(() => new BraintreeService(new Configuration(Environment.SANDBOX, "dummy", "dummy", "dummy")).Get("/"));
        }

        [Test]
        [Category("Unit")]
        public void ProductionSSLCertificateSuccessful()
        {
            new WinHttpHandler().ServerCertificateValidationCallback = TrustAllCertificates;
            Assert.Throws<AuthenticationException>(() => new BraintreeService(new Configuration(Environment.PRODUCTION, "dummy", "dummy", "dummy")).Get("/"));
        }

        [Test]
        [Category("Unit")]
        public void ThrowExceptionIfErrorStatusCodeIsUpgradeRequired()
        {
            Assert.Throws<UpgradeRequiredException>(() => BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 426, null));
        }

        [Test]
        [Category("Unit")]
        public void ThrowExceptionIfErrorStatusCodeIsTooManyRequests()
        {
            Assert.Throws<TooManyRequestsException>(()=>BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 429, null));
        }

        [Test]
        [Category("Unit")]
        public void ThrowExceptionIfErrorStatusCodeIsDownForMaintenance()
        {
            Assert.Throws<DownForMaintenanceException>(() => BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 503, null));
        }

        private static bool TrustAllCertificates(object sender, X509Certificate x509Certificate, X509Chain x509Chain, SslPolicyErrors sslPolicyErrors)
        {
           // to work around this bug which is yet to be released in mono
           //  https://bugzilla.novell.com/show_bug.cgi?id=606002
           return true;
        }

    }
}
