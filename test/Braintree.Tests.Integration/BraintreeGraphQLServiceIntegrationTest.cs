using Braintree.Exceptions;
using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Braintree.Tests.Integration
{
    public class GraphQLPingResponse
    {
        public string ping { get; }
    }

    [TestFixture]
    public class BraintreeGraphQLServiceIntegrationTest
    {

        public BraintreeGraphQLService GetService()
        {
            Configuration configuration = new Configuration(
                Environment.DEVELOPMENT,
                "integration_merchant_id",
                "integration_public_key",
                "integration_private_key"
            );
            return new BraintreeGraphQLService(configuration);
        }

        [Test]
        public void SmokeTestQueryGraphQLWithoutVariables_isSuccessful()
        {
            BraintreeGraphQLService service = GetService();

            var result = service.QueryGraphQL("query {\n    ping\n}", null);

            Assert.AreEqual(result.data["ping"], "pong");
        }

        [Test]
        public void SmokeTestQueryGraphQLWithoutVariablesInSandbox_canMakeRequestsWithoutSSLErrors()
        {
            Configuration configuration = new Configuration(
                Environment.SANDBOX,
                "merchant_id",
                "public_key",
                "private_key"
            );
            BraintreeGraphQLService service = new BraintreeGraphQLService(configuration);

            // we get an expected authentication error here
            // the important thing is that we don't get an ssl error
            Assert.Throws<AuthenticationException>(() => service.QueryGraphQL("query {\n    ping\n}", null));
        }

        [Test]
        public void SmokeTestQueryGraphQLWithoutVariablesInProduction_canMakeRequestsWithoutSSLErrors()
        {
            Configuration configuration = new Configuration(
                Environment.PRODUCTION,
                "merchant_id",
                "public_key",
                "private_key"
            );
            BraintreeGraphQLService service = new BraintreeGraphQLService(configuration);

            // we get an expected authentication error here
            // the important thing is that we don't get an ssl error
            Assert.Throws<AuthenticationException>(() => service.QueryGraphQL("query {\n    ping\n}", null));
        }

        [Test]
        public void SmokeTestQueryGraphQLWithVariables_isSuccessful()
        {
            BraintreeGraphQLService service = GetService();

            var definition = @"
mutation CreateClientToken($input: CreateClientTokenInput!) {
    createClientToken(input: $input) {
        clientMutationId
        clientToken
    }
}";
            var variables = new Dictionary<string, object> {
                {"input", new Dictionary<string, object> {
                    {"clientMutationId", "abc123"},
                    {"clientToken", new Dictionary<string, object> {
                        {"merchantAccountId", "ABC123"}
                    }}
                }}
            };

            var result = service.QueryGraphQL(definition, variables);

			Assert.IsNull(result.errors);
            var createClientToken = (Dictionary<string, object>) result.data["createClientToken"];
            var clientToken = createClientToken["clientToken"];
            Assert.IsInstanceOf<string>(clientToken);
        }
        [Test]
        public void SmokeTestQueryGraphQLWithVariables_isUnsuccessful_andReturnsParsableErrors()
        {
            BraintreeGraphQLService service = GetService();

            var definition = @"
query TransactionLevelFeeReport($date: Date!, $merchantAccountId: ID) {
    report {
        transactionLevelFees(date: $date, merchantAccountId: $merchantAccountId) {
            url
        }
    }
}";
            var variables = new Dictionary<string, object> {
                {"date", "2018-01-01"},
                {"merchantAccountId", "some_merchant"}
            };

            var result = service.QueryGraphQL(definition, variables);

            Assert.AreEqual(result.errors.Count, 1);
            Assert.AreEqual(result.errors[0].message, "Invalid merchant account id: some_merchant");
        }

    }
}
