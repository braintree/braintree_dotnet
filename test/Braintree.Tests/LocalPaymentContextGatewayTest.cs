using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;
using Braintree.GraphQL;

namespace Braintree.Tests
{
    [TestFixture]
    public class LocalPaymentContextGatewayTest
    {
        private Mock<BraintreeService> mockService;
        private Mock<IGraphQLClient> mockGraphQLClient;
        private LocalPaymentContextGateway gateway;

        [SetUp]
        public void SetUp()
        {
            var mockConfiguration = new Configuration(Environment.SANDBOX, "test_merchant_id", "test_public_key", "test_private_key");
            mockService = new Mock<BraintreeService>(mockConfiguration);
            mockGraphQLClient = new Mock<IGraphQLClient>();
            gateway = new LocalPaymentContextGateway(mockService.Object, mockGraphQLClient.Object);
        }

        [Test]
        public void CreateSuccess()
        {
            var input = CreateLocalPaymentContextInput.Builder()
                .Amount(10.00m, "EUR")
                .Type(LocalPaymentType.MBWAY)
                .MerchantAccountId("test_merchant_account")
                .ReturnUrl("https://example.com/return")
                .CancelUrl("https://example.com/cancel")
                .PayerInfo(PayerInfoInput.Builder()
                    .Email("test@example.com")
                    .GivenName("John")
                    .Surname("Doe")
                    .Build())
                .Build();

            var mockResponse = CreateSuccessfulCreateResponse();
            mockGraphQLClient.Setup(client => client.Query(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                           .Returns(mockResponse);

            var result = gateway.Create(input);

            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target);
            Assert.AreEqual("test-payment-context-id", result.Target.Id);
            Assert.AreEqual("MBWAY", result.Target.Type);
            Assert.AreEqual("https://example.com/approve", result.Target.ApprovalUrl);
        }

        [Test]
        public void CreateWithValidationErrors()
        {
            var input = CreateLocalPaymentContextInput.Builder().Build();

            var mockResponse = CreateErrorResponse();
            mockGraphQLClient.Setup(client => client.Query(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                           .Returns(mockResponse);

            var result = gateway.Create(input);

            Assert.IsFalse(result.IsSuccess());
            Assert.IsNotNull(result.Errors);
        }

        [Test]
        public void CreateGraphQLQueryUsesCorrectMutation()
        {
            var input = CreateLocalPaymentContextInput.Builder()
                .Amount(10.00m, "EUR")
                .Type(LocalPaymentType.MBWAY)
                .ReturnUrl("https://example.com/return")
                .CancelUrl("https://example.com/cancel")
                .Build();

            var mockResponse = CreateSuccessfulCreateResponse();
            mockGraphQLClient.Setup(client => client.Query(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                           .Returns(mockResponse);

            gateway.Create(input);

            mockGraphQLClient.Verify(client => client.Query(
                It.Is<string>(query =>
                    query.Contains("mutation CreateLocalPaymentContext") &&
                    query.Contains("createLocalPaymentContext(input: $input)")
                ),
                It.IsAny<Dictionary<string, object>>()
            ), Times.Once);
        }

        [Test]
        public void FindSuccess()
        {
            var mockResponse = CreateSuccessfulFindResponse();
            mockGraphQLClient.Setup(client => client.Query(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                           .Returns(mockResponse);

            var result = gateway.Find("test-id");

            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target);
            Assert.AreEqual("test-payment-context-id", result.Target.Id);
            Assert.AreEqual("MBWAY", result.Target.Type);
        }

        [Test]
        public void FindNotFound()
        {
            var mockResponse = CreateNotFoundResponse();
            mockGraphQLClient.Setup(client => client.Query(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                           .Returns(mockResponse);

            Assert.Throws<Braintree.Exceptions.NotFoundException>(() => gateway.Find("nonexistent-id"));
        }

        [Test]
        public void FindGraphQLQueryUsesCorrectQuery()
        {
            var mockResponse = CreateSuccessfulFindResponse();
            mockGraphQLClient.Setup(client => client.Query(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                           .Returns(mockResponse);

            gateway.Find("test-id");

            mockGraphQLClient.Verify(client => client.Query(
                It.Is<string>(query =>
                    query.Contains("query Node($id: ID!)") &&
                    query.Contains("node(id: $id)")
                ),
                It.Is<Dictionary<string, object>>(vars => vars.ContainsKey("id") && (string)vars["id"] == "test-id")
            ), Times.Once);
        }

        private GraphQLResponse CreateSuccessfulCreateResponse()
        {
            var amount = new Dictionary<string, object>
            {
                ["value"] = "10.00",
                ["currencyCode"] = "EUR"
            };

            var paymentContext = new Dictionary<string, object>
            {
                ["id"] = "test-payment-context-id",
                ["type"] = "MBWAY",
                ["paymentId"] = "test-payment-id",
                ["approvalUrl"] = "https://example.com/approve",
                ["merchantAccountId"] = "test_merchant_account",
                ["amount"] = amount
            };

            var result = new Dictionary<string, object>
            {
                ["paymentContext"] = paymentContext
            };

            var data = new Dictionary<string, object>
            {
                ["createLocalPaymentContext"] = result
            };

            return new GraphQLResponse
            {
                data = data,
                errors = null
            };
        }

        private GraphQLResponse CreateSuccessfulFindResponse()
        {
            var amount = new Dictionary<string, object>
            {
                ["value"] = "10.00",
                ["currencyIsoCode"] = "EUR"
            };

            var nodeData = new Dictionary<string, object>
            {
                ["id"] = "test-payment-context-id",
                ["type"] = "MBWAY",
                ["paymentId"] = "test-payment-id",
                ["approvalUrl"] = "https://example.com/approve",
                ["merchantAccountId"] = "test_merchant_account",
                ["amount"] = amount
            };

            var data = new Dictionary<string, object>
            {
                ["node"] = nodeData
            };

            return new GraphQLResponse
            {
                data = data,
                errors = null
            };
        }

        private GraphQLResponse CreateNotFoundResponse()
        {
            var data = new Dictionary<string, object>
            {
                ["node"] = null
            };

            return new GraphQLResponse
            {
                data = data,
                errors = null
            };
        }

        private GraphQLResponse CreateErrorResponse()
        {
            var error = new GraphQLError
            {
                message = "Validation error",
                extensions = new Dictionary<string, object>()
            };

            return new GraphQLResponse
            {
                data = null,
                errors = new List<GraphQLError> { error }
            };
        }
    }
}
