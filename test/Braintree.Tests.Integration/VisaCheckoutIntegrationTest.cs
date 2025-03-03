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
        public void CreateFromPaymentMethodNonce()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            string nonce = Braintree.Test.Nonce.VisaCheckoutDiscover;
            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(paymentMethodResult.IsSuccess());

            Assert.IsInstanceOf(typeof(VisaCheckoutCard), paymentMethodResult.Target);
            VisaCheckoutCard visaCheckoutCard = (VisaCheckoutCard) paymentMethodResult.Target;

            Assert.AreEqual("abc123", visaCheckoutCard.CallId);
            Assert.IsNotNull(visaCheckoutCard.BillingAddress);
            Assert.IsNotNull(visaCheckoutCard.Bin);
            Assert.IsNotNull(visaCheckoutCard.CardType);
            Assert.IsNotNull(visaCheckoutCard.CardholderName);
            Assert.IsNotNull(visaCheckoutCard.Commercial);
            Assert.IsNotNull(visaCheckoutCard.CountryOfIssuance);
            Assert.IsNotNull(visaCheckoutCard.CreatedAt);
            Assert.IsNotNull(visaCheckoutCard.CustomerId);
            Assert.IsNotNull(visaCheckoutCard.CustomerLocation);
            Assert.IsNotNull(visaCheckoutCard.Debit);
            Assert.IsNotNull(visaCheckoutCard.DurbinRegulated);
            Assert.IsNotNull(visaCheckoutCard.ExpirationDate);
            Assert.IsNotNull(visaCheckoutCard.ExpirationMonth);
            Assert.IsNotNull(visaCheckoutCard.ExpirationYear);
            Assert.IsNotNull(visaCheckoutCard.Healthcare);
            Assert.IsNotNull(visaCheckoutCard.ImageUrl);
            Assert.IsNotNull(visaCheckoutCard.IsDefault);
            Assert.IsNotNull(visaCheckoutCard.IsExpired);
            Assert.IsNotNull(visaCheckoutCard.IssuingBank);
            Assert.IsNotNull(visaCheckoutCard.LastFour);
            Assert.IsNotNull(visaCheckoutCard.MaskedNumber);
            Assert.IsNotNull(visaCheckoutCard.Payroll);
            Assert.IsNotNull(visaCheckoutCard.Prepaid);
            Assert.IsNotNull(visaCheckoutCard.PrepaidReloadable);
            Assert.IsNotNull(visaCheckoutCard.ProductId);
            Assert.IsNotNull(visaCheckoutCard.Subscriptions);
            Assert.IsNotNull(visaCheckoutCard.Token);
            Assert.IsNotNull(visaCheckoutCard.UniqueNumberIdentifier);
            Assert.IsNotNull(visaCheckoutCard.UpdatedAt);
        }

        [Test]
        public void CreateWithVerification()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            string nonce = Braintree.Test.Nonce.VisaCheckoutDiscover;
            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce,
                Options = new PaymentMethodOptionsRequest()
                {
                    VerifyCard = true
                }
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(paymentMethodResult.IsSuccess());

            VisaCheckoutCard visaCheckoutCard = (VisaCheckoutCard) paymentMethodResult.Target;

            Assert.AreEqual(VerificationStatus.VERIFIED, visaCheckoutCard.Verification.Status);
        }

        [Test]
        public void SearchForTransaction()
        {
            string nonce = Braintree.Test.Nonce.VisaCheckoutDiscover;
            TransactionRequest request = new TransactionRequest
            {
                Amount = 1000M,
                PaymentMethodNonce = nonce
            };
            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());

            var searchRequest = new TransactionSearchRequest().
                Id.Is(transactionResult.Target.Id).
                PaymentInstrumentType.Is("visa_checkout_card");

            var searchResult = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(1, searchResult.MaximumCount);
            Assert.AreEqual(searchResult.FirstItem.Id, transactionResult.Target.Id);
        }

        [Test]
        public void CreateTransactionFromNonceAndVault()
        {
            string nonce = Braintree.Test.Nonce.VisaCheckoutDiscover;
            TransactionRequest request = new TransactionRequest
            {
                Amount = 1000M,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    StoreInVault = true
                }
            };
            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());

            VisaCheckoutCardDetails visaCheckoutCardDetails = (VisaCheckoutCardDetails) transactionResult.Target.VisaCheckoutCardDetails;

            Assert.AreEqual("abc123", visaCheckoutCardDetails.CallId);
            Assert.IsNotNull(visaCheckoutCardDetails.Bin);
            Assert.IsNotNull(visaCheckoutCardDetails.CardType);
            Assert.IsNotNull(visaCheckoutCardDetails.CardholderName);
            Assert.IsNotNull(visaCheckoutCardDetails.Commercial);
            Assert.IsNotNull(visaCheckoutCardDetails.CountryOfIssuance);
            Assert.IsNotNull(visaCheckoutCardDetails.CustomerLocation);
            Assert.IsNotNull(visaCheckoutCardDetails.Debit);
            Assert.IsNotNull(visaCheckoutCardDetails.DurbinRegulated);
            Assert.IsNotNull(visaCheckoutCardDetails.ExpirationDate);
            Assert.IsNotNull(visaCheckoutCardDetails.ExpirationMonth);
            Assert.IsNotNull(visaCheckoutCardDetails.ExpirationYear);
            Assert.IsNotNull(visaCheckoutCardDetails.Healthcare);
            Assert.IsNotNull(visaCheckoutCardDetails.ImageUrl);
            Assert.IsNotNull(visaCheckoutCardDetails.IssuingBank);
            Assert.IsNotNull(visaCheckoutCardDetails.LastFour);
            Assert.IsNotNull(visaCheckoutCardDetails.MaskedNumber);
            Assert.IsNotNull(visaCheckoutCardDetails.Payroll);
            Assert.IsNotNull(visaCheckoutCardDetails.Prepaid);
            Assert.IsNotNull(visaCheckoutCardDetails.PrepaidReloadable);
            Assert.IsNotNull(visaCheckoutCardDetails.ProductId);
            Assert.IsNotNull(visaCheckoutCardDetails.Token);
        }
    }
}
