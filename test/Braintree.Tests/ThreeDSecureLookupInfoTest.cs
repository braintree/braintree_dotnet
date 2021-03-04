using Newtonsoft.Json;
using NUnit.Framework;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class ThreeDSecureLookupInfoTest
    {

        [Test]
        public void ConstructFromXMLResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<lookup>");
            builder.Append("  <trans-status>status</trans-status>");
            builder.Append("  <trans-status-reason>reason</trans-status-reason>");
            builder.Append("</lookup>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            ThreeDSecureLookupInfo info = new ThreeDSecureLookupInfo(new NodeWrapper(doc).GetNode("//lookup"));
            Assert.IsInstanceOf(typeof(ThreeDSecureLookupInfo), info);
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
            ThreeDSecureLookupInfo info = new ThreeDSecureLookupInfo(json);

            Assert.IsInstanceOf(typeof(ThreeDSecureLookupInfo), info);
            Assert.AreEqual("status", info.TransStatus);
            Assert.AreEqual("reason", info.TransStatusReason);
        }
    }
}