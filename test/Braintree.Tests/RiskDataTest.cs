using NUnit.Framework;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class RiskDataTest
    {

        [Test]
        public void ConstructFromXMLResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<risk-data>");
            builder.Append("<decision>it is decided</decision>");
            builder.Append("<decision-reasons>x,y,z</decision-reasons>");
            builder.Append("<device-data-captured>true</device-data-captured>");
            builder.Append("<fraud-service-provider>provider</fraud-service-provider>");
            builder.Append("<id>imaid</id>");
            builder.Append("<transaction-risk-score>05</transaction-risk-score>");
            builder.Append("<liability-shift>");
            builder.Append("<responsible-party>paypal</responsible-party>");
            builder.Append("<conditions>unauthorized,item_not_received,</conditions>");
            builder.Append("</liability-shift>");
            builder.Append("</risk-data>");


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            RiskData data = new RiskData(new NodeWrapper(doc).GetNode("//risk-data"));
            Assert.IsTrue(data.deviceDataCaptured);
            Assert.AreEqual("it is decided", data.decision);
            Assert.AreEqual("provider", data.fraudServiceProvider);
            Assert.AreEqual("imaid", data.id);
            Assert.AreEqual("05", data.TransactionRiskScore);
            Assert.IsNotNull(data.DecisionReasons);
            Assert.IsNotNull(data.LiabilityShift);
            Assert.AreEqual("paypal", data.LiabilityShift.ResponsibleParty);
        }

        [Test]
        public void DoesNotIncludeLiabilityShiftIfNotPresent()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<risk-data>");
            builder.Append("<decision>it is decided</decision>");
            builder.Append("<decision-reasons>x,y,z</decision-reasons>");
            builder.Append("<device-data-captured>true</device-data-captured>");
            builder.Append("<fraud-service-provider>provider</fraud-service-provider>");
            builder.Append("<id>imaid</id>");
            builder.Append("<transaction-risk-score>05</transaction-risk-score>");
            builder.Append("</risk-data>");


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            RiskData data = new RiskData(new NodeWrapper(doc).GetNode("//risk-data"));
            Assert.IsNull(data.LiabilityShift);
        }
    }
}
