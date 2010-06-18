using System;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class TransparentRedirectTest
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
        public void Url_ReturnsCorrectUrl()
        {
            Assert.AreEqual("http://localhost:3000/merchants/integration_merchant_id/transparent_redirect_requests", gateway.TransparentRedirect.Url);
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
                CreditCard = new CreditCardRequest
                {
                    Number = SandboxValues.CreditCardNumber.VISA,
                    ExpirationDate = "05/2009",
                }
            };

            String queryString = TestHelper.QueryStringForTR(trParams, request, gateway.TransparentRedirect.Url);
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

    }
}
