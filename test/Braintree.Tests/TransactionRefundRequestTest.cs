using System;
using NUnit.Framework;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransactionRefundRequestTest
    {
        [Test]
        public void ToXml_IncludesAmount()
        {
            TransactionRefundRequest request = new TransactionRefundRequest();
            request.Amount = 10.00M;

            string xml = request.ToXml();
            Assert.IsTrue(xml.Contains("<amount>10.00</amount>"));
        }

        [Test]
        public void ToXml_IncludesOrderId()
        {
            TransactionRefundRequest request = new TransactionRefundRequest();
            request.OrderId = "order-123";

            string xml = request.ToXml();
            Assert.IsTrue(xml.Contains("<order-id>order-123</order-id>"));
        }

        [Test]
        public void ToXml_IncludesMerchantAccountId()
        {
            TransactionRefundRequest request = new TransactionRefundRequest();
            request.MerchantAccountId = "merchant-456";

            string xml = request.ToXml();
            Assert.IsTrue(xml.Contains("<merchant-account-id>merchant-456</merchant-account-id>"));
        }

        [Test]
        public void ToXml_IncludesApiRequestKey()
        {
            TransactionRefundRequest request = new TransactionRefundRequest();
            request.ApiRequestKey = "test-api-key-123";

            string xml = request.ToXml();
            Assert.IsTrue(xml.Contains("<api-request-key>test-api-key-123</api-request-key>"));
        }

        [Test]
        public void ToXml_ExcludesApiRequestKeyWhenNull()
        {
            TransactionRefundRequest request = new TransactionRefundRequest();
            request.ApiRequestKey = null;

            string xml = request.ToXml();
            Assert.IsFalse(xml.Contains("api-request-key"));
        }

        [Test]
        public void ToXml_IncludesAllFields()
        {
            TransactionRefundRequest request = new TransactionRefundRequest();
            request.Amount = 25.50M;
            request.ApiRequestKey = "refund-key-789";
            request.OrderId = "order-abc";
            request.MerchantAccountId = "merchant-xyz";

            string xml = request.ToXml();
            Assert.IsTrue(xml.Contains("<amount>25.50</amount>"));
            Assert.IsTrue(xml.Contains("<api-request-key>refund-key-789</api-request-key>"));
            Assert.IsTrue(xml.Contains("<order-id>order-abc</order-id>"));
            Assert.IsTrue(xml.Contains("<merchant-account-id>merchant-xyz</merchant-account-id>"));
        }
    }
}
