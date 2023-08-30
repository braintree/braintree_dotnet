using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class CustomerRequestTest
    {
        [Test]
        [System.Obsolete]
        public void ToXml_Includes_DeviceSessionId()
        {
            var request = new CustomerRequest()
            {
                CreditCard = new CreditCardRequest()
                {
                    DeviceSessionId = "my_dsid",
                    FraudMerchantId = "my_fmid"
                }
            };

            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
            Assert.IsTrue(request.ToXml().Contains("my_fmid"));
        }

        [Test]
        public void ToXml_Includes_CreditCard_DeviceData()
        {
            var request = new CustomerRequest()
            {
                CreditCard = new CreditCardRequest()
                {
                    DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"my_fmid\"}"
                }
            };

            Assert.IsTrue(request.ToXml().Contains("device_session_id"));
            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
            Assert.IsTrue(request.ToXml().Contains("fraud_merchant_id"));
            Assert.IsTrue(request.ToXml().Contains("my_fmid"));
        }

        [Test]
        public void ToXml_Includes_DeviceData()
        {
            var request = new CustomerRequest()
            {
                DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"my_fmid\"}"
            };

            Assert.IsTrue(request.ToXml().Contains("device_session_id"));
            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
            Assert.IsTrue(request.ToXml().Contains("fraud_merchant_id"));
            Assert.IsTrue(request.ToXml().Contains("my_fmid"));
        }

        [Test]
        public void ToXml_Includes_TaxIdentifiers()
        {
            var request = new CustomerRequest()
            {
                TaxIdentifiers = new TaxIdentifierRequest[]
                {
                    new TaxIdentifierRequest
                    {
                        CountryCode = "US",
                        Identifier = "123"
                    },
                    new TaxIdentifierRequest
                    {
                        CountryCode = "CL",
                        Identifier = "456"
                    }
                }
            };

            Assert.IsTrue(request.ToXml().Contains("US"));
            Assert.IsTrue(request.ToXml().Contains("123"));

            Assert.IsTrue(request.ToXml().Contains("CL"));
            Assert.IsTrue(request.ToXml().Contains("456"));
        }

        [Test]
        public void ToXml_Includes_ThreeDSecureAuthenticationId()
        {
            var request = new CustomerRequest
            {
                ThreeDSecureAuthenticationId = "some-authentication-id"
            };

            var xml = request.ToXml();

            Assert.IsTrue(xml.Contains("<three-d-secure-authentication-id>some-authentication-id</three-d-secure-authentication-id>"));
        }
    }
}
