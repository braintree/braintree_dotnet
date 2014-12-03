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
        private BraintreeService service;

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

            service = new BraintreeService(gateway.Configuration);
        }

        [Test]
        public void ConstructFromResponse()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<api-error-response>");
            builder.Append("  <verification>");
            builder.Append("    <avs-error-response-code nil=\"true\"></avs-error-response-code>");
            builder.Append("    <avs-postal-code-response-code>I</avs-postal-code-response-code>");
            builder.Append("    <status>processor_declined</status>");
            builder.Append("    <processor-response-code>2000</processor-response-code>");
            builder.Append("    <avs-street-address-response-code>I</avs-street-address-response-code>");
            builder.Append("    <processor-response-text>Do Not Honor</processor-response-text>");
            builder.Append("    <cvv-response-code>M</cvv-response-code>");
            builder.Append("  </verification>");
            builder.Append("  <errors>");
            builder.Append("    <errors type=\"array\"/>");
            builder.Append("  </errors>");
            builder.Append("</api-error-response>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            CreditCardVerification verification = new CreditCardVerification(new NodeWrapper(doc).GetNode("//verification"), service);
            Assert.AreEqual(null, verification.AvsErrorResponseCode);
            Assert.AreEqual("I", verification.AvsPostalCodeResponseCode);
            Assert.AreEqual(VerificationStatus.PROCESSOR_DECLINED, verification.Status);
            Assert.AreEqual("2000", verification.ProcessorResponseCode);
            Assert.AreEqual("I", verification.AvsStreetAddressResponseCode);
            Assert.AreEqual("Do Not Honor", verification.ProcessorResponseText);
            Assert.AreEqual("M", verification.CvvResponseCode);
        }

        [Test]
        public void ConstructFromResponseWithNoVerification()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            builder.Append("<api-error-response>");
            builder.Append("  <errors>");
            builder.Append("    <errors type=\"array\"/>");
            builder.Append("  </errors>");
            builder.Append("</api-error-response>");

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(builder.ToString());

            CreditCardVerification verification = new CreditCardVerification(new NodeWrapper(doc).GetNode("//verification"), service);
            Assert.AreEqual(null, verification.AvsErrorResponseCode);
            Assert.AreEqual(null, verification.AvsPostalCodeResponseCode);
            Assert.AreEqual(null, verification.Status);
            Assert.AreEqual(null, verification.ProcessorResponseCode);
            Assert.AreEqual(null, verification.AvsStreetAddressResponseCode);
            Assert.AreEqual(null, verification.ProcessorResponseText);
            Assert.AreEqual(null, verification.CvvResponseCode);
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

        [Test]
        public void CardTypeIndicators()
        {
            String name = Guid.NewGuid().ToString("n");
            var createRequest = new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    CardholderName = name,
                    Number = CreditCardNumbers.CardTypeIndicators.Unknown,
                    ExpirationDate = "05/12",
                    Options = new CreditCardOptionsRequest
                    {
                      VerifyCard = true
                    }
                }
            };

            gateway.Customer.Create(createRequest);

            CreditCardVerificationSearchRequest searchRequest = new CreditCardVerificationSearchRequest().
                CreditCardCardholderName.Is(name);

            ResourceCollection<CreditCardVerification> collection = gateway.CreditCardVerification.Search(searchRequest);

            CreditCardVerification verification = collection.FirstItem;

            Assert.AreEqual(verification.CreditCard.Prepaid, Braintree.CreditCardPrepaid.UNKNOWN);
            Assert.AreEqual(verification.CreditCard.Debit, Braintree.CreditCardDebit.UNKNOWN);
            Assert.AreEqual(verification.CreditCard.DurbinRegulated, Braintree.CreditCardDurbinRegulated.UNKNOWN);
            Assert.AreEqual(verification.CreditCard.Commercial, Braintree.CreditCardCommercial.UNKNOWN);
            Assert.AreEqual(verification.CreditCard.Healthcare, Braintree.CreditCardHealthcare.UNKNOWN);
            Assert.AreEqual(verification.CreditCard.Payroll, Braintree.CreditCardPayroll.UNKNOWN);

        }

        [Test]
        public void Search_OnTextFields()
        {
            var createRequest = new CustomerRequest
            {
                CreditCard = new CreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/12",
                    Options = new CreditCardOptionsRequest
                    {
                      VerifyCard = true
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);
            String token = result.Target.CreditCards[0].Token;

            CreditCardVerificationSearchRequest searchRequest = new CreditCardVerificationSearchRequest().
                PaymentMethodToken.Is(token);

            ResourceCollection<CreditCardVerification> collection = gateway.CreditCardVerification.Search(searchRequest);
            CreditCardVerification verification = collection.FirstItem;

            Assert.AreEqual(1, collection.MaximumCount);
            Assert.AreEqual(token, verification.CreditCard.Token);
        }
    }

}
