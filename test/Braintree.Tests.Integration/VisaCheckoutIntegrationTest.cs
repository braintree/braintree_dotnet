using Braintree.Exceptions;
using Braintree.Test;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Params = System.Collections.Generic.Dictionary<string, object>;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class VisaCheckoutIntegrationTest
    {
        private BraintreeGateway gateway;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
        }

        [Test]
        public void SearchByPaymentInstrumentType()
        {
            var searchRequest = new TransactionSearchRequest().
                PaymentInstrumentType.Is("visa_checkout_card");

            var searchResult = gateway.Transaction.Search(searchRequest);

            Assert.IsNotNull(searchResult);
            Assert.GreaterOrEqual(searchResult.MaximumCount, 1);
        }
    }
}
