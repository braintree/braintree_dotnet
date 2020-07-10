using Braintree.Exceptions;
using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class PaymentMethodNonceRequestTest
    {
        [Test]
        public void ToXml_RequiresMerchantAccountIDWhenRequestingAuthInsights()
        {
            PaymentMethodNonceRequest request = new PaymentMethodNonceRequest();
            Assert.DoesNotThrow(() => request.ToXml());

            request.AuthenticationInsight = true;
            Assert.Throws<PaymentMethodNonceRequestInvalidException>(() => request.ToXml());
        }

        [Test]
        public void ToXml_BackwardsCompatibility()
        {
            PaymentMethodNonceRequest request = new PaymentMethodNonceRequest();

            Assert.DoesNotThrow(() => {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(request.ToXml());
            });
            Assert.IsFalse(request.ToXml().Contains("<amount>"));
            Assert.IsFalse(request.ToXml().Contains("<merchant-account-id>"));
            Assert.IsFalse(request.ToXml().Contains("<authentication-insight>"));
        }

        [Test]
        public void ToXml_Includes_MerchantAccountID()
        {
            PaymentMethodNonceRequest request = new PaymentMethodNonceRequest();
            request.AuthenticationInsight = true;

            request.MerchantAccountId = "my_merchant_account_id";

            Assert.DoesNotThrow(() => {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(request.ToXml());
            });
            Assert.IsTrue(request.ToXml().Contains("<merchant-account-id>my_merchant_account_id</merchant-account-id>"));
        }

        [Test]
        public void ToXml_Includes_AuthenticationInsight()
        {
            PaymentMethodNonceRequest request = new PaymentMethodNonceRequest();
            request.AuthenticationInsight = true;
            request.MerchantAccountId = "my_merchant_account_id";

            Assert.DoesNotThrow(() => {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(request.ToXml());
            });
            Assert.IsTrue(request.ToXml().Contains("<authentication-insight>true</authentication-insight>"));
        }

        [Test]
        public void ToXml_DoesNotInclude_AuthenticationInsightOptionsIfUnspecified()
        {
            PaymentMethodNonceRequest request = new PaymentMethodNonceRequest();
            request.AuthenticationInsight = true;
            request.MerchantAccountId = "my_merchant_account_id";

            Assert.DoesNotThrow(() => {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(request.ToXml());
            });
            Assert.IsFalse(request.ToXml().Contains("<authentication-insight-options>"));
        }

        [Test]
        public void ToXml_Includes_AuthenticationInsightOptionsIfSpecified()
        {
            AuthenticationInsightOptionsRequest options = new AuthenticationInsightOptionsRequest();
            options.Amount = 1234.00M;

            PaymentMethodNonceRequest request = new PaymentMethodNonceRequest();
            request.AuthenticationInsight = true;
            request.MerchantAccountId = "my_merchant_account_id";
            request.AuthenticationInsightOptions = options;

            Assert.DoesNotThrow(() => {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(request.ToXml());
            });

            Assert.IsTrue(request.ToXml().Contains("<authentication-insight-options>"));
            Assert.IsTrue(request.ToXml().Contains("<amount>1234.00</amount>"));
        }

        [Test]
        public void AuthenticationInsightOptionsIfAmountNotSpecifiedDefaultsToZero()
        {
            AuthenticationInsightOptionsRequest options = new AuthenticationInsightOptionsRequest();

            PaymentMethodNonceRequest request = new PaymentMethodNonceRequest();
            request.AuthenticationInsight = true;
            request.MerchantAccountId = "my_merchant_account_id";
            request.AuthenticationInsightOptions = options;

            Assert.DoesNotThrow(() => {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(request.ToXml());
            });

            Assert.IsTrue(request.ToXml().Contains("<authentication-insight-options>"));
            Assert.IsTrue(request.ToXml().Contains("<amount>0.00</amount>"));
        }
    }
}
