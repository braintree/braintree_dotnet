using System;
using System.Net;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Braintree.Tests
{
    //NOTE: good
    [TestFixture]
    public class ClientTokenTest
    {
        [Test]
        public void Generate_DefaultToken()
        {
            var gateway = new BraintreeGateway();
            Assert.IsNotNull(gateway.ClientToken);
            string token = gateway.ClientToken.Generate();
            Assert.IsNotEmpty(token);
        }

        [Test]
        public void Generate_RaisesExceptionIfVerifyCardIsIncludedWithoutCustomerId()
        {
            var gateway = new BraintreeGateway();
            try {
                gateway.ClientToken.Generate(
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
            var gateway = new BraintreeGateway();
            try {
                gateway.ClientToken.Generate(
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
            var gateway = new BraintreeGateway();
            try {
                gateway.ClientToken.Generate(
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
            var gateway = new BraintreeGateway();
            var clientToken = TestHelper.GenerateDecodedClientToken(gateway);
            var authorizationFingerprint = TestHelper.extractParamFromJson("authorizationFingerprint", clientToken);

            var encodedFingerprint = HttpUtility.UrlEncode(authorizationFingerprint, Encoding.UTF8);
            var url = "v1/payment_methods.json";
            url += "?authorizationFingerprint=" + encodedFingerprint;
            url += "&sharedCustomerIdentifierType=testing";
            url += "&sharedCustomerIdentifier=test-identifier";

            HttpWebResponse response = new BraintreeTestHttpService().Get(gateway.MerchantId, url);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            response.Close();
        }

        [Test]
        public void Generate_SupportsVersionOption()
        {
            var gateway = new BraintreeGateway();
            var clientToken = gateway.ClientToken.Generate(
                new ClientTokenRequest
                {
                    Version = 1
                }
            );
            int version = TestHelper.extractIntParamFromJson("version", clientToken);
            Assert.AreEqual(1, version);
        }

        [Test]
        public void Generate_DefaultsToVersionTwo()
        {
            var gateway = new BraintreeGateway();
            var encodedClientToken = gateway.ClientToken.Generate();
            var decodedClientToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedClientToken));
            int version = TestHelper.extractIntParamFromJson("version", decodedClientToken);
            Assert.AreEqual(2, version);
        }

        [Test]
        public void Generate_GatewayRespectsVerifyCard()
        {
            var gateway = new BraintreeGateway();
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

            var builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier_type", "testing").
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("credit_card[number]", "4000111111111115").
                AddTopLevelElement("credit_card[expiration_month]", "11").
                AddTopLevelElement("credit_card[expiration_year]", "2099");

            HttpWebResponse response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/credit_cards.json", builder.ToQueryString());
            Assert.AreEqual(422, (int)response.StatusCode);
            response.Close();

            Customer customer = gateway.Customer.Find(customerId);
            Assert.AreEqual(0, customer.CreditCards.Length);
        }

        [Test]
        public void Generate_GatewayRespectsFailOnDuplicatePaymentMethod()
        {
            var gateway = new BraintreeGateway();
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

            var builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier_type", "testing").
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("credit_card[number]", "4111111111111111").
                AddTopLevelElement("credit_card[expiration_month]", "11").
                AddTopLevelElement("credit_card[expiration_year]", "2099");

            HttpWebResponse response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/credit_cards.json", builder.ToQueryString());
            response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/credit_cards.json", builder.ToQueryString());

            Assert.AreEqual(422, (int)response.StatusCode);
            response.Close();

            Customer customer = gateway.Customer.Find(customerId);
            Assert.AreEqual(1, customer.CreditCards.Length);
        }

        [Test]
        public void Generate_GatewayRespectsMakeDefault()
        {
            var gateway = new BraintreeGateway();
            Result<Customer> result = gateway.Customer.Create(new CustomerRequest());
            Assert.IsTrue(result.IsSuccess());
            string customerId = result.Target.Id;

            var request = new CreditCardRequest
            {
                CustomerId = customerId,
                Number = "5555555555554444",
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

            var builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier_type", "testing").
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("credit_card[number]", "4111111111111111").
                AddTopLevelElement("credit_card[expiration_month]", "11").
                AddTopLevelElement("credit_card[expiration_year]", "2099");

            HttpWebResponse response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/credit_cards.json", builder.ToQueryString());
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            response.Close();

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
        public void Generate_ThrowExceptionWhenCustomerNotFound()
        {
            var gateway = new BraintreeGateway();
            string encodedClientToken = "";
            try
            {
                encodedClientToken += gateway.ClientToken.Generate(
                    new ClientTokenRequest
                    {
                        CustomerId = "NON_EXISTENT_CUSTOMER_ID"
                    }
                );
                Assert.Fail("Should raise ArgumentException");
            } catch (ArgumentException e) {
                Match match = Regex.Match(e.Message, @"customer_id");
                Assert.IsTrue(match.Success);
            }
        }
    }
}
