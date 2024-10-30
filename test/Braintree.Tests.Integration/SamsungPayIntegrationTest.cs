using Braintree.Exceptions;
using Braintree.Test;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Params = System.Collections.Generic.Dictionary<string, object>;

//NEXT_MAJOR_VERSION remove these tests
// SamsungPayCard has been deprecated
namespace Braintree.Tests.Integration
{
    [TestFixture]
    [Ignore("Samsung Pay deprecated TODO - Remove in major release")]
    public class SamsungPayIntegrationTest
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
        public void CreateFromPaymentMethodNonce()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            string nonce = Braintree.Test.Nonce.SamsungPayDiscover;
            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce,
                CardholderName = "Jenny Block",
                BillingAddress = new PaymentMethodAddressRequest
                {
                    CountryName = "Chad",
                }
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(paymentMethodResult.IsSuccess());

            Assert.IsInstanceOf(typeof(SamsungPayCard), paymentMethodResult.Target);
            SamsungPayCard samsungPayCard = (SamsungPayCard) paymentMethodResult.Target;

            Assert.IsNotNull(samsungPayCard.BillingAddress);
            Assert.IsNotNull(samsungPayCard.Bin);
            Assert.IsNotNull(samsungPayCard.CardholderName);
            Assert.IsNotNull(samsungPayCard.CardType);
            Assert.IsNotNull(samsungPayCard.Commercial);
            Assert.IsNotNull(samsungPayCard.CountryOfIssuance);
            Assert.IsNotNull(samsungPayCard.CreatedAt);
            Assert.IsNotNull(samsungPayCard.CustomerId);
            Assert.IsNotNull(samsungPayCard.CustomerLocation);
            Assert.IsNotNull(samsungPayCard.Debit);
            Assert.IsNotNull(samsungPayCard.IsDefault);
            Assert.IsNotNull(samsungPayCard.DurbinRegulated);
            Assert.IsNotNull(samsungPayCard.ExpirationDate);
            Assert.IsNotNull(samsungPayCard.ExpirationMonth);
            Assert.IsNotNull(samsungPayCard.ExpirationYear);
            Assert.IsNotNull(samsungPayCard.IsExpired);
            Assert.IsNotNull(samsungPayCard.Healthcare);
            Assert.IsNotNull(samsungPayCard.ImageUrl);
            Assert.IsNotNull(samsungPayCard.IssuingBank);
            Assert.IsNotNull(samsungPayCard.LastFour);
            Assert.IsNotNull(samsungPayCard.SourceCardLastFour);
            Assert.IsNotNull(samsungPayCard.MaskedNumber);
            Assert.IsNotNull(samsungPayCard.Payroll);
            Assert.IsNotNull(samsungPayCard.Prepaid);
            Assert.IsNotNull(samsungPayCard.ProductId);
            Assert.IsNotNull(samsungPayCard.Subscriptions);
            Assert.IsNotNull(samsungPayCard.Token);
            Assert.IsNotNull(samsungPayCard.UniqueNumberIdentifier);
            Assert.IsNotNull(samsungPayCard.UpdatedAt);
        }

        [Test]
        public void CreateFromPaymentMethodNonceWithNameAndAddress()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            string nonce = Braintree.Test.Nonce.SamsungPayDiscover;
            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce,
                CardholderName = "Jenny Block",
                BillingAddress = new PaymentMethodAddressRequest
                {
                    FirstName = "Gronk",
                    CountryName = "Chad",
                    CountryCodeAlpha2 = "TD",
                    CountryCodeAlpha3 = "TCD",
                    CountryCodeNumeric = "148"
                }
            };

            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(paymentMethodResult.IsSuccess());

            SamsungPayCard samsungPayCard = (SamsungPayCard) paymentMethodResult.Target;
            Assert.AreEqual("Jenny Block", samsungPayCard.CardholderName);

            Address billingAddress = samsungPayCard.BillingAddress;
            Assert.AreEqual("Chad", billingAddress.CountryName);
            Assert.AreEqual("TD", billingAddress.CountryCodeAlpha2);
            Assert.AreEqual("TCD", billingAddress.CountryCodeAlpha3);
        }

        [Test]
        public void SearchForTransaction()
        {
            string nonce = Braintree.Test.Nonce.SamsungPayDiscover;
            TransactionRequest request = new TransactionRequest
            {
                Amount = 1000M,
                PaymentMethodNonce = nonce
            };
            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());

            var searchRequest = new TransactionSearchRequest().
                Id.Is(transactionResult.Target.Id).
                PaymentInstrumentType.Is("samsung_pay_card");

            var searchResult = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(1, searchResult.MaximumCount);
            Assert.AreEqual(searchResult.FirstItem.Id, transactionResult.Target.Id);
        }

        [Test]
        public void CreateTransactionFromNonceAndVault()
        {
            string nonce = Braintree.Test.Nonce.SamsungPayDiscover;
            TransactionRequest request = new TransactionRequest
            {
                Amount = 1000M,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    StoreInVault = true
                },
                CreditCard = new TransactionCreditCardRequest
                {
                   CardholderName = "Jenny Block"
                }
            };
            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());

            SamsungPayCardDetails samsungPayCardDetails = (SamsungPayCardDetails) transactionResult.Target.SamsungPayCardDetails;

            Assert.IsNotNull(samsungPayCardDetails.Bin);
            Assert.IsNotNull(samsungPayCardDetails.CardholderName);
            Assert.IsNotNull(samsungPayCardDetails.CardType);
            Assert.IsNotNull(samsungPayCardDetails.Commercial);
            Assert.IsNotNull(samsungPayCardDetails.CountryOfIssuance);
            Assert.IsNotNull(samsungPayCardDetails.CustomerLocation);
            Assert.IsNotNull(samsungPayCardDetails.Debit);
            Assert.IsNotNull(samsungPayCardDetails.DurbinRegulated);
            Assert.IsNotNull(samsungPayCardDetails.ExpirationDate);
            Assert.IsNotNull(samsungPayCardDetails.ExpirationMonth);
            Assert.IsNotNull(samsungPayCardDetails.ExpirationYear);
            Assert.IsNotNull(samsungPayCardDetails.Healthcare);
            Assert.IsNotNull(samsungPayCardDetails.ImageUrl);
            Assert.IsNotNull(samsungPayCardDetails.IssuingBank);
            Assert.IsNotNull(samsungPayCardDetails.LastFour);
            Assert.IsNotNull(samsungPayCardDetails.SourceCardLastFour);
            Assert.IsNotNull(samsungPayCardDetails.MaskedNumber);
            Assert.IsNotNull(samsungPayCardDetails.Payroll);
            Assert.IsNotNull(samsungPayCardDetails.Prepaid);
            Assert.IsNotNull(samsungPayCardDetails.ProductId);
            Assert.IsNotNull(samsungPayCardDetails.Token);
        }
    }
}
