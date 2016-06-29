using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class CreditCardRequestTest
    {
        [Test]
        [Category("Unit")]
        public void ToXml_Includes_DeviceSessionId()
        {
            CreditCardRequest request = new CreditCardRequest();
            request.DeviceSessionId = "my_dsid";

            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
        }

        [Test]
        [Category("Unit")]
        public void ToXml_Includes_FraudMerchantId()
        {
            CreditCardRequest request = new CreditCardRequest();
            request.FraudMerchantId = "my_fmid";

            Assert.IsTrue(request.ToXml().Contains("my_fmid"));
        }

        [Test]
        [Category("Unit")]
        public void ToXml_Includes_DeviceData()
        {
            CreditCardRequest request = new CreditCardRequest();
            request.DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"my_fmid\"}";

            Assert.IsTrue(request.ToXml().Contains("device-data"));
            Assert.IsTrue(request.ToXml().Contains("device_session_id"));
            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
            Assert.IsTrue(request.ToXml().Contains("fraud_merchant_id"));
            Assert.IsTrue(request.ToXml().Contains("my_fmid"));
        }

        [Test]
        [Category("Unit")]
        public void ToXml_Includes_PaymentMethodNonce()
        {
            CreditCardRequest request = new CreditCardRequest();
            request.PaymentMethodNonce = "my-payment-method-nonce";

            Assert.IsTrue(request.ToXml().Contains("my-payment-method-nonce"));
        }

        [Test]
        [Category("Unit")]
        public void ToXml_Includes_Token()
        {
            CreditCardRequest request = new CreditCardRequest();
            request.Token = "my-token";

            Assert.IsTrue(request.ToXml().Contains("my-token"));
        }
    }
}
