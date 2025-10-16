using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using NUnit.Framework;
using Braintree.TestUtil;
using Newtonsoft.Json;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class BankAccountInstantVerificationIntegrationTest
    {
        private BraintreeGateway gateway;
        private BraintreeGateway usBankGateway;

        [SetUp]
        public void SetUp()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration2_merchant_id",
                PublicKey = "integration2_public_key",
                PrivateKey = "integration2_private_key"
            };

            usBankGateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
        }


        private string GenerateUsBankAccountNonceViaOpenBanking()
        {
            var config = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );

            // Use the new Open Banking REST API endpoint to tokenize without ACH mandate
            var requestBody = new
            {
                account_details = new
                {
                    account_number = "567891234",
                    account_type = "CHECKING",
                    classification = "PERSONAL",
                    tokenized_account = true,
                    last_4 = "1234"
                },
                institution_details = new
                {
                    bank_id = new
                    {
                        bank_code = "021000021",
                        country_code = "US"
                    }
                },
                account_holders = new[]
                {
                    new
                    {
                        ownership = "PRIMARY",
                        full_name = new
                        {
                            name = "Dan Schulman"
                        },
                        name = new
                        {
                            given_name = "Dan",
                            surname = "Schulman",
                            full_name = "Dan Schulman"
                        }
                    }
                }
            };

            var graphQLBaseUrl = config.GetGraphQLUrl();
            var atmosphereBaseUrl = graphQLBaseUrl.Replace("/graphql", "");
            var url = atmosphereBaseUrl + "/v1/open-finance/tokenize-bank-account-details";
            var jsonBody = JsonConvert.SerializeObject(requestBody);

#if netcore
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Braintree-Version", "2019-01-01");
                request.Headers.Add("User-Agent", "Braintree .NET Library " + typeof(BraintreeGateway).Assembly.GetName().Version.ToString());
                request.Headers.Add("X-ApiVersion", "2019-01-01");
                
                var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{config.PublicKey}:{config.PrivateKey}"));
                request.Headers.Add("Authorization", "Basic " + authString);

                var response = client.SendAsync(request).GetAwaiter().GetResult();
                var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"HTTP error {response.StatusCode}: {responseContent}");
                }

                dynamic responseData = JsonConvert.DeserializeObject(responseContent);
                if (responseData.tenant_token == null)
                {
                    throw new Exception($"Open Banking tokenization failed: {responseContent}");
                }

                return (string)responseData.tenant_token;
            }
#else
            throw new NotImplementedException("Open Banking tokenization not implemented for .NET Framework");
