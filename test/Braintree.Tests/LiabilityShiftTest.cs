using NUnit.Framework;
using System.Text;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class LiabilityShiftTest
    {

        [Test]
        public void ConstructFromXMLResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<liability-shift>");
            builder.Append("<responsible-party>paypal</responsible-party>");
            builder.Append("<conditions>unauthorized,item_not_received,</conditions>");
            builder.Append("</liability-shift>");


            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            LiabilityShift liabilityShift = new LiabilityShift(new NodeWrapper(doc).GetNode("//liability-shift"));
            Assert.AreEqual("paypal", liabilityShift.ResponsibleParty);
            Assert.IsNotNull(liabilityShift.Conditions);
        }
    }
}