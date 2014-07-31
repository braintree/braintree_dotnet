using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;
using Braintree.Test;

using Params = System.Collections.Generic.Dictionary<string, string>;

namespace Braintree.Tests
{
    [TestFixture]
    public class PaymentMethodTest
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
        public void Create_CreatesPayPalAccountWithFuturePaymentNonce()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            String nonce = TestHelper.GenerateFuturePaymentPayPalNonce(gateway);
            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(paymentMethodResult.IsSuccess());
            Assert.IsNotNull(paymentMethodResult.Target.Token);
            Assert.IsNotNull(paymentMethodResult.Target.ImageUrl);
            Assert.IsInstanceOfType(typeof(PayPalAccount), paymentMethodResult.Target);
        }

        [Test]
        public void Create_CreatesPayPalAccountWithOneTimePaymentNonceFails()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            String nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsFalse(paymentMethodResult.IsSuccess());
            Assert.AreEqual(
                ValidationErrorCode.PAYPAL_ACCOUNT_CANNOT_VAULT_ONE_TIME_USE_PAYPAL_ACCOUNT,
                paymentMethodResult.Errors.ForObject("paypal-account").OnField("base")[0].Code
            );
        }

        [Test]
        public void Create_CreatesCreditCardWithNonce()
        {
            String nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(paymentMethodResult.IsSuccess());
            Assert.IsNotNull(paymentMethodResult.Target.Token);
            Assert.IsInstanceOfType(typeof(CreditCard), paymentMethodResult.Target);
        }

        [Test]
        public void Create_CanMakeDefaultAndSetToken()
        {
            Result<Customer> customerResult = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(customerResult.IsSuccess());
            var creditCardRequest = new CreditCardRequest
            {
                CustomerId = customerResult.Target.Id,
                Number = "5105105105105100",
                ExpirationDate = "05/12"
            };
            CreditCard creditCard = gateway.CreditCard.Create(creditCardRequest).Target;
            Assert.IsTrue(creditCard.IsDefault.Value);

            String nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Random random = new Random();
            int randomNumber = random.Next(0, 10000);
            var token = "token_" + randomNumber;
            var request = new PaymentMethodRequest
            {
                CustomerId = customerResult.Target.Id,
                PaymentMethodNonce = nonce,
                Token = token,
                Options = new PaymentMethodOptionsRequest
                {
                    MakeDefault = true
                }
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(paymentMethodResult.IsSuccess());
            Assert.IsTrue(paymentMethodResult.Target.IsDefault.Value);
            Assert.AreEqual(token, paymentMethodResult.Target.Token);
        }

        [Test]
        public void Create_DoesntReturnErrorIfCreditCardOptionsArePresentForPayPalNonce()
        {
            var customer = gateway.Customer.Create().Target;
            var originalToken = string.Format("paypal-account-{0}", DateTime.Now.Ticks);
            var nonce = TestHelper.GetNonceForPayPalAccount(
                gateway,
                new Params
                {
                    { "consent_code", "consent-code" },
                    { "token", originalToken }
                });

            var result = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = nonce,
                CustomerId = customer.Id,
                Options = new PaymentMethodOptionsRequest
                {
                    VerifyCard = true,
                    FailOnDuplicatePaymentMethod = true,
                    VerificationMerchantAccountId = "not_a_real_merchant_account_id"
                }
            });

            Assert.IsTrue(result.IsSuccess());
        }

        [Test]
        public void Create_RespectsVerifyCardAndVerificationMerchantAccountIdOutsideTheNonce()
        {
            var nonce = TestHelper.GetNonceForNewPaymentMethod(
                gateway,
                new Params
                {
                    { "number", "4000111111111115" },
                    { "expiration_month", "11" },
                    { "expiration_year", "2099" }
                },
                isCreditCard : true);

            var customer = gateway.Customer.Create().Target;
            var result = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = nonce,
                CustomerId = customer.Id,
                Options = new PaymentMethodOptionsRequest
                {
                    VerifyCard = true,
                    VerificationMerchantAccountId = MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID
                }
            });

            Assert.IsFalse(result.IsSuccess());
            Assert.IsNotNull(result.CreditCardVerification);
            Assert.AreEqual(VerificationStatus.PROCESSOR_DECLINED, result.CreditCardVerification.Status);
            Assert.AreEqual("2000", result.CreditCardVerification.ProcessorResponseCode);
            Assert.AreEqual("Do Not Honor", result.CreditCardVerification.ProcessorResponseText);
            Assert.AreEqual(MerchantAccountIDs.NON_DEFAULT_MERCHANT_ACCOUNT_ID, result.CreditCardVerification.MerchantAccountId);
        }

        [Test]
        public void Create_RespectsFailOnDuplicatePaymentMethodWhenIncludedOutsideNonce()
        {
            var customer = gateway.Customer.Create().Target;
            var creditCardResult = gateway.CreditCard.Create(new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2012"
            });

            Assert.IsTrue(creditCardResult.IsSuccess());
            var nonce = TestHelper.GetNonceForNewPaymentMethod(
                gateway,
                new Params
                {
                    { "number", SandboxValues.CreditCardNumber.VISA },
                    { "expiration_date", "05/2012" }
                },
                isCreditCard : true
            );

            var paypalResult = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = nonce,
                CustomerId = customer.Id,
                Options = new PaymentMethodOptionsRequest
                {
                    FailOnDuplicatePaymentMethod = true
                }
            });

            Assert.IsFalse(paypalResult.IsSuccess());
            Assert.AreEqual(ValidationErrorCode.CREDIT_CARD_DUPLICATE_CARD_EXISTS, paypalResult.Errors.DeepAll().First().Code);
        }

        [Test]
        public void Delete_DeletesCreditCard()
        {
            var request = new PaymentMethodRequest
            {
                CustomerId = gateway.Customer.Create(new CustomerRequest()).Target.Id,
                PaymentMethodNonce = Nonce.Transactable
            };
            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());

            gateway.PaymentMethod.Delete(result.Target.Token);
        }

        [Test]
        public void Delete_DeletesPayPalAccount()
        {
            var request = new PaymentMethodRequest
            {
                CustomerId = gateway.Customer.Create(new CustomerRequest()).Target.Id,
                PaymentMethodNonce = Nonce.PayPalFuturePayment
            };
            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());

            gateway.PaymentMethod.Delete(result.Target.Token);
        }

        [Test]
        public void Delete_RaisesNotFoundErrorWhenTokenDoesntExist()
        {
            try
            {
                gateway.PaymentMethod.Delete(" ");
                Assert.Fail("Should have raised NotFoundException");
            }
            catch(NotFoundException) {}
        }

        [Test]
        public void Find_FindsCreditCard()
        {
            var request = new PaymentMethodRequest
            {
                CustomerId = gateway.Customer.Create(new CustomerRequest()).Target.Id,
                PaymentMethodNonce = Nonce.Transactable
            };
            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());

            PaymentMethod found = gateway.PaymentMethod.Find(result.Target.Token);
            Assert.AreEqual(result.Target.Token, found.Token);
        }

        [Test]
        public void Find_FindsPayPalAccount()
        {
            var request = new PaymentMethodRequest
            {
                CustomerId = gateway.Customer.Create(new CustomerRequest()).Target.Id,
                PaymentMethodNonce = Nonce.PayPalFuturePayment
            };
            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());

            PaymentMethod found = gateway.PaymentMethod.Find(result.Target.Token);
            Assert.AreEqual(result.Target.Token, found.Token);
        }

        [Test]
        public void Find_RaisesNotFoundErrorWhenTokenIsBlank()
        {
            try
            {
                gateway.PaymentMethod.Find(" ");
                Assert.Fail("Should have raised NotFoundException");
            }
            catch(NotFoundException) {}
        }

        [Test]
        public void Find_RaisesNotFoundErrorWhenTokenDoesntExist()
        {
            try
            {
                gateway.PaymentMethod.Find("missing");
                Assert.Fail("Should have raised NotFoundException");
            }
            catch(NotFoundException) {}
        }
    }
}
