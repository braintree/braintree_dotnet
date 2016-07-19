using System;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
    [TestFixture]
    public class ClientTokenTest
    {
        [Test]
        [Category("Unit")]
        public void Generate_RaisesExceptionIfVerifyCardIsIncludedWithoutCustomerId()
        {
          BraintreeGateway gateway = new BraintreeGateway
          {
              Environment = Environment.DEVELOPMENT,
              MerchantId = "integration_merchant_id",
              PublicKey = "integration_public_key",
              PrivateKey = "integration_private_key"
          };
          Exception exception = null;
          try {
              gateway.ClientToken.generate(
                  new ClientTokenRequest
                  {
                      Options = new ClientTokenOptionsRequest
                      {
                          VerifyCard = true
                      }
                  }
              );
          } catch (Exception tempException) {
              exception = tempException;
          }
          Assert.IsNotNull(exception);
          Assert.IsTrue(Regex.Match(exception.Message, @"VerifyCard").Success);
          Assert.IsInstanceOf<ArgumentException>(exception);
        }

        [Test]
        [Category("Unit")]
        public void Generate_RaisesExceptionIfMakeDefaultIsIncludedWithoutCustomerId()
        {
          BraintreeGateway gateway = new BraintreeGateway
          {
              Environment = Environment.DEVELOPMENT,
              MerchantId = "integration_merchant_id",
              PublicKey = "integration_public_key",
              PrivateKey = "integration_private_key"
          };
          Exception exception = null;
          try {
              gateway.ClientToken.generate(
                  new ClientTokenRequest
                  {
                      Options = new ClientTokenOptionsRequest
                      {
                          MakeDefault = true
                      }
                  }
              );
          } catch (Exception tempException) {
              exception = tempException;
          }
          Assert.IsNotNull(exception);
          Assert.IsTrue(Regex.Match(exception.Message, @"MakeDefault").Success);
          Assert.IsInstanceOf<ArgumentException>(exception);
        }

        [Test]
        [Category("Unit")]
        public void Generate_RaisesExceptionIfFailOnDuplicatePaymentMethodIsIncludedWithoutCustomerId()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            Exception exception = null;
            try {
                gateway.ClientToken.generate(
                    new ClientTokenRequest
                    {
                        Options = new ClientTokenOptionsRequest
                        {
                            FailOnDuplicatePaymentMethod = true
                        }
                    }
                );
            } catch (Exception tempException) {
                exception = tempException;
            }
            Assert.IsNotNull(exception);
            Assert.IsTrue(Regex.Match(exception.Message, @"FailOnDuplicatePaymentMethod").Success);
            Assert.IsInstanceOf<ArgumentException>(exception);
        }
    }

    [TestFixture]
    public class ClientTokenTestIT
    {
        [Test]
        [Category("Integration")]
        public void Generate_GeneratedFingerprintIsAcceptedByGateway()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            var clientToken = TestHelper.GenerateDecodedClientToken(gateway);
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            var encodedFingerprint = WebUtility.UrlEncode(authorizationFingerprint);
            var url = "v1/payment_methods.json";
            url += "?authorizationFingerprint=" + encodedFingerprint;
            url += "&sharedCustomerIdentifierType=testing";
            url += "&sharedCustomerIdentifier=test-identifier";

            var response = new BraintreeTestHttpService().Get(gateway.MerchantId, url);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            response.Dispose();
        }

        [Test]
        [Category("Integration")]
        public void Generate_SupportsVersionOption()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            var clientToken = gateway.ClientToken.generate(
                new ClientTokenRequest
                {
                    Version = 1
                }
            );
            int version = TestHelper.extractIntParamFromJson("version", clientToken);
            Assert.AreEqual(1, version);
        }

        [Test]
        [Category("Integration")]
        public void Generate_DefaultsToVersionTwo()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            var encodedClientToken = gateway.ClientToken.generate();
            var decodedClientToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedClientToken));
            int version = TestHelper.extractIntParamFromJson("version", decodedClientToken);
            Assert.AreEqual(2, version);
        }

        [Test]
        [Category("Integration")]
        public void Generate_GatewayRespectsVerifyCard()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());

            string customerId = result.Target.Id;
            var clientToken = TestHelper.GenerateDecodedClientToken(gateway,
                new ClientTokenRequest
                {
                    CustomerId = customerId,
                    Options = new ClientTokenOptionsRequest
                    {
                        VerifyCard = true
                    }
                }
            );
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier_type", "testing").
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("credit_card[number]", "4000111111111115").
                AddTopLevelElement("credit_card[expiration_month]", "11").
                AddTopLevelElement("credit_card[expiration_year]", "2099");

            var response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/credit_cards.json", builder.ToQueryString());
            Assert.AreEqual(422, (int)response.StatusCode);

            Customer customer = gateway.Customer.Find(customerId);
            Assert.AreEqual(0, customer.CreditCards.Length);
        }

        [Test]
        [Category("Integration")]
        public void Generate_GatewayRespectsFailOnDuplicatePaymentMethod()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());
            string customerId = result.Target.Id;

            var request = new CreditCardRequest
            {
                CustomerId = customerId,
                Number = "4111111111111111",
                ExpirationDate = "05/2099"
            };

            Result<CreditCard> creditCardResult = gateway.CreditCard.Create(request);
            Assert.IsTrue(creditCardResult.IsSuccess());

            var clientToken = TestHelper.GenerateDecodedClientToken(gateway,
                new ClientTokenRequest
                {
                    CustomerId = customerId,
                    Options = new ClientTokenOptionsRequest
                    {
                        FailOnDuplicatePaymentMethod = true
                    }
                }
            );
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier_type", "testing").
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("credit_card[number]", "4111111111111111").
                AddTopLevelElement("credit_card[expiration_month]", "11").
                AddTopLevelElement("credit_card[expiration_year]", "2099");

            var response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/credit_cards.json", builder.ToQueryString());
            response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/credit_cards.json", builder.ToQueryString());

            Assert.AreEqual(422, (int)response.StatusCode);

            Customer customer = gateway.Customer.Find(customerId);
            Assert.AreEqual(1, customer.CreditCards.Length);
        }

        [Test]
        [Category("Integration")]
        public void Generate_GatewayRespectsMakeDefault()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());
            string customerId = result.Target.Id;

            var request = new CreditCardRequest
            {
                CustomerId = customerId,
                Number = "5105105105105100",
                ExpirationDate = "05/2099"
            };
            Result<CreditCard> creditCardResult = gateway.CreditCard.Create(request);
            Assert.IsTrue(creditCardResult.IsSuccess());

            var clientToken = TestHelper.GenerateDecodedClientToken(gateway,
                new ClientTokenRequest
                {
                    CustomerId = customerId,
                    Options = new ClientTokenOptionsRequest
                    {
                        MakeDefault = true
                    }
                }
            );
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier_type", "testing").
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("credit_card[number]", "4111111111111111").
                AddTopLevelElement("credit_card[expiration_month]", "11").
                AddTopLevelElement("credit_card[expiration_year]", "2099");

            var response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/credit_cards.json", builder.ToQueryString());
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            Customer customer = gateway.Customer.Find(customerId);
            Assert.AreEqual(2, customer.CreditCards.Length);
            foreach (CreditCard creditCard in customer.CreditCards)
            {
                if (creditCard.LastFour == "1111") {
                    Assert.IsTrue(creditCard.IsDefault.Value);
                }
            }
        }

        [Test]
        [Category("Integration")]
        public void Generate_ThrowExceptionWhenCustomerNotFound()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };

            string encodedClientToken = "";
            Exception exception = null;
            try
            {
                encodedClientToken += gateway.ClientToken.generate(
                    new ClientTokenRequest
                    {
                        CustomerId = "NON_EXISTENT_CUSTOMER_ID"
                    }
                );
            } catch (Exception tempException) {
                exception = tempException;
            }
            Assert.IsNotNull(exception);
            Assert.IsTrue(Regex.Match(exception.Message, @"customer_id").Success);
            Assert.IsInstanceOf<ArgumentException>(exception);
        } 
    }
}
