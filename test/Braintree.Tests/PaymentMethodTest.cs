using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Braintree;
using Braintree.Exceptions;
using Braintree.Test;

using Params = System.Collections.Generic.Dictionary<string, object>;

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
        [Category("Integration")]
        public void Create_CreatesPayPalAccountWithFuturePaymentNonce()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            string nonce = TestHelper.GenerateFuturePaymentPayPalNonce(gateway);
            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(paymentMethodResult.IsSuccess());
            Assert.IsNotNull(paymentMethodResult.Target.Token);
            Assert.IsNotNull(paymentMethodResult.Target.ImageUrl);
            Assert.AreEqual(result.Target.Id, paymentMethodResult.Target.CustomerId);
            Assert.IsInstanceOf<PayPalAccount>(paymentMethodResult.Target);
        }

        [Test]
        [Category("Integration")]
        public void Create_CreatesPayPalAccountWithOneTimePaymentNonceFails()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            string nonce = TestHelper.GenerateOneTimePayPalNonce(gateway);
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
        [Category("Integration")]
        public void Create_CreatesCreditCardWithNonce()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
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
            Assert.AreEqual(result.Target.Id, paymentMethodResult.Target.CustomerId);
            Assert.IsInstanceOf(typeof(CreditCard), paymentMethodResult.Target);
        }

        [Test]
        [Category("Integration")]
        public void Create_CreatesCreditCardWithNonceAndDeviceData()
        {
            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            var request = new PaymentMethodRequest()
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = nonce,
                Options = new PaymentMethodOptionsRequest()
                {
                    VerifyCard = true
                },
                DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"my_fmid\"}"
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(paymentMethodResult.IsSuccess());
            Assert.IsNotNull(paymentMethodResult.Target.Token);
            Assert.AreEqual(result.Target.Id, paymentMethodResult.Target.CustomerId);
        }

        [Test]
        [Category("Unit")]
        public void ToXml_IncludesDeviceData()
        {
            var request = new PaymentMethodRequest()
            {
                DeviceData = "{\"device_session_id\":\"my_dsid\", \"fraud_merchant_id\":\"my_fmid\"}"
            };

            Assert.IsTrue(request.ToXml().Contains("device_session_id"));
            Assert.IsTrue(request.ToXml().Contains("my_dsid"));
            Assert.IsTrue(request.ToXml().Contains("fraud_merchant_id"));
            Assert.IsTrue(request.ToXml().Contains("my_fmid"));
        }

        [Test]
        [Category("Integration")]
        public void Create_CreatesApplePayCardWithNonce()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = Nonce.ApplePayAmex
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(paymentMethodResult.IsSuccess());
            Assert.IsNotNull(paymentMethodResult.Target.Token);
            Assert.IsNotNull(paymentMethodResult.Target.ImageUrl);
            Assert.IsInstanceOf(typeof(ApplePayCard), paymentMethodResult.Target);
            ApplePayCard applePayCard = (ApplePayCard) paymentMethodResult.Target;
            Assert.IsNotNull(applePayCard.CardType);
            Assert.IsNotNull(applePayCard.ExpirationMonth);
            Assert.IsNotNull(applePayCard.ExpirationYear);
            Assert.IsNotNull(applePayCard.CreatedAt);
            Assert.IsNotNull(applePayCard.UpdatedAt);
            Assert.IsNotNull(applePayCard.Subscriptions);
            Assert.IsNotNull(applePayCard.PaymentInstrumentName);
            Assert.IsNotNull(applePayCard.SourceDescription);
            Assert.IsNotNull(applePayCard.IsExpired);
            Assert.AreEqual(result.Target.Id, applePayCard.CustomerId);
        }

        [Test]
        [Category("Integration")]
        public void Create_CreatesAndroidPayProxyCardWithNonce()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = Nonce.AndroidPayDiscover
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(paymentMethodResult.IsSuccess());
            Assert.IsNotNull(paymentMethodResult.Target.Token);
            Assert.IsNotNull(paymentMethodResult.Target.ImageUrl);
            Assert.IsInstanceOf(typeof(AndroidPayCard), paymentMethodResult.Target);
            AndroidPayCard androidPayCard = (AndroidPayCard) paymentMethodResult.Target;
            Assert.IsNotNull(androidPayCard.IsDefault);
            Assert.IsNotNull(androidPayCard.CardType);
            Assert.IsNotNull(androidPayCard.VirtualCardType);
            Assert.IsNotNull(androidPayCard.SourceCardType);
            Assert.IsNotNull(androidPayCard.SourceDescription);
            Assert.IsNotNull(androidPayCard.Last4);
            Assert.IsNotNull(androidPayCard.VirtualCardLast4);
            Assert.IsNotNull(androidPayCard.SourceCardLast4);
            Assert.IsNotNull(androidPayCard.Bin);
            Assert.IsNotNull(androidPayCard.ExpirationMonth);
            Assert.IsNotNull(androidPayCard.ExpirationYear);
            Assert.IsNotNull(androidPayCard.GoogleTransactionId);
            Assert.IsNotNull(androidPayCard.CreatedAt);
            Assert.IsNotNull(androidPayCard.UpdatedAt);
            Assert.IsNotNull(androidPayCard.Subscriptions);
        }

        [Test]
        [Category("Integration")]
        public void Create_CreatesAndroidPayNetworkTokenWithNonce()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = Nonce.AndroidPayMasterCard
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(paymentMethodResult.IsSuccess());
            Assert.IsNotNull(paymentMethodResult.Target.Token);
            Assert.IsNotNull(paymentMethodResult.Target.ImageUrl);
            Assert.IsInstanceOf(typeof(AndroidPayCard), paymentMethodResult.Target);
            AndroidPayCard androidPayCard = (AndroidPayCard) paymentMethodResult.Target;
            Assert.IsNotNull(androidPayCard.IsDefault);
            Assert.IsNotNull(androidPayCard.CardType);
            Assert.IsNotNull(androidPayCard.VirtualCardType);
            Assert.IsNotNull(androidPayCard.SourceCardType);
            Assert.IsNotNull(androidPayCard.SourceDescription);
            Assert.IsNotNull(androidPayCard.Last4);
            Assert.IsNotNull(androidPayCard.VirtualCardLast4);
            Assert.IsNotNull(androidPayCard.SourceCardLast4);
            Assert.IsNotNull(androidPayCard.Bin);
            Assert.IsNotNull(androidPayCard.ExpirationMonth);
            Assert.IsNotNull(androidPayCard.ExpirationYear);
            Assert.IsNotNull(androidPayCard.GoogleTransactionId);
            Assert.IsNotNull(androidPayCard.CreatedAt);
            Assert.IsNotNull(androidPayCard.UpdatedAt);
            Assert.IsNotNull(androidPayCard.Subscriptions);
        }

        [Test]
        [Category("Integration")]
        public void Create_CreatesAmexExpressCheckoutCardWithNonce()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = Nonce.AmexExpressCheckout
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(paymentMethodResult.IsSuccess());
            Assert.IsNotNull(paymentMethodResult.Target.Token);
            Assert.IsNotNull(paymentMethodResult.Target.ImageUrl);
            Assert.IsInstanceOf(typeof(AmexExpressCheckoutCard), paymentMethodResult.Target);
            AmexExpressCheckoutCard amexExpressCheckoutCard = (AmexExpressCheckoutCard) paymentMethodResult.Target;

            Assert.IsNotNull(amexExpressCheckoutCard.CardType);
            Assert.IsNotNull(amexExpressCheckoutCard.Bin);
            Assert.IsNotNull(amexExpressCheckoutCard.ExpirationMonth);
            Assert.IsNotNull(amexExpressCheckoutCard.ExpirationYear);
            Assert.IsNotNull(amexExpressCheckoutCard.CardMemberNumber);
            Assert.IsNotNull(amexExpressCheckoutCard.CardMemberExpiryDate);
            Assert.IsNotNull(amexExpressCheckoutCard.ImageUrl);
            Assert.IsNotNull(amexExpressCheckoutCard.SourceDescription);
            Assert.IsNotNull(amexExpressCheckoutCard.IsDefault);
            Assert.IsNotNull(amexExpressCheckoutCard.CreatedAt);
            Assert.IsNotNull(amexExpressCheckoutCard.UpdatedAt);
            Assert.IsNotNull(amexExpressCheckoutCard.Subscriptions);
        }

        [Test]
        [Category("Integration")]
        public void Create_CreatesVenmoAccountWithNonce()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = Nonce.VenmoAccount
            };

            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(paymentMethodResult.IsSuccess());

            VenmoAccount venmoAccount = (VenmoAccount) paymentMethodResult.Target;

            Assert.IsNotNull(venmoAccount.Username);
            Assert.IsNotNull(venmoAccount.VenmoUserId);
            Assert.IsNotNull(venmoAccount.ImageUrl);
            Assert.IsNotNull(venmoAccount.SourceDescription);
            Assert.IsNotNull(venmoAccount.IsDefault);
            Assert.IsNotNull(venmoAccount.CreatedAt);
            Assert.IsNotNull(venmoAccount.UpdatedAt);
            Assert.IsNotNull(venmoAccount.CustomerId);
            Assert.IsNotNull(venmoAccount.Subscriptions);
        }

        [Test]
        [Category("Integration")]
        public void Create_CreatesAbstractPaymentMethod()
        {
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            var request = new PaymentMethodRequest
            {
                CustomerId = result.Target.Id,
                PaymentMethodNonce = Nonce.AbstractTransactable
            };
            Result<PaymentMethod> paymentMethodResult = gateway.PaymentMethod.Create(request);

            Assert.IsTrue(paymentMethodResult.IsSuccess());
            Assert.IsNotNull(paymentMethodResult.Target.Token);
            Assert.IsNotNull(paymentMethodResult.Target.ImageUrl);
        }

        [Test]
        [Category("Integration")]
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

            string nonce = TestHelper.GenerateUnlockedNonce(gateway);
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
        [Category("Integration")]
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
        [Category("Integration")]
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
        [Category("Integration")]
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
        [Category("Integration")]
        public void Create_AllowsPassingBillingAddressOutsideTheNonce()
        {
            var customer = gateway.Customer.Create().Target;
            /* var nonce = TestHelper.GenerateUnlockedNonce(gateway, "4111111111111111", customer.Id); */
            var nonce = TestHelper.GetNonceForNewCreditCard(
                gateway,
                new Params
                {
                    { "number", "4111111111111111" },
                    { "expirationMonth", "12" },
                    { "expirationYear", "2020" },
                    { "options", new Params
                        {
                            { "validate", false }
                        }
                    }
                });

            Assert.IsFalse(string.IsNullOrEmpty(nonce));

            var result = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = nonce,
                CustomerId = customer.Id,
                BillingAddress = new PaymentMethodAddressRequest
                {
                    StreetAddress = "123 Abc Way"
                }
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.That(result.Target, Is.InstanceOf<CreditCard>());

            var token = result.Target.Token;
            var foundCreditCard = gateway.CreditCard.Find(token);
            Assert.IsNotNull(foundCreditCard);
            Assert.AreEqual("123 Abc Way", foundCreditCard.BillingAddress.StreetAddress);
        }

        [Test]
        [Category("Integration")]
        public void Create_OverridesTheBillingAddressInTheNonce()
        {
            var customer = gateway.Customer.Create().Target;
            var nonce = TestHelper.GetNonceForNewCreditCard(
                gateway,
                new Params
                {
                    { "number", "4111111111111111" },
                    { "expirationMonth", "12" },
                    { "expirationYear", "2020" },
                    { "options", new Params
                        {
                            { "validate", false }
                        }
                    }
                });

            Assert.IsFalse(string.IsNullOrEmpty(nonce));

            var result = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = nonce,
                CustomerId = customer.Id,
                BillingAddress = new PaymentMethodAddressRequest
                {
                    StreetAddress = "123 Abc Way"
                }
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.That(result.Target, Is.InstanceOf<CreditCard>());

            var token = result.Target.Token;
            var foundCreditCard = gateway.CreditCard.Find(token);
            Assert.IsNotNull(foundCreditCard);
            Assert.AreEqual("123 Abc Way", foundCreditCard.BillingAddress.StreetAddress);
        }

        [Test]
        [Category("Integration")]
        public void Create_DoesNotOverrideTheBillingAddressForVaultedCreditCards()
        {
            var customer = gateway.Customer.Create().Target;
            var nonce = TestHelper.GetNonceForNewCreditCard(
                gateway,
                new Params
                {
                    { "number", "4111111111111111" },
                    { "expirationMonth", "12" },
                    { "expirationYear", "2020" },
                    { "billing_address", new Params
                        {
                            { "street_address", "456 Xyz Way" }
                        }
                    }
                },
                customer.Id);

            Assert.IsFalse(string.IsNullOrEmpty(nonce));

            var result = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = nonce,
                CustomerId = customer.Id,
                BillingAddress = new PaymentMethodAddressRequest
                {
                    StreetAddress = "123 Abc Way"
                }
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.That(result.Target, Is.InstanceOf<CreditCard>());

            var token = result.Target.Token;
            var foundCreditCard = gateway.CreditCard.Find(token);
            Assert.IsNotNull(foundCreditCard);
            Assert.AreEqual("456 Xyz Way", foundCreditCard.BillingAddress.StreetAddress);
        }

        [Test]
        [Category("Integration")]
        public void Create_AllowsPassingBillingAddressIdOutsideTheNonce()
        {
            var customer = gateway.Customer.Create().Target;
            var nonce = TestHelper.GetNonceForNewCreditCard(
                gateway,
                new Params
                {
                    { "number", "4111111111111111" },
                    { "expirationMonth", "12" },
                    { "expirationYear", "2020" },
                    { "options", new Params

                        {
                            { "validate", "456 Xyz Way" }
                        }
                    }
                },
                customer.Id);

            Assert.IsFalse(string.IsNullOrEmpty(nonce));
            var addressResult = gateway.Address.Create(
                customer.Id,
                new AddressRequest
                {
                    FirstName = "Bobby",
                    LastName = "Tables"
                });

            Assert.IsTrue(addressResult.IsSuccess());
            var address = addressResult.Target;
            var result = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = nonce,
                CustomerId = customer.Id,
                BillingAddressId = address.Id
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.That(result.Target, Is.InstanceOf<CreditCard>());

            var token = result.Target.Token;
            var foundCreditCard = gateway.CreditCard.Find(token);
            Assert.IsNotNull(foundCreditCard);
            Assert.AreEqual("Bobby", foundCreditCard.BillingAddress.FirstName);
            Assert.AreEqual("Tables", foundCreditCard.BillingAddress.LastName);
        }

        [Test]
        [Category("Integration")]
        public void Create_IgnoresPassedBillingAddressParamsForPayPal()
        {
            var nonce = TestHelper.GetNonceForPayPalAccount(
                gateway,
                new Params
                {
                    { "consent-code", "PAYPAL_CONSENT_CODE" }
                });

            var customer = gateway.Customer.Create().Target;
            var result = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = nonce,
                CustomerId = customer.Id,
                BillingAddress = new PaymentMethodAddressRequest
                {
                    StreetAddress = "123 Abc Way"
                }
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.That(result.Target, Is.InstanceOf<PayPalAccount>());
            Assert.IsNotNull(result.Target.ImageUrl);

            var token = result.Target.Token;
            var foundPaypalAccount = gateway.PayPalAccount.Find(token);
            Assert.IsNotNull(foundPaypalAccount);
        }

        [Test]
        [Category("Integration")]
        public void Create_IgnoresPassedBillingAddressIdForPayPalAccount()
        {
            var nonce = TestHelper.GetNonceForPayPalAccount(
                gateway,
                new Params
                {
                    { "consent-code", "PAYPAL_CONSENT_CODE" }
                });

            var customer = gateway.Customer.Create().Target;
            var result = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = nonce,
                CustomerId = customer.Id,
                BillingAddressId = "address_id"
            });

            Assert.IsTrue(result.IsSuccess());
            Assert.That(result.Target, Is.InstanceOf<PayPalAccount>());
            Assert.IsNotNull(result.Target.ImageUrl);

            var token = result.Target.Token;
            var foundPaypalAccount = gateway.PayPalAccount.Find(token);
            Assert.IsNotNull(foundPaypalAccount);
        }

        [Test]
        [Category("Integration")]
        public void Delete_DeletesCreditCard()
        {
            var request = new PaymentMethodRequest
            {
                CustomerId = gateway.Customer.Create(new CustomerRequest()).Target.Id,
                PaymentMethodNonce = Nonce.Transactable
            };
            Result<PaymentMethod> createResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(createResult.IsSuccess());

            Result<PaymentMethod> deleteResult = gateway.PaymentMethod.Delete(createResult.Target.Token);

            Assert.IsTrue(deleteResult.IsSuccess());
        }

        [Test]
        [Category("Integration")]
        public void Delete_DeletesPayPalAccount()
        {
            var request = new PaymentMethodRequest
            {
                CustomerId = gateway.Customer.Create(new CustomerRequest()).Target.Id,
                PaymentMethodNonce = Nonce.PayPalFuturePayment
            };
            Result<PaymentMethod> createResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(createResult.IsSuccess());

            Result<PaymentMethod> deleteResult = gateway.PaymentMethod.Delete(createResult.Target.Token);

            Assert.IsTrue(deleteResult.IsSuccess());
        }

        [Test]
        [Category("Integration")]
        public void Delete_DeletesAndroidPayAccount()
        {
            var request = new PaymentMethodRequest
            {
                CustomerId = gateway.Customer.Create(new CustomerRequest()).Target.Id,
                PaymentMethodNonce = Nonce.AndroidPay
            };
            Result<PaymentMethod> createResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(createResult.IsSuccess());

            Result<PaymentMethod> deleteResult = gateway.PaymentMethod.Delete(createResult.Target.Token);

            Assert.IsTrue(deleteResult.IsSuccess());
        }

        [Test]
        [Category("Integration")]
        public void Delete_DeletesApplePayAccount()
        {
            var request = new PaymentMethodRequest
            {
                CustomerId = gateway.Customer.Create(new CustomerRequest()).Target.Id,
                PaymentMethodNonce = Nonce.ApplePayVisa
            };
            Result<PaymentMethod> createResult = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(createResult.IsSuccess());

            Result<PaymentMethod> deleteResult = gateway.PaymentMethod.Delete(createResult.Target.Token);

            Assert.IsTrue(deleteResult.IsSuccess());
        }

        [Test]
        [Category("Integration")]
        public void Delete_RaisesNotFoundErrorWhenTokenDoesntExist()
        {
            Assert.Throws<NotFoundException>(() => gateway.PaymentMethod.Delete(" "));
        }

        [Test]
        [Category("Integration")]
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
        [Category("Integration")]
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
        [Category("Integration")]
        public void Find_FindsApplePayCard()
        {
            var request = new PaymentMethodRequest
            {
                CustomerId = gateway.Customer.Create(new CustomerRequest()).Target.Id,
                PaymentMethodNonce = Nonce.ApplePayAmex
            };
            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());

            PaymentMethod found = gateway.PaymentMethod.Find(result.Target.Token);
            Assert.AreEqual(result.Target.Token, found.Token);
            Assert.IsInstanceOf(typeof(ApplePayCard), found);
        }

        [Test]
        [Category("Integration")]
        public void Find_FindsAndroidPayCard()
        {
            var request = new PaymentMethodRequest
            {
                CustomerId = gateway.Customer.Create(new CustomerRequest()).Target.Id,
                PaymentMethodNonce = Nonce.AndroidPay
            };
            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());

            PaymentMethod found = gateway.PaymentMethod.Find(result.Target.Token);
            Assert.AreEqual(result.Target.Token, found.Token);
            Assert.IsInstanceOf(typeof(AndroidPayCard), found);
        }

        [Test]
        [Category("Integration")]
        public void Find_FindsAbstractPaymentMethod()
        {
            var request = new PaymentMethodRequest
            {
                CustomerId = gateway.Customer.Create(new CustomerRequest()).Target.Id,
                PaymentMethodNonce = Nonce.AbstractTransactable
            };
            Result<PaymentMethod> result = gateway.PaymentMethod.Create(request);
            Assert.IsTrue(result.IsSuccess());

            PaymentMethod found = gateway.PaymentMethod.Find(result.Target.Token);
            Assert.AreEqual(result.Target.Token, found.Token);
        }

        [Test]
        [Category("Unit")]
        public void Find_RaisesNotFoundErrorWhenTokenIsBlank()
        {
            Assert.Throws<NotFoundException>(() =>gateway.PaymentMethod.Find(" "));
        }

        [Test]
        [Category("Integration")]
        public void Find_RaisesNotFoundErrorWhenTokenDoesntExist()
        {
            Assert.Throws<NotFoundException>(() => gateway.PaymentMethod.Find("missing"));
        }

        [Test]
        [Category("Integration")]
        public void Update_UpdatesTheCreditCard()
        {
            var MASTERCARD = SandboxValues.CreditCardNumber.MASTER_CARD;
            var customer = gateway.Customer.Create().Target;
            var creditCard = gateway.CreditCard.Create(new CreditCardRequest
            {
                CardholderName = "Original Holder",
                CustomerId = customer.Id,
                CVV = "123",
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2012"
            }).Target;

            var updateResult = gateway.PaymentMethod.Update(
                creditCard.Token,
                new PaymentMethodRequest
                {
                    CardholderName = "New Holder",
                    CVV = "456",
                    Number = MASTERCARD,
                    ExpirationDate = "06/2013"
                });

            Assert.IsTrue(updateResult.IsSuccess());
            Assert.That(updateResult.Target, Is.InstanceOf<CreditCard>());

            var updatedCreditCard = (CreditCard)updateResult.Target;
            Assert.AreEqual("New Holder", updatedCreditCard.CardholderName);
            Assert.AreEqual(MASTERCARD.Substring(0, 6), updatedCreditCard.Bin);
            Assert.AreEqual(MASTERCARD.Substring(MASTERCARD.Length - 4), updatedCreditCard.LastFour);
            Assert.AreEqual("06/2013", updatedCreditCard.ExpirationDate);
        }

        [Test]
        [Category("Integration")]
        public void Update_UpdatesTheCoinbaseAccount()
        {
            var customer = gateway.Customer.Create().Target;
            PaymentMethodRequest request = new PaymentMethodRequest()
            {
                CustomerId = customer.Id,
                PaymentMethodNonce = Nonce.Coinbase
            };
            var coinbaseAccount = gateway.PaymentMethod.Create(request).Target;

            var updateResult = gateway.PaymentMethod.Update(
                coinbaseAccount.Token,
                new PaymentMethodRequest
                {
                    Options = new PaymentMethodOptionsRequest { MakeDefault = true }
                });

            Assert.IsTrue(updateResult.IsSuccess());
            Assert.That(updateResult.Target, Is.InstanceOf<CoinbaseAccount>());

            var updatedCoinbaseAccount = (CoinbaseAccount)updateResult.Target;
            Assert.IsTrue(updatedCoinbaseAccount.IsDefault.Value);
        }

        [Test]
        [Category("Integration")]
        public void Update_CreatesNewBillingAddressByDefault()
        {
            var customer = gateway.Customer.Create().Target;
            var creditCard = gateway.CreditCard.Create(new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2012",
                BillingAddress = new CreditCardAddressRequest
                {
                    StreetAddress = "123 Nigeria Ave"
                }
            }).Target;

            var updateResult = gateway.PaymentMethod.Update(
                creditCard.Token,
                new PaymentMethodRequest
                {
                    BillingAddress = new PaymentMethodAddressRequest
                    {
                        Region = "IL",
                    }
                });

            Assert.IsTrue(updateResult.IsSuccess());
            Assert.That(updateResult.Target, Is.InstanceOf<CreditCard>());

            var updatedCreditCard = (CreditCard)updateResult.Target;
            Assert.AreEqual("IL", updatedCreditCard.BillingAddress.Region);
            Assert.IsNull(updatedCreditCard.BillingAddress.StreetAddress);
            Assert.AreNotEqual(updatedCreditCard.BillingAddress.Id, creditCard.BillingAddress.Id);
        }

        [Test]
        [Category("Integration")]
        public void Update_UpdatesTheBillingAddressIfOptionIsSpecified()
        {
            var customer = gateway.Customer.Create().Target;
            var creditCard = gateway.CreditCard.Create(new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2012",
                BillingAddress = new CreditCardAddressRequest
                {
                    StreetAddress = "123 Nigeria Ave"
                }
            }).Target;

            var updateResult = gateway.PaymentMethod.Update(
                creditCard.Token,
                new PaymentMethodRequest
                {
                    BillingAddress = new PaymentMethodAddressRequest
                    {
                        Region = "IL",
                        Options = new PaymentMethodAddressOptionsRequest
                        {
                            UpdateExisting = true
                        }
                    }
                });

            Assert.IsTrue(updateResult.IsSuccess());
            Assert.That(updateResult.Target, Is.InstanceOf<CreditCard>());

            var updatedCreditCard = (CreditCard)updateResult.Target;
            Assert.AreEqual("IL", updatedCreditCard.BillingAddress.Region);
            Assert.AreEqual("123 Nigeria Ave", updatedCreditCard.BillingAddress.StreetAddress);
            Assert.AreEqual(updatedCreditCard.BillingAddress.Id, creditCard.BillingAddress.Id);
        }

        [Test]
        [Category("Integration")]
        public void Update_UpdatesCountryViaCodes()
        {
            var customer = gateway.Customer.Create().Target;
            var creditCard = gateway.CreditCard.Create(new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2012",
                BillingAddress = new CreditCardAddressRequest
                {
                    StreetAddress = "123 Nigeria Ave"
                }
            }).Target;

            var updateResult = gateway.PaymentMethod.Update(
                creditCard.Token,
                new PaymentMethodRequest
                {
                    BillingAddress = new PaymentMethodAddressRequest
                    {
                        CountryName = "American Samoa",
                        CountryCodeAlpha2 = "AS",
                        CountryCodeAlpha3 = "ASM",
                        CountryCodeNumeric = "016",
                        Options = new PaymentMethodAddressOptionsRequest
                        {
                            UpdateExisting = true
                        }
                    }
                });

            Assert.IsTrue(updateResult.IsSuccess());
            var updatedCreditCard = (CreditCard)updateResult.Target;
            Assert.AreEqual("American Samoa", updatedCreditCard.BillingAddress.CountryName);
            Assert.AreEqual("AS", updatedCreditCard.BillingAddress.CountryCodeAlpha2);
            Assert.AreEqual("ASM", updatedCreditCard.BillingAddress.CountryCodeAlpha3);
            Assert.AreEqual("016", updatedCreditCard.BillingAddress.CountryCodeNumeric);
        }

        [Test]
        [Category("Integration")]
        public void Update_CanPassExpirationMonthAndExpirationYear()
        {
            var customer = gateway.Customer.Create().Target;
            var creditCard = gateway.CreditCard.Create(new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2012",
            }).Target;

            var updateResult = gateway.PaymentMethod.Update(
                creditCard.Token,
                new PaymentMethodRequest
                {
                    Number = SandboxValues.CreditCardNumber.MASTER_CARD,
                    ExpirationMonth = "07",
                    ExpirationYear = "2011"
                });

            Assert.IsTrue(updateResult.IsSuccess());
            Assert.That(updateResult.Target, Is.InstanceOf<CreditCard>());
            var updatedCreditCard = (CreditCard)updateResult.Target;
            Assert.AreEqual("07", updatedCreditCard.ExpirationMonth);
            Assert.AreEqual("2011", updatedCreditCard.ExpirationYear);
            Assert.AreEqual("07/2011", updatedCreditCard.ExpirationDate);
        }

        [Test]
        [Category("Integration")]
        public void Update_VerifiesTheUpdateIfOptionsVerifyCardIsTrue()
        {
            var customer = gateway.Customer.Create().Target;
            var creditCard = gateway.CreditCard.Create(new CreditCardRequest
            {
                CardholderName = "Original Holder",
                CustomerId = customer.Id,
                CVV = "123",
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2012",
            }).Target;

            var updateResult = gateway.PaymentMethod.Update(
                creditCard.Token,
                new PaymentMethodRequest
                {
                    CardholderName = "New Holder",
                    CVV = "456",
                    Number = CreditCardNumbers.FailsSandboxVerification.MasterCard,
                    ExpirationDate = "06/2013",
                    Options = new PaymentMethodOptionsRequest
                    {
                        VerifyCard = true
                    }
                });

            Assert.IsFalse(updateResult.IsSuccess());
            Assert.IsNotNull(updateResult.CreditCardVerification);
            Assert.AreEqual(VerificationStatus.PROCESSOR_DECLINED, updateResult.CreditCardVerification.Status);
            Assert.IsNull(updateResult.CreditCardVerification.GatewayRejectionReason);
        }

        [Test]
        [Category("Integration")]
        public void Update_CanUpdateTheBillingAddress()
        {
            var customer = gateway.Customer.Create().Target;
            var creditCard = gateway.CreditCard.Create(new CreditCardRequest
            {
                CardholderName = "Original Holder",
                CustomerId = customer.Id,
                CVV = "123",
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2012",
                BillingAddress = new CreditCardAddressRequest
                {
                    FirstName = "Old First Name",
                    LastName = "Old Last Name",
                    Company = "Old Company",
                    StreetAddress = "123 Old St",
                    ExtendedAddress = "Apt Old",
                    Locality = "Old City",
                    Region = "Old State",
                    PostalCode = "12345",
                    CountryName = "Canada"
                }
            }).Target;

            var result = gateway.PaymentMethod.Update(
                creditCard.Token,
                new PaymentMethodRequest
                {
                    Options = new PaymentMethodOptionsRequest
                    {
                        VerifyCard = false
                    },
                    BillingAddress = new PaymentMethodAddressRequest
                    {
                        FirstName = "New First Name",
                        LastName = "New Last Name",
                        Company = "New Company",
                        StreetAddress = "123 New St",
                        Locality = "New City",
                        Region = "New State",
                        PostalCode = "56789",
                        CountryName = "United States of America"
                    }
                });

            Assert.IsTrue(result.IsSuccess());
            Assert.That(result.Target, Is.InstanceOf<CreditCard>());
            var address = ((CreditCard)result.Target).BillingAddress;
            Assert.AreEqual("New First Name", address.FirstName);
            Assert.AreEqual("New Last Name", address.LastName);
            Assert.AreEqual("New Company", address.Company);
            Assert.AreEqual("123 New St", address.StreetAddress);
            Assert.AreEqual("New City", address.Locality);
            Assert.AreEqual("New State", address.Region);
            Assert.AreEqual("56789", address.PostalCode);
            Assert.AreEqual("United States of America", address.CountryName);
        }

        [Test]
        [Category("Integration")]
        public void Update_ReturnsAnErrorResponseIfInvalid()
        {
            var customer = gateway.Customer.Create().Target;
            var creditCard = gateway.CreditCard.Create(new CreditCardRequest
            {
                CardholderName = "Original Holder",
                CustomerId = customer.Id,
                CVV = "123",
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2012",
            }).Target;

            var result = gateway.PaymentMethod.Update(
                creditCard.Token,
                new PaymentMethodRequest
                {
                    CardholderName = "New Holder",
                    Number = "invalid",
                    ExpirationDate = "05/2014"
                });

            Assert.IsFalse(result.IsSuccess());
            Assert.AreEqual("Credit card number must be 12-19 digits.", result.Errors.ForObject("credit_card").OnField("number")[0].Message);
        }

        [Test]
        [Category("Integration")]
        public void Update_CanUpdateTheDefault()
        {
            var customer = gateway.Customer.Create().Target;
            var card1 = gateway.CreditCard.Create(new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2009",
            }).Target;

            var card2 = gateway.CreditCard.Create(new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2009",
            }).Target;

            Assert.IsTrue(card1.IsDefault.Value);
            Assert.IsFalse(card2.IsDefault.Value);
            gateway.PaymentMethod.Update(
                card2.Token,
                new PaymentMethodRequest
                {
                    Options = new PaymentMethodOptionsRequest { MakeDefault = true }
                });

            Assert.IsFalse(gateway.CreditCard.Find(card1.Token).IsDefault.Value);
            Assert.IsTrue(gateway.CreditCard.Find(card2.Token).IsDefault.Value);
        }

        [Test]
        [Category("Integration")]
        public void Update_UpdatesPayPalAccountToken()
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

            var originalResult = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = nonce,
                CustomerId = customer.Id
            });

            Assert.That(originalResult.Target, Is.InstanceOf<PayPalAccount>());
            var updatedToken = string.Format("UPDATED_TOKEN-{0}", DateTime.Now.Ticks);
            var updatedResult = gateway.PaymentMethod.Update(
                originalToken,
                new PaymentMethodRequest
                {
                    Token = updatedToken
                });

            Assert.IsTrue(updatedResult.IsSuccess());
            var updatedPaypalAccount = gateway.PayPalAccount.Find(updatedToken);
            Assert.AreEqual(((PayPalAccount)originalResult.Target).Email, updatedPaypalAccount.Email);

            Exception exception = null;
            try {
                gateway.PayPalAccount.Find(originalToken);
            } catch (Exception e) {
                exception = e;
            }

            Assert.IsNotNull(exception);
            Assert.IsInstanceOf(typeof(NotFoundException), exception);
        }

        [Test]
        [Category("Integration")]
        public void Update_CanMakePayPalAccountsTheDefaultPaymentMethod()
        {
            var customer = gateway.Customer.Create().Target;
            var result = gateway.CreditCard.Create(new CreditCardRequest
            {
                CustomerId = customer.Id,
                Number = SandboxValues.CreditCardNumber.VISA,
                ExpirationDate = "05/2009",
                Options = new CreditCardOptionsRequest
                {
                    MakeDefault = true
                }
            });

            Assert.IsTrue(result.IsSuccess());
            var nonce = TestHelper.GetNonceForPayPalAccount(gateway, new Params {{ "consent_code", "consent-code" }});
            var originalToken = gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = nonce,
                CustomerId = customer.Id
            }).Target.Token;

            var updatedResult = gateway.PaymentMethod.Update(
                originalToken,
                new PaymentMethodRequest { Options = new PaymentMethodOptionsRequest { MakeDefault = true }});

            Assert.IsTrue(updatedResult.IsSuccess());
            var updatedPaypalAccount = gateway.PayPalAccount.Find(originalToken);
            Assert.IsTrue(updatedPaypalAccount.IsDefault.Value);
        }

        [Test]
        [Category("Integration")]
        public void Update_ReturnsAnErrorIfTokenForAccountIsUsedToAttemptUpdate()
        {
            var customer = gateway.Customer.Create().Target;
            var firstToken = string.Format("paypal-account-{0}", DateTime.Now.Ticks);
            var secondToken = string.Format("paypal-account-{0}", DateTime.Now.Ticks);

            var firstNonce = TestHelper.GetNonceForPayPalAccount(
                gateway,
                new Params
                {
                    { "consent_code", "consent-code" },
                    { "token", firstToken }
                });

            gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = firstNonce,
                CustomerId = customer.Id
            });

            var secondNonce = TestHelper.GetNonceForPayPalAccount(
                gateway,
                new Params
                {
                    { "consent_code", "consent-code" },
                    { "token", secondToken }
                });

            gateway.PaymentMethod.Create(new PaymentMethodRequest
            {
                PaymentMethodNonce = secondNonce,
                CustomerId = customer.Id
            });

            var updatedResult = gateway.PaymentMethod.Update(
                firstToken,
                new PaymentMethodRequest { Token = secondToken });

            Assert.IsFalse(updatedResult.IsSuccess());
            Assert.AreEqual("92906", ((int)updatedResult.Errors.DeepAll().First().Code).ToString());
        }
    }
}
