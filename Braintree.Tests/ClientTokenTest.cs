using System;
using System.Net;
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
        public void Generate_RaisesExceptionIfVerifyCardIsIncludedWithoutCustomerId()
        {
          BraintreeGateway gateway = new BraintreeGateway
          {
              Environment = Environment.DEVELOPMENT,
              MerchantId = "integration_merchant_id",
              PublicKey = "integration_public_key",
              PrivateKey = "integration_private_key"
          };
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
              Assert.Fail("Should raise ArgumentException");
          } catch (ArgumentException e) {
              Match match = Regex.Match(e.Message, @"VerifyCard");
              Assert.IsTrue(match.Success);
          }
        }

        [Test]
        public void Generate_RaisesExceptionIfMakeDefaultIsIncludedWithoutCustomerId()
        {
          BraintreeGateway gateway = new BraintreeGateway
          {
              Environment = Environment.DEVELOPMENT,
              MerchantId = "integration_merchant_id",
              PublicKey = "integration_public_key",
              PrivateKey = "integration_private_key"
          };
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
              Assert.Fail("Should raise ArgumentException");
          } catch (ArgumentException e) {
              Match match = Regex.Match(e.Message, @"MakeDefault");
              Assert.IsTrue(match.Success);
          }
        }

        [Test]
        public void Generate_RaisesExceptionIfFailOnDuplicatePaymentMethodIsIncludedWithoutCustomerId()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
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
                Assert.Fail("Should raise ArgumentException");
            } catch (ArgumentException e) {
                Match match = Regex.Match(e.Message, @"FailOnDuplicatePaymentMethod");
                Assert.IsTrue(match.Success);
            }
        }
    }

    [TestFixture]
    public class ClientTokenTestIT
    {
        [Test]
        public void Generate_GeneratedFingerprintIsAcceptedByGateway()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            var clientToken = gateway.ClientToken.generate();
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            var encodedFingerprint = HttpUtility.UrlEncode(authorizationFingerprint, Encoding.UTF8);
            var url = "nonces.json";
            url += "?authorizationFingerprint=" + encodedFingerprint;
            url += "&sharedCustomerIdentifierType=testing";
            url += "&sharedCustomerIdentifier=test-identifier";

            HttpWebResponse Response = new BraintreeTestHttpService().Get(gateway.MerchantId, url);
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
        }

        [Test]
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
            var clientToken = gateway.ClientToken.generate(
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

            HttpWebResponse Response = new BraintreeTestHttpService().Post(gateway.MerchantId, "nonces.json", builder.ToQueryString());
            Assert.AreEqual(422, (int)Response.StatusCode);

            Customer customer = gateway.Customer.Find(customerId);
            Assert.AreEqual(0, customer.CreditCards.Length);
        }

        [Test]
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

            var clientToken = gateway.ClientToken.generate(
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

            HttpWebResponse Response = new BraintreeTestHttpService().Post(gateway.MerchantId, "nonces.json", builder.ToQueryString());
            Response = new BraintreeTestHttpService().Post(gateway.MerchantId, "nonces.json", builder.ToQueryString());

            Assert.AreEqual(422, (int)Response.StatusCode);

            Customer customer = gateway.Customer.Find(customerId);
            Assert.AreEqual(1, customer.CreditCards.Length);
        }

        [Test]
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

            var clientToken = gateway.ClientToken.generate(
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

            HttpWebResponse Response = new BraintreeTestHttpService().Post(gateway.MerchantId, "nonces.json", builder.ToQueryString());
            Assert.AreEqual(HttpStatusCode.Created, Response.StatusCode);

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
        public void Generate_GatewayAcceptsMerchantAccountId()
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
                    MerchantAccountId = "my_merchant_account"
                }
            );
            var merchantAccountId = TestHelper.extractParamFromJson("merchantAccountId", clientToken);

            Assert.AreEqual(merchantAccountId, "my_merchant_account");
        }
    }
}
