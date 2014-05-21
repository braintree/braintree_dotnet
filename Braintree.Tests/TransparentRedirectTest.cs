using System;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransparentRedirectTest
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
        public void Url_ReturnsCorrectUrl()
        {
            var host = System.Environment.GetEnvironmentVariable("GATEWAY_HOST") ?? "localhost";
            var port = System.Environment.GetEnvironmentVariable("GATEWAY_PORT") ?? "3000";

            var url = "http://" + host + ":" + port + "/merchants/integration_merchant_id/transparent_redirect_requests";
            Assert.AreEqual(url, gateway.TransparentRedirect.Url);
        }

        [Test]
        public void BuildTrData_BuildsAQueryStringWithApiVersion()
        {
            String tr_data = gateway.TransparentRedirect.BuildTrData(new TransactionRequest(), "example.com");
            TestHelper.AssertIncludes("api_version=4", tr_data);
        }

        [Test]
        public void CreateTransactionFromTransparentRedirect()
        {
            TransactionRequest trParams = new TransactionRequest
            {
                Type = TransactionType.SALE
            };

            TransactionRequest request = new TransactionRequest
            {
                Amount = SandboxValues.TransactionAmount.AUTHORIZE,
                CreditCard = new TransactionCreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url, service);
            Result<Transaction> result = gateway.TransparentRedirect.ConfirmTransaction(queryString);
            Assert.IsTrue(result.IsSuccess());
            Transaction transaction = result.Target;

            Assert.AreEqual(1000.00, transaction.Amount);
            Assert.AreEqual(TransactionType.SALE, transaction.Type);
            Assert.AreEqual(TransactionStatus.AUTHORIZED, transaction.Status);
            Assert.AreEqual(DateTime.Now.Year, transaction.CreatedAt.Value.Year);
            Assert.AreEqual(DateTime.Now.Year, transaction.UpdatedAt.Value.Year);

            CreditCard creditCard = transaction.CreditCard;
            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("05", creditCard.ExpirationMonth);
            Assert.AreEqual("2009", creditCard.ExpirationYear);
            Assert.AreEqual("05/2009", creditCard.ExpirationDate);

        }

        [Test]
        public void CreateCustomerFromTransparentRedirect()
        {
            CustomerRequest trParams = new CustomerRequest
            {
                FirstName = "John"
            };

            CustomerRequest request = new CustomerRequest
            {
                LastName = "Doe"
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url, service);
            Result<Customer> result = gateway.TransparentRedirect.ConfirmCustomer(queryString);
            Assert.IsTrue(result.IsSuccess());
            Customer customer = result.Target;

            Assert.AreEqual("John", customer.FirstName);
            Assert.AreEqual("Doe", customer.LastName);
        }

        [Test]
        public void UpdateCustomerFromTransparentRedirect()
        {
            var createRequest = new CustomerRequest
            {
                FirstName = "Miranda",
                LastName = "Higgenbottom"
            };

            Customer createdCustomer = gateway.Customer.Create(createRequest).Target;

            CustomerRequest trParams = new CustomerRequest
            {
                CustomerId = createdCustomer.Id,
                FirstName = "Penelope"
            };

            CustomerRequest request = new CustomerRequest
            {
                LastName = "Lambert"
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url, service);
            Result<Customer> result = gateway.TransparentRedirect.ConfirmCustomer(queryString);
            Assert.IsTrue(result.IsSuccess());
            Customer updatedCustomer = gateway.Customer.Find(createdCustomer.Id);

            Assert.AreEqual("Penelope", updatedCustomer.FirstName);
            Assert.AreEqual("Lambert", updatedCustomer.LastName);
        }

        [Test]
        public void CreateCreditCardFromTransparentRedirect()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;
            CreditCardRequest trParams = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "4111111111111111",
                ExpirationDate = "10/10"
            };

            CreditCardRequest request = new CreditCardRequest
            {
                CardholderName = "John Doe"
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url, service);
            Result<CreditCard> result = gateway.TransparentRedirect.ConfirmCreditCard(queryString);
            Assert.IsTrue(result.IsSuccess());
            CreditCard creditCard = result.Target;

            Assert.AreEqual("John Doe", creditCard.CardholderName);
            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("10/2010", creditCard.ExpirationDate);
        }


        [Test]
        public void UpdateCreditCardFromTransparentRedirect()
        {
            Customer customer = gateway.Customer.Create(new CustomerRequest()).Target;

            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12",
                CardholderName = "Beverly D'angelo"
            };

            CreditCard createdCreditCard = gateway.CreditCard.Create(creditCardRequest).Target;


            CreditCardRequest trParams = new CreditCardRequest
            {
                PaymentMethodToken = createdCreditCard.Token,
                Number = "4111111111111111",
                ExpirationDate = "10/10"
            };

            CreditCardRequest request = new CreditCardRequest
            {
                CardholderName = "Sampsonite"
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url, service);
            Result<CreditCard> result = gateway.TransparentRedirect.ConfirmCreditCard(queryString);
            Assert.IsTrue(result.IsSuccess());
            CreditCard creditCard = gateway.CreditCard.Find(createdCreditCard.Token);

            Assert.AreEqual("Sampsonite", creditCard.CardholderName);
            Assert.AreEqual("411111", creditCard.Bin);
            Assert.AreEqual("1111", creditCard.LastFour);
            Assert.AreEqual("10/2010", creditCard.ExpirationDate);
        }
    }
}
