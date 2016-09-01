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
        [Category("Unit")]
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

            CreditCardVerification verification = new CreditCardVerification(new NodeWrapper(doc).GetNode("//verification"), gateway);
            Assert.AreEqual(null, verification.AvsErrorResponseCode);
            Assert.AreEqual("I", verification.AvsPostalCodeResponseCode);
            Assert.AreEqual(VerificationStatus.PROCESSOR_DECLINED, verification.Status);
            Assert.AreEqual("2000", verification.ProcessorResponseCode);
            Assert.AreEqual("I", verification.AvsStreetAddressResponseCode);
            Assert.AreEqual("Do Not Honor", verification.ProcessorResponseText);
            Assert.AreEqual("M", verification.CvvResponseCode);
        }

        [Test]
        [Category("Unit")]
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

            CreditCardVerification verification = new CreditCardVerification(new NodeWrapper(doc).GetNode("//verification"), gateway);
            Assert.AreEqual(null, verification.AvsErrorResponseCode);
            Assert.AreEqual(null, verification.AvsPostalCodeResponseCode);
            Assert.AreEqual(null, verification.Status);
            Assert.AreEqual(null, verification.ProcessorResponseCode);
            Assert.AreEqual(null, verification.AvsStreetAddressResponseCode);
            Assert.AreEqual(null, verification.ProcessorResponseText);
            Assert.AreEqual(null, verification.CvvResponseCode);
        }

        [Test]
        [Category("Integration")]
        public void Create_ReturnsSuccessfulResponse()
        {
            var request = new CreditCardVerificationRequest
            {
                CreditCard = new CreditCardVerificationCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                    BillingAddress = new CreditCardAddressRequest
                    {
                        CountryName = "Greece",
                        CountryCodeAlpha2 = "GR",
                        CountryCodeAlpha3 = "GRC",
                        CountryCodeNumeric = "300"
                    }
                },
                Options = new CreditCardVerificationOptionsRequest
                {
                    MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID,
                    Amount = "5.00"
                }
            };

            Result<CreditCardVerification> result = gateway.CreditCardVerification.Create(request);
            Assert.IsTrue(result.IsSuccess());
            CreditCardVerification verification = result.Target;
            Assert.AreEqual(verification.MerchantAccountId,
                            MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID);
        }

        [Test]
        [Category("Integration")]
        public void Create_HandlesInvalidResponse()
        {
            var request = new CreditCardVerificationRequest
            {
                CreditCard = new CreditCardVerificationCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                    BillingAddress = new CreditCardAddressRequest
                    {
                        CountryName = "Greece",
                        CountryCodeAlpha2 = "GR",
                        CountryCodeAlpha3 = "GRC",
                        CountryCodeNumeric = "300"
                    }
                },
                Options = new CreditCardVerificationOptionsRequest
                {
                    MerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID,
                    Amount = "-5.00"
                }
            };

            Result<CreditCardVerification> result = gateway.CreditCardVerification.Create(request);
            Assert.IsFalse(result.IsSuccess());
            CreditCardVerification verification = result.Target;
            Assert.AreEqual(ValidationErrorCode.VERIFICATION_OPTIONS_AMOUNT_CANNOT_BE_NEGATIVE,
                            result.Errors.ForObject("Verification").ForObject("Options").OnField("Amount")[0].Code);
        }

        [Test]
        [Category("Integration")]
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
                Ids.IncludedIn(verification1.Id, verification2.Id).
                Status.IncludedIn(verification1.Status);

            ResourceCollection<CreditCardVerification> collection = gateway.CreditCardVerification.Search(searchRequest);

            Assert.AreEqual(2, collection.MaximumCount);
        }

        [Test]
        [Category("Integration")]
        public void CardTypeIndicators()
        {
            string name = Guid.NewGuid().ToString("n");
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
            Assert.AreEqual(verification.CreditCard.CountryOfIssuance, Braintree.CreditCard.CountryOfIssuanceUnknown);
            Assert.AreEqual(verification.CreditCard.IssuingBank, Braintree.CreditCard.IssuingBankUnknown);
            Assert.AreEqual(verification.CreditCard.ProductId, Braintree.CreditCard.ProductIdUnknown);
        }

        [Test]
        [Category("Integration")]
        public void Search_OnTextFields()
        {
            var createRequest = new CustomerRequest
            {
                Email = "mike.a@example.com",
                CreditCard = new CreditCardRequest
                {
                    Number = "4111111111111111",
                    ExpirationDate = "05/12",
                    BillingAddress = new CreditCardAddressRequest
                    {
                        PostalCode = "44444"
                    },
                    Options = new CreditCardOptionsRequest
                    {
                        VerifyCard = true
                    }
                }
            };

            Result<Customer> result = gateway.Customer.Create(createRequest);
            string token = result.Target.CreditCards[0].Token;
            string postalCode = result.Target.CreditCards[0].BillingAddress.PostalCode;
            string customerId = result.Target.Id;
            string customerEmail = result.Target.Email;

            CreditCardVerificationSearchRequest searchRequest = new CreditCardVerificationSearchRequest().
                PaymentMethodToken.Is(token).
                BillingAddressDetailsPostalCode.Is(postalCode).
                CustomerId.Is(customerId).
                CustomerEmail.Is(customerEmail);

            ResourceCollection<CreditCardVerification> collection = gateway.CreditCardVerification.Search(searchRequest);
            CreditCardVerification verification = collection.FirstItem;

            Assert.AreEqual(1, collection.MaximumCount);
            Assert.AreEqual(token, verification.CreditCard.Token);
            Assert.AreEqual(postalCode, verification.BillingAddress.PostalCode);
        }
    }
}
