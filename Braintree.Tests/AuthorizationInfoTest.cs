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
    public class AuthorizationInfoTest
    {
        [Test]
        public void Generate_RaisesExceptionIfVerifyCardIsIncludedWithoutCustomerId()
        {
          var authorizationInfo = new AuthorizationInfo
          {
              MerchantId = "my-merchant-id",
              PublicKey = "my-public-key",
              PrivateKey = "my-private-key",
              Options = new AuthorizationInfoOptions{ VerifyCard = true }
          };
          try {
              authorizationInfo.generate();
              Assert.Fail("Should raise ArgumentException");
          } catch (ArgumentException e) {
              Match match = Regex.Match(e.Message, @"VerifyCard");
              Assert.IsTrue(match.Success);
          }

        }

        [Test]
        public void Generate_RaisesExceptionIfMakeDefaultIsIncludedWithoutCustomerId()
        {
          var authorizationInfo = new AuthorizationInfo
          {
              MerchantId = "my-merchant-id",
              PublicKey = "my-public-key",
              PrivateKey = "my-private-key",
              Options = new AuthorizationInfoOptions{ MakeDefault = true }
          };
          try {
              authorizationInfo.generate();
              Assert.Fail("Should raise ArgumentException");
          } catch (ArgumentException e) {
              Match match = Regex.Match(e.Message, @"MakeDefault");
              Assert.IsTrue(match.Success);
          }

        }

        [Test]
        public void Generate_RaisesExceptionIfFailOnDuplicatePaymentMethodIsIncludedWithoutCustomerId()
        {
          var authorizationInfo = new AuthorizationInfo
          {
              MerchantId = "my-merchant-id",
              PublicKey = "my-public-key",
              PrivateKey = "my-private-key",
              Options = new AuthorizationInfoOptions{ FailOnDuplicatePaymentMethod = true }
          };
          try {
              authorizationInfo.generate();
              Assert.Fail("Should raise ArgumentException");
          } catch (ArgumentException e) {
              Match match = Regex.Match(e.Message, @"FailOnDuplicatePaymentMethod");
              Assert.IsTrue(match.Success);
          }

        }

        [Test]
        public void Generate_IncludesCreatedAtPublicKeyClientApiUrl()
        {
            var authorizationInfo = new AuthorizationInfo
            {
                MerchantId = "my-merchant-id",
                PublicKey = "my-public-key",
                PrivateKey = "my-private-key",
                ClientApiUrl = "http://client.api.url",
                AuthUrl = "http://auth.url"
            }.generate();
            var fingerprint = TestHelper.extractParamFromJson("fingerprint", authorizationInfo);

            string[] fingerprintArray = fingerprint.Split('|');
            var signature = fingerprintArray[0];
            var payload = fingerprintArray[1];

            Assert.IsTrue(signature.Length > 1);

            var regex = new Regex(@"created_at=\d+");
            Assert.IsTrue(regex.IsMatch(payload));
            Assert.IsTrue(payload.Contains("public_key=my-public-key"));

            var clientApiUrl = TestHelper.extractParamFromJson("client_api_url", authorizationInfo);
            Assert.AreEqual("http://client.api.url", clientApiUrl);

            var authUrl = TestHelper.extractParamFromJson("auth_url", authorizationInfo);
            Assert.AreEqual("http://auth.url", authUrl);
        }

        [Test]
        public void Generate_CanIncludeCustomerId()
        {

            var authorizationInfo = new AuthorizationInfo
            {
                MerchantId = "integration_merchant_id",
                PublicKey = "my-public-key",
                PrivateKey = "my-private-key",
                Options = new AuthorizationInfoOptions{ CustomerId = "my-customer-id" }
            }.generate();
            var fingerprint = TestHelper.extractParamFromJson("fingerprint", authorizationInfo);
            string[] fingerprintArray = fingerprint.Split('|');
            var payload = fingerprintArray[1];

            Assert.IsTrue(payload.Contains("customer_id=my-customer-id"));
        }

        [Test]
        public void Generate_CanIncludeCreditCardOptions()
        {
            var authorizationInfo = new AuthorizationInfo
            {
                MerchantId = "integration_merchant_id",
                PublicKey = "my-public-key",
                PrivateKey = "my-private-key",
                Options = new AuthorizationInfoOptions {
                  CustomerId = "my-customer-id",
                  VerifyCard = true,
                  MakeDefault = true,
                  FailOnDuplicatePaymentMethod = true
                }
            }.generate();
            var fingerprint = TestHelper.extractParamFromJson("fingerprint", authorizationInfo);
            string[] fingerprintArray = fingerprint.Split('|');
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
            var authorizationInfo = gateway.GenerateAuthorizationInfo();
            var fingerprint = TestHelper.extractParamFromJson("fingerprint", authorizationInfo);

            string[] fingerprintArray = fingerprint.Split('|');
            var payload = fingerprintArray[1];

            Assert.IsTrue(payload.Contains("public_key=integration_public_key"));

            var expectedClientApiUrl = new BraintreeService(gateway.Configuration).BaseMerchantURL() + "/client_api";
            var clientApiUrl = TestHelper.extractParamFromJson("client_api_url", authorizationInfo);
            Assert.AreEqual(expectedClientApiUrl, clientApiUrl);

            var authUrl = TestHelper.extractParamFromJson("auth_url", authorizationInfo);
            Assert.AreEqual("http://auth.venmo.dev:4567", authUrl);

            var regex = new Regex(@"created_at=\d+");
            Assert.IsTrue(regex.IsMatch(payload));
        }
    }

    [TestFixture]
    public class AuthorizationInfoTestIT
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
            var authorizationInfo = gateway.GenerateAuthorizationInfo();
            var fingerprint = TestHelper.extractParamFromJson("fingerprint", authorizationInfo);

            var encodedFingerprint = HttpUtility.UrlEncode(fingerprint, Encoding.UTF8);
            var url = "credit_cards.json";
            url += "?authorizationFingerprint=" + encodedFingerprint;
            url += "&sessionIdentifierType=testing";
            url += "&sessionIdentifier=test-identifier";

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
            var authorizationInfo = gateway.GenerateAuthorizationInfo(new AuthorizationInfoOptions {
              CustomerId = customerId,
              VerifyCard = true
            });
            var fingerprint = TestHelper.extractParamFromJson("fingerprint", authorizationInfo);

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", fingerprint).
              AddTopLevelElement("session_identifier_type", "testing").
              AddTopLevelElement("session_identifier", "test-identifier").
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

            var authorizationInfo = gateway.GenerateAuthorizationInfo(new AuthorizationInfoOptions {
              CustomerId = customerId,
              FailOnDuplicatePaymentMethod = true
            });
            var fingerprint = TestHelper.extractParamFromJson("fingerprint", authorizationInfo);

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", fingerprint).
              AddTopLevelElement("session_identifier_type", "testing").
              AddTopLevelElement("session_identifier", "test-identifier").
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

            var authorizationInfo = gateway.GenerateAuthorizationInfo(new AuthorizationInfoOptions {
              CustomerId = customerId,
              MakeDefault = true
            });
            var fingerprint = TestHelper.extractParamFromJson("fingerprint", authorizationInfo);

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", fingerprint).
              AddTopLevelElement("session_identifier_type", "testing").
              AddTopLevelElement("session_identifier", "test-identifier").
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
