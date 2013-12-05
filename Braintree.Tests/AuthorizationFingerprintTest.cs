using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using NUnit.Framework;

namespace Braintree.Tests
{
    public class BraintreeTestHttpService
    {
        public String ApiVersion = "3";

        public HttpWebResponse Get(string URL)
        {
            return GetXmlResponse(URL, "GET", null);
        }

        public HttpWebResponse Post(string URL, String requestBody)
        {
            return GetXmlResponse(URL, "POST", requestBody);
        }

        private HttpWebResponse GetXmlResponse(String Path, String method, String requestBody)
        {
            try
            {
                var request = WebRequest.Create(Environment.DEVELOPMENT.GatewayURL + "/client_api/" + Path) as HttpWebRequest;
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("Accept-Encoding", "gzip");
                request.Accept = "application/json";
                request.UserAgent = "Braintree .NET " + typeof(BraintreeService).Assembly.GetName().Version.ToString();
                request.Method = method;
                request.KeepAlive = false;
                request.Timeout = 60000;
                request.ReadWriteTimeout = 60000;

                if (requestBody != null)
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(requestBody);
                    request.ContentLength = buffer.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                }

                var response = request.GetResponse() as HttpWebResponse;
                response.Close();
                return response;
            }
            catch (WebException e)
            {
                var response = (HttpWebResponse)e.Response;
                if (response == null) throw e;
                return response;
            }
        }

        private Stream GetResponseStream(HttpWebResponse response)
        {
            var stream = response.GetResponseStream();
            if (response.ContentEncoding.Equals("gzip", StringComparison.CurrentCultureIgnoreCase))
            {
                stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            return stream;
        }
    }

    [TestFixture]
    public class AuthorizationFingerprintTest
    {
        [Test]
        public void Generate_IncludesMerchantIdCreatedAtPublicKey()
        {
            var fingerprint = new AuthorizationFingerprint
            {
                MerchantId = "my-merchant-id",
                PublicKey = "my-public-key",
                PrivateKey = "my-private-key"
            }.generate();
            string[] fingerprintArray = fingerprint.Split('|');
            var signature = fingerprintArray[0];
            var payload = fingerprintArray[1];

            Assert.IsTrue(signature.Length > 1);

            Assert.IsTrue(payload.Contains("merchant_id=my-merchant-id"));
            Assert.IsTrue(payload.Contains("public_key=my-public-key"));

            var regex = new Regex(@"created_at=\d+");
            Assert.IsTrue(regex.IsMatch(payload));
        }

        [Test]
        public void Generate_CanIncludeCustomerId()
        {

            var fingerprint = new AuthorizationFingerprint
            {
                MerchantId = "integration_merchant_id",
                PublicKey = "my-public-key",
                PrivateKey = "my-private-key",
                Options = new AuthorizationFingerprintOptions{ CustomerId = "my-customer-id" }
            }.generate();
            string[] fingerprintArray = fingerprint.Split('|');
            var payload = fingerprintArray[1];

            Assert.IsTrue(payload.Contains("customer_id=my-customer-id"));
        }

        [Test]
        public void Generate_CanIncludeCreditCardOptions()
        {
            var fingerprint = new AuthorizationFingerprint
            {
                MerchantId = "integration_merchant_id",
                PublicKey = "my-public-key",
                PrivateKey = "my-private-key",
                Options = new AuthorizationFingerprintOptions {
                  VerifyCard = true,
                  MakeDefault = true,
                  FailOnDuplicatePaymentMethod = true
                }
            }.generate();
            string[] fingerprintArray = fingerprint.Split('|');
            var payload = fingerprintArray[1];

            Assert.IsTrue(payload.Contains("credit_card[options][make_default]=true"));
            Assert.IsTrue(payload.Contains("credit_card[options][fail_on_duplicate_payment_method]=true"));
            Assert.IsTrue(payload.Contains("credit_card[options][verify_card]=true"));
        }
    }

    [TestFixture]
    public class AuthorizationFingerprintTestIT
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
            var fingerprint = gateway.GenerateAuthorizationFingerprint();
            var encodedFingerprint = HttpUtility.UrlEncode(fingerprint, Encoding.UTF8);
            var url = "credit_cards.json";
            url += "?authorizationFingerprint=" + encodedFingerprint;
            url += "&sessionIdentifierType=testing";
            url += "&sessionIdentifier=test-identifier";

            HttpWebResponse Response = new BraintreeTestHttpService().Get(url);
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
            var fingerprint = gateway.GenerateAuthorizationFingerprint(new AuthorizationFingerprintOptions {
              CustomerId = customerId,
              VerifyCard = true
            });

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", fingerprint).
              AddTopLevelElement("session_identifier_type", "testing").
              AddTopLevelElement("session_identifier", "test-identifier").
              AddTopLevelElement("credit_card[number]", "4000111111111115").
              AddTopLevelElement("credit_card[expiration_month]", "11").
              AddTopLevelElement("credit_card[expiration_year]", "2099");

            HttpWebResponse Response = new BraintreeTestHttpService().Post("credit_cards.json", builder.ToQueryString());
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

            var fingerprint = gateway.GenerateAuthorizationFingerprint(new AuthorizationFingerprintOptions {
              CustomerId = customerId,
              FailOnDuplicatePaymentMethod = true
            });

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", fingerprint).
              AddTopLevelElement("session_identifier_type", "testing").
              AddTopLevelElement("session_identifier", "test-identifier").
              AddTopLevelElement("credit_card[number]", "4111111111111111").
              AddTopLevelElement("credit_card[expiration_month]", "11").
              AddTopLevelElement("credit_card[expiration_year]", "2099");

            HttpWebResponse Response = new BraintreeTestHttpService().Post("credit_cards.json", builder.ToQueryString());
            Response = new BraintreeTestHttpService().Post("credit_cards.json", builder.ToQueryString());

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

            var fingerprint = gateway.GenerateAuthorizationFingerprint(new AuthorizationFingerprintOptions {
              CustomerId = customerId,
              MakeDefault = true
            });

            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", fingerprint).
              AddTopLevelElement("session_identifier_type", "testing").
              AddTopLevelElement("session_identifier", "test-identifier").
              AddTopLevelElement("credit_card[number]", "4111111111111111").
              AddTopLevelElement("credit_card[expiration_month]", "11").
              AddTopLevelElement("credit_card[expiration_year]", "2099");

            HttpWebResponse Response = new BraintreeTestHttpService().Post("credit_cards.json", builder.ToQueryString());
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
