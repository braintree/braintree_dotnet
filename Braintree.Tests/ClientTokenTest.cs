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
          var clientToken = new ClientToken
          {
              MerchantId = "my-merchant-id",
              PublicKey = "my-public-key",
              PrivateKey = "my-private-key",
              Options = new ClientTokenOptions{ VerifyCard = true }
          };
          try {
              clientToken.generate();
              Assert.Fail("Should raise ArgumentException");
          } catch (ArgumentException e) {
              Match match = Regex.Match(e.Message, @"VerifyCard");
              Assert.IsTrue(match.Success);
          }

        }

        [Test]
        public void Generate_RaisesExceptionIfMakeDefaultIsIncludedWithoutCustomerId()
        {
          var clientToken = new ClientToken
          {
              MerchantId = "my-merchant-id",
              PublicKey = "my-public-key",
              PrivateKey = "my-private-key",
              Options = new ClientTokenOptions{ MakeDefault = true }
          };
          try {
              clientToken.generate();
              Assert.Fail("Should raise ArgumentException");
          } catch (ArgumentException e) {
              Match match = Regex.Match(e.Message, @"MakeDefault");
              Assert.IsTrue(match.Success);
          }

        }

        [Test]
        public void Generate_RaisesExceptionIfFailOnDuplicatePaymentMethodIsIncludedWithoutCustomerId()
        {
          var clientToken = new ClientToken
          {
              MerchantId = "my-merchant-id",
              PublicKey = "my-public-key",
              PrivateKey = "my-private-key",
              Options = new ClientTokenOptions{ FailOnDuplicatePaymentMethod = true }
          };
          try {
              clientToken.generate();
              Assert.Fail("Should raise ArgumentException");
          } catch (ArgumentException e) {
              Match match = Regex.Match(e.Message, @"FailOnDuplicatePaymentMethod");
              Assert.IsTrue(match.Success);
          }

        }

        [Test]
        public void Generate_IncludesCreatedAtPublicKeyClientApiUrl()
        {
            var clientToken = new ClientToken
            {
                MerchantId = "my-merchant-id",
                PublicKey = "my-public-key",
                PrivateKey = "my-private-key",
                ClientApiUrl = "http://client.api.url",
                AuthUrl = "http://auth.url"
            }.generate();
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            string[] fingerprintArray = authorizationFingerprint.Split('|');
            var signature = fingerprintArray[0];
            var payload = fingerprintArray[1];

            Assert.IsTrue(signature.Length > 1);

            var regex = new Regex(@"created_at=\d+");
            Assert.IsTrue(regex.IsMatch(payload));
            Assert.IsTrue(payload.Contains("public_key=my-public-key"));

            var clientApiUrl = TestHelper.extractParamFromJson("clientApiUrl", clientToken);
            Assert.AreEqual("http://client.api.url", clientApiUrl);

            var authUrl = TestHelper.extractParamFromJson("authUrl", clientToken);
            Assert.AreEqual("http://auth.url", authUrl);
        }

        [Test]
        public void Generate_CanIncludeCustomerId()
        {

            var clientToken = new ClientToken
            {
                MerchantId = "integration_merchant_id",
                PublicKey = "my-public-key",
                PrivateKey = "my-private-key",
                Options = new ClientTokenOptions{ CustomerId = "my-customer-id" }
            }.generate();
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);
            string[] fingerprintArray = authorizationFingerprint.Split('|');
            var payload = fingerprintArray[1];

            Assert.IsTrue(payload.Contains("customer_id=my-customer-id"));
        }

        [Test]
        public void Generate_CanIncludeCreditCardOptions()
        {
            var clientToken = new ClientToken
            {
                MerchantId = "integration_merchant_id",
                PublicKey = "my-public-key",
                PrivateKey = "my-private-key",
                Options = new ClientTokenOptions {
                  CustomerId = "my-customer-id",
                  VerifyCard = true,
                  MakeDefault = true,
                  FailOnDuplicatePaymentMethod = true
                }
            }.generate();
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);
            string[] fingerprintArray = authorizationFingerprint.Split('|');
            var payload = fingerprintArray[1];

            Assert.IsTrue(payload.Contains("credit_card[options][make_default]=true"));
            Assert.IsTrue(payload.Contains("credit_card[options][fail_on_duplicate_payment_method]=true"));
            Assert.IsTrue(payload.Contains("credit_card[options][verify_card]=true"));
        }

        [Test]
        public void Generate_ThroughBraintreeGateway()
        {
            BraintreeGateway gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };
            var clientToken = gateway.GenerateClientToken();
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            string[] fingerprintArray = authorizationFingerprint.Split('|');
            var payload = fingerprintArray[1];

            Assert.IsTrue(payload.Contains("public_key=integration_public_key"));

            var expectedClientApiUrl = new BraintreeService(gateway.Configuration).BaseMerchantURL() + "/client_api";
            var clientApiUrl = TestHelper.extractParamFromJson("clientApiUrl", clientToken);
            Assert.AreEqual(expectedClientApiUrl, clientApiUrl);

            var authUrl = TestHelper.extractParamFromJson("authUrl", clientToken);
            Assert.AreEqual("http://auth.venmo.dev:9292", authUrl);

            var regex = new Regex(@"created_at=\d+");
            Assert.IsTrue(regex.IsMatch(payload));
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
            var clientToken = gateway.GenerateClientToken();
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            var encodedFingerprint = HttpUtility.UrlEncode(authorizationFingerprint, Encoding.UTF8);
            var url = "credit_cards.json";
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
            var clientToken = gateway.GenerateClientToken(new ClientTokenOptions {
              CustomerId = customerId,
              VerifyCard = true
            });
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
              AddTopLevelElement("shared_customer_identifier_type", "testing").
              AddTopLevelElement("shared_customer_identifier", "test-identifier").
              AddTopLevelElement("credit_card[number]", "4000111111111115").
              AddTopLevelElement("credit_card[expiration_month]", "11").
              AddTopLevelElement("credit_card[expiration_year]", "2099");

            HttpWebResponse Response = new BraintreeTestHttpService().Post(gateway.MerchantId, "credit_cards.json", builder.ToQueryString());
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

            var clientToken = gateway.GenerateClientToken(new ClientTokenOptions {
              CustomerId = customerId,
              FailOnDuplicatePaymentMethod = true
            });
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
              AddTopLevelElement("shared_customer_identifier_type", "testing").
              AddTopLevelElement("shared_customer_identifier", "test-identifier").
              AddTopLevelElement("credit_card[number]", "4111111111111111").
              AddTopLevelElement("credit_card[expiration_month]", "11").
              AddTopLevelElement("credit_card[expiration_year]", "2099");

            HttpWebResponse Response = new BraintreeTestHttpService().Post(gateway.MerchantId, "credit_cards.json", builder.ToQueryString());
            Response = new BraintreeTestHttpService().Post(gateway.MerchantId, "credit_cards.json", builder.ToQueryString());

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

            var clientToken = gateway.GenerateClientToken(new ClientTokenOptions {
              CustomerId = customerId,
              MakeDefault = true
            });
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
              AddTopLevelElement("shared_customer_identifier_type", "testing").
              AddTopLevelElement("shared_customer_identifier", "test-identifier").
              AddTopLevelElement("credit_card[number]", "4111111111111111").
              AddTopLevelElement("credit_card[expiration_month]", "11").
              AddTopLevelElement("credit_card[expiration_year]", "2099");

            HttpWebResponse Response = new BraintreeTestHttpService().Post(gateway.MerchantId, "credit_cards.json", builder.ToQueryString());
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
    }
}
