using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionRequestTest
    {
        [Test]
        [Category("Unit")]
        public void ToXml_Includes_DeviceSessionId()
        {
            TransactionRequest request = new TransactionRequest();
            request.DeviceSessionId = "my_dsid";

            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
        }

        [Test]
        [Category("Unit")]
        public void ToXml_Includes_FraudMerchantId()
        {
            TransactionRequest request = new TransactionRequest();
            request.FraudMerchantId = "my_fmid";

            Assert.IsTrue(request.ToXml().Contains("my_fmid"));
        }

        [Test]
        [Category("Unit")]
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
        [Category("Unit")]
        public void ToXml_InludesPaymentMethodNonce()
        {
            TransactionRequest request = new TransactionRequest();
            request.PaymentMethodNonce = "1232131232";

            Assert.IsTrue(request.ToXml().Contains("1232131232"));
        }
    }
}