#endif
        }

        [Test]
        public void CreateJwtWithValidRequest()
        {
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "15Ladders",
                ReturnUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel"
            };

            Result<BankAccountInstantVerificationJwt> result = gateway.BankAccountInstantVerification.CreateJwt(request);

            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target.Jwt);
        }

        [Test]
        public void CreateJwtFailsWithInvalidBusinessName()
        {
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "",
                ReturnUrl = "https://example.com/return",
                CancelUrl = "https://example.com/cancel"
            };

            Result<BankAccountInstantVerificationJwt> result = gateway.BankAccountInstantVerification.CreateJwt(request);

            Assert.IsFalse(result.IsSuccess());
            Assert.IsNotNull(result.Errors);
        }

        [Test]
        public void CreateJwtFailsWithInvalidUrls()
        {
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "15Ladders",
                ReturnUrl = "not-a-valid-url",
                CancelUrl = "also-not-valid"
            };

            Result<BankAccountInstantVerificationJwt> result = usBankGateway.BankAccountInstantVerification.CreateJwt(request);

            Assert.IsFalse(result.IsSuccess());
            Assert.IsNotNull(result.Errors);
        }

        [Test]
        public void TokenizesBankAccountViaOpenFinanceApiVaultsWithAndCharges()
        {
            string nonce = GenerateUsBankAccountNonceViaOpenBanking();

            Result<Customer> customerResult = usBankGateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customerResult.IsSuccess());
            Customer customer = customerResult.Target;

            DateTime mandateAcceptedAt = DateTime.UtcNow.AddMinutes(-5);

            var paymentMethodRequest = new PaymentMethodRequest
            {
                CustomerId = customer.Id,
                PaymentMethodNonce = nonce,
                Options = new PaymentMethodOptionsRequest
                {
                    VerificationMerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                    UsBankAccountVerificationMethod = UsBankAccountVerificationMethod.INSTANT_VERIFICATION_ACCOUNT_VALIDATION
                }
            };
            paymentMethodRequest.UsBankAccount()
                .AchMandateText("I authorize this transaction and future debits")
                .AchMandateAcceptedAt(mandateAcceptedAt)
                .Done();

            var paymentMethodResult = usBankGateway.PaymentMethod.Create(paymentMethodRequest);
            Assert.IsTrue(paymentMethodResult.IsSuccess(), "Expected payment method creation success but got failure with validation errors");

            UsBankAccount usBankAccount = (UsBankAccount)paymentMethodResult.Target;

            // Verify verification details
            var verification = usBankAccount.Verifications.FirstOrDefault();
            Assert.IsNotNull(verification, "Expected at least one verification");
            Assert.AreEqual(UsBankAccountVerificationMethod.INSTANT_VERIFICATION_ACCOUNT_VALIDATION, verification.VerificationMethod);
            Assert.AreEqual(UsBankAccountVerificationStatus.VERIFIED, verification.Status);

            // Verify ACH mandate details
            Assert.IsNotNull(usBankAccount.AchMandate, "ACH mandate should not be null");
            Assert.AreEqual("I authorize this transaction and future debits", usBankAccount.AchMandate.Text);
            Assert.IsNotNull(usBankAccount.AchMandate.AcceptedAt, "ACH mandate AcceptedAt should not be null");

            var transactionRequest = new TransactionRequest
            {
                Amount = 12.34m,
                PaymentMethodToken = usBankAccount.Token,
                MerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            var transactionResult = usBankGateway.Transaction.Sale(transactionRequest);
            Assert.IsTrue(transactionResult.IsSuccess(), "Expected transaction success but got failure");
            Transaction transaction = transactionResult.Target;

            // Verify transaction details
            Assert.IsNotNull(transaction.Id, "Transaction ID should not be null");
            Assert.IsTrue(transaction.Id.Length > 0, "Transaction ID should not be empty");
            Assert.AreEqual(12.34m, transaction.Amount);

            // Verify US Bank Account details in transaction
            Assert.IsNotNull(transaction.UsBankAccountDetails, "UsBankAccountDetails should not be null");
            Assert.AreEqual(usBankAccount.Token, transaction.UsBankAccountDetails.Token);
            Assert.AreEqual("1234", transaction.UsBankAccountDetails.Last4);
            Assert.AreEqual("021000021", transaction.UsBankAccountDetails.RoutingNumber);
            Assert.AreEqual("checking", transaction.UsBankAccountDetails.AccountType);

            // Verify ACH mandate in transaction
            Assert.IsNotNull(transaction.UsBankAccountDetails.AchMandate, "AchMandate should not be null in transaction");
            Assert.AreEqual("I authorize this transaction and future debits", transaction.UsBankAccountDetails.AchMandate.Text);
            Assert.IsNotNull(transaction.UsBankAccountDetails.AchMandate.AcceptedAt, "AchMandate AcceptedAt should not be null in transaction");
        }

        [Test]
        public void ChargeUsBankWithAchMandateInstantVerification()
        {
            string nonce = GenerateUsBankAccountNonceViaOpenBanking();

            DateTime mandateAcceptedAt = DateTime.UtcNow.AddMinutes(-5);

            var transactionRequest = new TransactionRequest
            {
                Amount = 12.34m,
                PaymentMethodNonce = nonce,
                MerchantAccountId = MerchantAccountIDs.US_BANK_MERCHANT_ACCOUNT_ID,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };
            transactionRequest.UsBankAccount()
                .AchMandateText("I authorize this transaction and future debits")
                .AchMandateAcceptedAt(mandateAcceptedAt)
                .Done();

            var transactionResult = usBankGateway.Transaction.Sale(transactionRequest);

            Assert.IsTrue(transactionResult.IsSuccess(), "Expected transaction success but got failure with validation errors (see console output)");
            Transaction transaction = transactionResult.Target;

            // Verify transaction details
            Assert.IsNotNull(transaction.Id, "Transaction ID should not be null");
            Assert.IsTrue(transaction.Id.Length > 0, "Transaction ID should not be empty");
            Assert.AreEqual(12.34m, transaction.Amount);

            // Verify US Bank Account details in transaction
            Assert.IsNotNull(transaction.UsBankAccountDetails, "UsBankAccountDetails should not be null");
            Assert.AreEqual("Dan Schulman", transaction.UsBankAccountDetails.AccountHolderName);
            Assert.AreEqual("1234", transaction.UsBankAccountDetails.Last4);
            Assert.AreEqual("021000021", transaction.UsBankAccountDetails.RoutingNumber);
            Assert.AreEqual("checking", transaction.UsBankAccountDetails.AccountType);

            // Verify ACH mandate in transaction
            Assert.IsNotNull(transaction.UsBankAccountDetails.AchMandate, "AchMandate should not be null in transaction");
            Assert.AreEqual("I authorize this transaction and future debits", transaction.UsBankAccountDetails.AchMandate.Text);
            Assert.IsNotNull(transaction.UsBankAccountDetails.AchMandate.AcceptedAt, "AchMandate AcceptedAt should not be null in transaction");
        }


    }
}