using NUnit.Framework;
using System;

namespace Braintree.Tests
{
    [TestFixture]
    public class WebProxyTest
    {
        [Test]
        public void GetProxy_IsSameForStringAndUriConstructors()
        {
            WebProxy proxy1 = new WebProxy("http://localhost:3000");
            WebProxy proxy2 = new WebProxy(new Uri("http://localhost:3000"));

            Uri uri = new Uri("http://0.0.0.0");

            Assert.AreEqual(proxy1.GetProxy(uri).OriginalString, proxy2.GetProxy(uri).OriginalString);
        }
    }
}
