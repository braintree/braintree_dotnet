using Newtonsoft.Json;
using NUnit.Framework;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class ThreeDSecureAuthenticationInfoTest
    {
        [Test]
        public void ConstructFromXMLResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<authentication>");
            builder.Append("  <trans-status>status</trans-status>");
            builder.Append("  <trans-status-reason>reason</trans-status-reason>");
            builder.Append("</authentication>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            ThreeDSecureAuthenticationInfo info = new ThreeDSecureAuthenticationInfo(new NodeWrapper(doc).GetNode("//authentication"));
            Assert.IsInstanceOf(typeof(ThreeDSecureAuthenticationInfo), info);
            Assert.AreEqual("status", info.TransStatus);
            Assert.AreEqual("reason", info.TransStatusReason);
        }

        [Test]
        public void ConstructFromDynamicObject()
        {
            var rawJSON = @"{
'transStatus':'status',
'transStatusReason':'reason'
}";

            dynamic json = JsonConvert.DeserializeObject<dynamic>(rawJSON);
            ThreeDSecureAuthenticationInfo info = new ThreeDSecureAuthenticationInfo(json);

            Assert.IsInstanceOf(typeof(ThreeDSecureAuthenticationInfo), info);
            Assert.AreEqual("status", info.TransStatus);
            Assert.AreEqual("reason", info.TransStatusReason);
        }
    }
}
