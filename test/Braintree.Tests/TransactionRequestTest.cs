using NUnit.Framework;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionRequestTest
    {
        [Test]
        [System.Obsolete]
        public void ToXml_Includes_DeviceSessionId()
        {
            TransactionRequest request = new TransactionRequest();
            request.DeviceSessionId = "my_dsid";

            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
        }

        [Test]
        [System.Obsolete]
        public void ToXml_Includes_FraudMerchantId()
        {
            TransactionRequest request = new TransactionRequest();
            request.FraudMerchantId = "my_fmid";

            Assert.IsTrue(request.ToXml().Contains("my_fmid"));
        }

        [Test]
        public void ToXml_Includes_DeviceData()
        {
            TransactionRequest request = new TransactionRequest();
            request.DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"my_fmid\"}";

            Assert.IsTrue(request.ToXml().Contains("device-data"));
            Assert.IsTrue(request.ToXml().Contains("device_session_id"));
            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
            Assert.IsTrue(request.ToXml().Contains("fraud_merchant_id"));
            Assert.IsTrue(request.ToXml().Contains("my_fmid"));
        }

        [Test]
        public void ToXml_IncludesPaymentMethodNonce()
        {
            TransactionRequest request = new TransactionRequest();
            request.PaymentMethodNonce = "1232131232";

            Assert.IsTrue(request.ToXml().Contains("1232131232"));
        }

        [Test]
        public void ToXml_Includes_Level3SummaryData()
        {
            TransactionRequest request = new TransactionRequest();
            request.ShippingAmount = 1.00M;
            request.DiscountAmount = 2.00M;
            request.ShipsFromPostalCode = "12345";

            string xml = request.ToXml();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.AreEqual("1.00", doc.GetElementsByTagName("shipping-amount")[0].InnerXml);
            Assert.AreEqual("2.00", doc.GetElementsByTagName("discount-amount")[0].InnerXml);
            Assert.AreEqual("12345", doc.GetElementsByTagName("ships-from-postal-code")[0].InnerXml);
        }

        [Test]
        public void ToXml_IncludesInstallmentsCount()
        {
            TransactionRequest request = new TransactionRequest();
            request.InstallmentRequest = new InstallmentRequest();
            request.InstallmentRequest.Count = "4";

            string xml = request.ToXml();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.AreEqual("4", doc.SelectSingleNode("//installments/count").InnerText);
        }

        [Test]

        public void ToXml_IncludesProcessDebitAsCredit()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                MerchantAccountId = "pinless_debit",
                Options = new TransactionOptionsRequest
                {
                    CreditCard = new TransactionOptionsCreditCardRequest{
                        ProcessDebitAsCredit = true
                    }
                }
            };

            string xml=request.ToXml();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.AreEqual("true",doc.GetElementsByTagName("process-debit-as-credit")[0].InnerXml);

        }

        [Test]
        public void ToXml_IncludesFinalCapture()
        {
            TransactionRequest request = new TransactionRequest();
            request.FinalCapture = true;

            string xml = request.ToXml();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.AreEqual("true",doc.GetElementsByTagName("final-capture")[0].InnerXml);
        }

        [Test]
        public void ToXml_IncludesUsBankAccountOptions()
        {
            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                Options = new TransactionOptionsRequest
                {
                    UsBankAccount = new TransactionOptionsUsBankAccountRequest
                    {
                        AchType = "standard"
                    }
                }
            };

            string xml = request.ToXml();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.AreEqual("standard", doc.SelectSingleNode("//us-bank-account/ach-type").InnerText);
        }

        public void ToXml_Includes_ShippingTaxAmount()
        {
            TransactionRequest request = new TransactionRequest();
            request.ShippingAmount = 1.00M;
            request.DiscountAmount = 2.00M;
            request.ShippingTaxAmount = 3.00M;
            request.ShipsFromPostalCode = "12345";
            
            string xml = request.ToXml();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.AreEqual("1.00", doc.GetElementsByTagName("shipping-amount")[0].InnerXml);
            Assert.AreEqual("2.00", doc.GetElementsByTagName("discount-amount")[0].InnerXml);
            Assert.AreEqual("12345", doc.GetElementsByTagName("ships-from-postal-code")[0].InnerXml);
            Assert.AreEqual("3.00", doc.GetElementsByTagName("shipping-tax-amount")[0].InnerXml);
        }

        [Test]
        public void ToXml_IncludesAcceptPartialAuthorization()
        {
            TransactionRequest request = new TransactionRequest();
                
            request.Amount = SandboxValues.TransactionAmount.PARTIALLY_AUTHORIZED;
            request.AcceptPartialAuthorization = true;

            string xml=request.ToXml();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            Assert.AreEqual("true",doc.GetElementsByTagName("accept-partial-authorization")[0].InnerXml);
        }
    }
}
