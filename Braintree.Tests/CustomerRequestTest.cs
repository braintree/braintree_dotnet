using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree.Exceptions;

namespace Braintree.Tests
{
    [TestFixture]
    public class CustomerRequestTest
    {
        [Test]
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
        public void ToXml_Includes_DeviceData()
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
    }
}
