using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Braintree;
using System.Xml;

namespace Braintree.Tests
{
    [TestFixture]
    public class CreditCardVerificationTest
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
        public void Search_OnMultipleValueFields()
        {
            var createRequest = new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    Number = CreditCardNumbers.FailsSandboxVerification.Visa,
                    ExpirationDate = "05/12",
                    Options = new CreditCardOptionsRequest
                    {
                      VerifyCard = true
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);
            CreditCardVerification verification1 = gateway.CreditCardVerification.Find(result.CreditCardVerification.Id);

            createRequest = new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    Number = CreditCardNumbers.FailsSandboxVerification.MasterCard,
                    ExpirationDate = "05/12",
                    Options = new CreditCardOptionsRequest
                    {
                      VerifyCard = true
                    }
                }
            };

            result = gateway.Customer.Create(createRequest);
            CreditCardVerification verification2 = gateway.CreditCardVerification.Find(result.CreditCardVerification.Id);

            CreditCardVerificationSearchRequest searchRequest = new CreditCardVerificationSearchRequest().
                CreditCardCardType.IncludedIn(CreditCardCardType.VISA, CreditCardCardType.MASTER_CARD).
                Ids.IncludedIn(verification1.Id, verification2.Id);

            ResourceCollection<CreditCardVerification> collection = gateway.CreditCardVerification.Search(searchRequest);

            Assert.AreEqual(2, collection.MaximumCount);
        }
    }

}
