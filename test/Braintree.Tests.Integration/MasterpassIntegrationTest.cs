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
    public class MasterpassIntegrationTest
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

            string nonce = Braintree.Test.Nonce.MasterpassDiscover;
            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(paymentMethodResult.IsSuccess());

            Assert.IsInstanceOf(typeof(MasterpassCard), paymentMethodResult.Target);
            MasterpassCard masterpassCard = (MasterpassCard) paymentMethodResult.Target;

            Assert.IsNotNull(masterpassCard.BillingAddress);
            Assert.IsNotNull(masterpassCard.Bin);
            Assert.IsNotNull(masterpassCard.CardType);
            Assert.IsNotNull(masterpassCard.CardholderName);
            Assert.IsNotNull(masterpassCard.Commercial);
            Assert.IsNotNull(masterpassCard.CountryOfIssuance);
            Assert.IsNotNull(masterpassCard.CreatedAt);
            Assert.IsNotNull(masterpassCard.CustomerId);
            Assert.IsNotNull(masterpassCard.CustomerLocation);
            Assert.IsNotNull(masterpassCard.Debit);
            Assert.IsNotNull(masterpassCard.IsDefault);
            Assert.IsNotNull(masterpassCard.DurbinRegulated);
            Assert.IsNotNull(masterpassCard.ExpirationDate);
            Assert.IsNotNull(masterpassCard.ExpirationMonth);
            Assert.IsNotNull(masterpassCard.ExpirationYear);
            Assert.IsNotNull(masterpassCard.IsExpired);
            Assert.IsNotNull(masterpassCard.Healthcare);
            Assert.IsNotNull(masterpassCard.ImageUrl);
            Assert.IsNotNull(masterpassCard.IssuingBank);
            Assert.IsNotNull(masterpassCard.LastFour);
            Assert.IsNotNull(masterpassCard.MaskedNumber);
            Assert.IsNotNull(masterpassCard.Payroll);
            Assert.IsNotNull(masterpassCard.Prepaid);
            Assert.IsNotNull(masterpassCard.ProductId);
            Assert.IsNotNull(masterpassCard.Subscriptions);
            Assert.IsNotNull(masterpassCard.Token);
            Assert.IsNotNull(masterpassCard.UniqueNumberIdentifier);
            Assert.IsNotNull(masterpassCard.UpdatedAt);
        }

        [Test]
        public void SearchForTransaction()
        {
            string nonce = Braintree.Test.Nonce.MasterpassDiscover;
            TransactionRequest request = new TransactionRequest
            {
                Amount = 1000M,
                PaymentMethodNonce = nonce
            };
            var transactionResult = gateway.Transaction.Sale(request);
            Assert.IsTrue(transactionResult.IsSuccess());

            var searchRequest = new TransactionSearchRequest().
                Id.Is(transactionResult.Target.Id).
                PaymentInstrumentType.Is("masterpass_card");

            var searchResult = gateway.Transaction.Search(searchRequest);

            Assert.AreEqual(1, searchResult.MaximumCount);
            Assert.AreEqual(searchResult.FirstItem.Id, transactionResult.Target.Id);
        }

        [Test]
        public void CreateTransactionFromNonceAndVault()
        {
            string nonce = Braintree.Test.Nonce.MasterpassDiscover;
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

            MasterpassCardDetails masterpassCardDetails = (MasterpassCardDetails) transactionResult.Target.MasterpassCardDetails;

            Assert.IsNotNull(masterpassCardDetails.Bin);
            Assert.IsNotNull(masterpassCardDetails.CardType);
            Assert.IsNotNull(masterpassCardDetails.CardholderName);
            Assert.IsNotNull(masterpassCardDetails.Commercial);
            Assert.IsNotNull(masterpassCardDetails.CountryOfIssuance);
            Assert.IsNotNull(masterpassCardDetails.CustomerLocation);
            Assert.IsNotNull(masterpassCardDetails.Debit);
            Assert.IsNotNull(masterpassCardDetails.DurbinRegulated);
            Assert.IsNotNull(masterpassCardDetails.ExpirationDate);
            Assert.IsNotNull(masterpassCardDetails.ExpirationMonth);
            Assert.IsNotNull(masterpassCardDetails.ExpirationYear);
            Assert.IsNotNull(masterpassCardDetails.Healthcare);
            Assert.IsNotNull(masterpassCardDetails.ImageUrl);
            Assert.IsNotNull(masterpassCardDetails.IssuingBank);
            Assert.IsNotNull(masterpassCardDetails.LastFour);
            Assert.IsNotNull(masterpassCardDetails.MaskedNumber);
            Assert.IsNotNull(masterpassCardDetails.Payroll);
            Assert.IsNotNull(masterpassCardDetails.Prepaid);
            Assert.IsNotNull(masterpassCardDetails.ProductId);
            Assert.IsNotNull(masterpassCardDetails.Token);
        }
    }
}
