using Newtonsoft.Json;
using NUnit.Framework;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class ThreeDSecureInfoTest
    {

        [Test]
        public void ConstructFromXMLResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<three-d-secure-info>");
            builder.Append("<liability-shifted>true</liability-shifted>");
            builder.Append("<liability-shift-possible>true</liability-shift-possible>");
            builder.Append("<enrolled>Y</enrolled>");
            builder.Append("<status>status</status>");
            builder.Append("<cavv>imacavv</cavv>");
            builder.Append("<eci-flag>05</eci-flag>");
            builder.Append("<xid>1234</xid>");
            builder.Append("<three-d-secure-version>2.0.0</three-d-secure-version>");
            builder.Append("<ds-transaction-id>5678</ds-transaction-id>");
            builder.Append("<three-d-secure-authentication-id>09</three-d-secure-authentication-id>");
            builder.Append("<acs-transaction-id>ACS123</acs-transaction-id>");
            builder.Append("<pares-status>Y</pares-status>");
            builder.Append("<three-d-secure-server-transaction-id>3DS456</three-d-secure-server-transaction-id>");
            builder.Append("<lookup>");
            builder.Append("  <trans-status>status</trans-status>");
            builder.Append("  <trans-status-reason>reason</trans-status-reason>");
            builder.Append("</lookup>");
            builder.Append("<authentication>");
            builder.Append("  <trans-status>status</trans-status>");
            builder.Append("  <trans-status-reason>reason</trans-status-reason>");
            builder.Append("</authentication>");
            builder.Append("</three-d-secure-info>");


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            ThreeDSecureInfo info = new ThreeDSecureInfo(new NodeWrapper(doc).GetNode("//three-d-secure-info"));
            Assert.IsTrue(info.LiabilityShifted);
            Assert.IsTrue(info.LiabilityShiftPossible);
            Assert.AreEqual("Y", info.Enrolled);
            Assert.AreEqual("status", info.Status);
            Assert.AreEqual("imacavv", info.Cavv);
            Assert.AreEqual("05", info.EciFlag);
            Assert.AreEqual("1234", info.Xid);
            Assert.AreEqual("2.0.0", info.ThreeDSecureVersion);
            Assert.AreEqual("5678", info.DsTransactionId);
            Assert.AreEqual("09", info.ThreeDSecureAuthenticationId);
            Assert.AreEqual("ACS123", info.AcsTransactionId);
            Assert.AreEqual("Y", info.ParesStatus);
            Assert.AreEqual("3DS456", info.ThreeDSecureServerTransactionId);
            Assert.IsInstanceOf(typeof(ThreeDSecureLookupInfo), info.Lookup);
            Assert.AreEqual("status", info.Lookup.TransStatus);
            Assert.AreEqual("reason", info.Lookup.TransStatusReason);
            Assert.IsInstanceOf(typeof(ThreeDSecureAuthenticationInfo), info.Authentication);
            Assert.AreEqual("status", info.Authentication.TransStatus);
            Assert.AreEqual("reason", info.Authentication.TransStatusReason);
        }

        [Test]
        public void ConstructFromDynamicObject()
        {
            var rawJSON = @"{
'liabilityShifted':'true',
'liabilityShiftPossible':'true',
'status':'status',
'enrolled':'Y',
'cavv':'imacavv',
'xid':'1234',
'acsTransactionId':'ACS123',
'dsTransactionId':'5678',
'eciFlag':'05',
'paresStatus':'Y',
'threeDSecureAuthenticationId':'09',
'threeDSecureServerTransactionId':'3DS456',
'threeDSecureVersion':'2.0.0',
}";

            dynamic json = JsonConvert.DeserializeObject<dynamic>(rawJSON);
            ThreeDSecureInfo info = new ThreeDSecureInfo(json);

            Assert.IsTrue(info.LiabilityShifted);
            Assert.IsTrue(info.LiabilityShiftPossible);
            Assert.AreEqual("Y", info.Enrolled);
            Assert.AreEqual("status", info.Status);
            Assert.AreEqual("imacavv", info.Cavv);
            Assert.AreEqual("05", info.EciFlag);
            Assert.AreEqual("1234", info.Xid);
            Assert.AreEqual("2.0.0", info.ThreeDSecureVersion);
            Assert.AreEqual("5678", info.DsTransactionId);
            Assert.AreEqual("09", info.ThreeDSecureAuthenticationId);
            Assert.AreEqual("ACS123", info.AcsTransactionId);
            Assert.AreEqual("Y", info.ParesStatus);
            Assert.AreEqual("3DS456", info.ThreeDSecureServerTransactionId);
            Assert.IsInstanceOf(typeof(ThreeDSecureLookupInfo), info.Lookup);
            Assert.IsNull(info.Lookup.TransStatus);
            Assert.IsNull(info.Lookup.TransStatusReason);
            Assert.IsInstanceOf(typeof(ThreeDSecureAuthenticationInfo), info.Authentication);
            Assert.IsNull(info.Authentication.TransStatus);
            Assert.IsNull(info.Authentication.TransStatusReason);

        }
    }
}