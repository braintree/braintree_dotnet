#pragma warning disable CS0618

using Braintree.Exceptions;
using NUnit.Framework;
using System;
using System.Net;
using System.IO;
#if netcore
using System.Net.Http;
#else
using System.IO.Compression;
#endif
using System.Reflection;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class BraintreeServiceTest
    {
        private Configuration configuration;
        private BraintreeService service;

        [SetUp]
        public void Setup()
        {
            configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            service = new BraintreeService(configuration);
        }

        [Test]
        public void SetRequestHeaders_SetsHeaders()
        {
            configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            BraintreeService service = new BraintreeService(configuration);

#if netcore
            var request = configuration.HttpRequestMessageFactory(new HttpMethod("GET"), "http://localhost:3000");
            service.SetRequestHeaders(request);
            var expectedUserAgent = "Braintree .NET " + typeof(BraintreeService).GetTypeInfo().Assembly.GetName().Version.ToString();
            Assert.AreEqual(expectedUserAgent, request.Headers.UserAgent.ToString());
            Assert.AreEqual("gzip", request.Headers.AcceptEncoding.ToString());
            Assert.AreEqual("application/xml", request.Headers.Accept.ToString());
#else
            var request = configuration.HttpWebRequestFactory("http://localhost:3000");
            service.SetRequestHeaders(request);
            var expectedUserAgent = "Braintree .NET " + typeof(BraintreeService).Assembly.GetName().Version.ToString();
            Assert.AreEqual(expectedUserAgent, request.Headers.Get("User-Agent"));
            Assert.AreEqual("gzip", request.Headers.Get("Accept-Encoding"));
            Assert.AreEqual("application/xml", request.Headers.Get("Accept"));
#endif
        }

        [Test]
        public void GetWebProxy_ReturnsNullIfNotSpecified()
        {
            BraintreeService service = new BraintreeService(new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            ));
            Assert.AreEqual(null, service.GetWebProxy());
        }

        [Test]
        public void GetWebProxy_ReturnsProxyConfiguration()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            configuration.WebProxy = new WebProxy(new Uri("http://localhost:3000"));
            BraintreeService service = new BraintreeService(configuration);
            Uri destination = new Uri("http://0.0.0.0");

            Assert.AreEqual("http://localhost:3000", service.GetWebProxy().GetProxy(destination).OriginalString);
        }

        [Test]
        public void WebProxy_ReturnsWebProxyConfiguration()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            configuration.WebProxy = new WebProxy(new Uri("http://localhost:3000"));
            BraintreeService service = new BraintreeService(configuration);
            Assert.AreEqual(configuration.WebProxy, service.GetWebProxy());
        }

        [Test]
        public void ThrowExceptionIfErrorStatusCodeIsUpgradeRequired()
        {
            Assert.Throws<UpgradeRequiredException>(() => BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 426, null));
        }

        [Test]
        public void ThrowExceptionIfErrorStatusCodeIsTooManyRequests()
        {
            Assert.Throws<TooManyRequestsException>(() => BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 429, null));
        }

        [Test]
        public void ThrowExceptionIfErrorStatusCodeIsDownForMaintenance()
        {
            Assert.Throws<DownForMaintenanceException> (() => BraintreeService.ThrowExceptionIfErrorStatusCode((HttpStatusCode) 503, null));
        }

        [Test]
        public void GetAuthorizationSchema_ReturnsBasicHeader()
        {
            string schema = service.GetAuthorizationSchema();
            Assert.AreEqual("Basic", schema);
        }

        [Test]
        public void GetAuthorizationHeader_ReturnsCredentials()
        {
            var headers = service.GetAuthorizationHeader();
            Assert.IsNotEmpty(headers);
        }

        [Test]
        public void ParseResponseStream_DoNotParseExternalEntities()
        {
#if netcore
            Assert.Throws<System.Xml.XmlException>(() =>  service.StringToXmlNode("<!DOCTYPE foo [  <!ELEMENT foo ANY > <!ENTITY xxe SYSTEM \"file:///etc/passwd\" >]><foo>&xxe;</foo>"));
#else
            var tempFilePath = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllText(tempFilePath, "Hello World!");
            Assert.IsTrue(System.IO.File.Exists(tempFilePath));
            Assert.IsTrue(new System.IO.FileInfo(tempFilePath).Length > 0);
            var rootNode = service.StringToXmlNode("<!DOCTYPE foo [  <!ELEMENT foo ANY > <!ENTITY xxe SYSTEM \"file://" + tempFilePath.Replace("\\", "//") + "\" >]><foo>&xxe;</foo>");
            Assert.IsEmpty(rootNode.InnerText);
#endif
        }
    }
}

#pragma warning restore CS0618
