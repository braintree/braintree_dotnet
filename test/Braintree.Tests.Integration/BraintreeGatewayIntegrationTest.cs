using Braintree.Exceptions;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class BraintreeGatewayIntegrationTest
    {

        public BraintreeGateway GetGateway()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );
            return new BraintreeGateway(configuration);
        }

        [Test]
        public void TokenizeRawCreditCardDetailsWithGraphQL_isSuccessful()
        {
            BraintreeGateway gateway = GetGateway();

            var query = @"
mutation ExampleServerSideSingleUseToken($input: TokenizeCreditCardInput!) {
  tokenizeCreditCard(input: $input) {
    paymentMethod {
      id
      usage
      details {
        ... on CreditCardDetails {
          bin
          brandCode
          last4
          expirationYear
          expirationMonth
        }
      }
    }
  }
}";
            var variables = new Dictionary<string, object> {
                {"input", new Dictionary<string, object> {
                   {"creditCard", new Dictionary<string, object> {
                       {"number", "4005519200000004"},
                       {"expirationYear", "2024"},
                       {"expirationMonth", "05"},
                       {"cardholderName", "Joe Bloggs"},
                   }}
                }}
            };

            var result = gateway.GraphQLClient.Query(query, variables);
            var tokenizeCreditCard = (Dictionary<string, object>) result.data["tokenizeCreditCard"];
            var paymentMethod = (Dictionary<string, object>) tokenizeCreditCard["paymentMethod"];
            var details = (Dictionary<string, object>) paymentMethod["details"];

            Assert.IsInstanceOf<string>(paymentMethod["id"]);
            Assert.AreEqual(details["bin"], "400551");
            Assert.AreEqual(details["last4"], "0004");
            Assert.AreEqual(details["brandCode"], "VISA");
            Assert.AreEqual(details["expirationMonth"], "05");
            Assert.AreEqual(details["expirationYear"], "2024");
        }

        [Test]
#if netcore
        public async Task TokenizeRawCreditCardDetailsWithGraphQLAsync_isSuccessful()
#else
        public void TokenizeRawCreditCardDetailsWithGraphQLAsync_isSuccessful()
        {
            Task.Run(async() =>
#endif
        {
            BraintreeGateway gateway = GetGateway();

            var query = @"
mutation ExampleServerSideSingleUseToken($input: TokenizeCreditCardInput!) {
  tokenizeCreditCard(input: $input) {
    paymentMethod {
      id
      usage
      details {
        ... on CreditCardDetails {
          bin
          brandCode
          last4
          expirationYear
          expirationMonth
        }
      }
    }
  }
}";
            var variables = new Dictionary<string, object> {
                {"input", new Dictionary<string, object> {
                   {"creditCard", new Dictionary<string, object> {
                       {"number", "4005519200000004"},
                       {"expirationYear", "2024"},
                       {"expirationMonth", "05"},
                       {"cardholderName", "Joe Bloggs"},
                   }}
                }}
            };

            var result = await gateway.GraphQLClient.QueryAsync(query, variables);
            var tokenizeCreditCard = (Dictionary<string, object>) result.data["tokenizeCreditCard"];
            var paymentMethod = (Dictionary<string, object>) tokenizeCreditCard["paymentMethod"];
            var details = (Dictionary<string, object>) paymentMethod["details"];

            Assert.IsInstanceOf<string>(paymentMethod["id"]);
            Assert.AreEqual(details["bin"], "400551");
            Assert.AreEqual(details["last4"], "0004");
            Assert.AreEqual(details["brandCode"], "VISA");
            Assert.AreEqual(details["expirationMonth"], "05");
            Assert.AreEqual(details["expirationYear"], "2024");
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif

        [Test]
        public void QueryGraphQLWithoutVariables_isSuccessful()
        {
            BraintreeGateway gateway = GetGateway();

            var query = @"
query ExampleQuery {
    ping
}";

            var result = gateway.GraphQLClient.Query(query);

            Assert.AreEqual(result.data["ping"], "pong");
        }

        [Test]
#if netcore
        public async Task QueryGraphQLWithoutVariablesAsync_isSuccessful()
#else
        public void QueryGraphQLWithoutVariablesAsync_isSuccessful()
        {
            Task.Run(async() =>
#endif
        {
            BraintreeGateway gateway = GetGateway();

            var query = @"
query ExampleQuery {
    ping
}";

            var result = await gateway.GraphQLClient.QueryAsync(query);

            Assert.AreEqual(result.data["ping"], "pong");
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif
    }
}
