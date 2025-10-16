using System;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;

namespace Braintree.Tests
{
    /// <summary>
    /// Tests for BankAccountInstantVerificationGateway and BankAccountInstantVerificationJwtRequest classes.
    /// This class combines tests for both the gateway functionality and the request serialization.
    /// </summary>
    [TestFixture]
    public class BankAccountInstantVerificationGatewayTest
    {
        private Mock<BraintreeService> mockService;
        private Mock<IGraphQLClient> mockGraphQLClient;
        private BankAccountInstantVerificationGateway gateway;

        [SetUp]
        public void SetUp()
        {
            var mockConfiguration = new Configuration(Environment.SANDBOX, "test_merchant_id", "test_public_key", "test_private_key");
            mockService = new Mock<BraintreeService>(mockConfiguration);
            mockGraphQLClient = new Mock<IGraphQLClient>();
            gateway = new BankAccountInstantVerificationGateway(mockService.Object, mockGraphQLClient.Object);
        }

        [Test]
        public void CreateJwtSuccess()
        {
            // Arrange
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "Test Business",
                ReturnUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel"
            };

            var mockResponse = CreateSuccessfulJwtResponse();
            mockGraphQLClient.Setup(client => client.Query(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                           .Returns(mockResponse);

            // Act
            var result = gateway.CreateJwt(request);

            // Assert
            Assert.IsTrue(result.IsSuccess());
            Assert.IsNotNull(result.Target);
            Assert.AreEqual("test-jwt-token", result.Target.Jwt);
        }

        [Test]
        public void CreateJwtWithValidationErrors()
        {
            // Arrange
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "",
                ReturnUrl = "invalid-url"
            };

            var mockResponse = CreateErrorResponse();
            mockGraphQLClient.Setup(client => client.Query(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                           .Returns(mockResponse);

            // Act
            var result = gateway.CreateJwt(request);

            // Assert
            Assert.IsFalse(result.IsSuccess());
            Assert.IsNotNull(result.Errors);
        }

        [Test]
        public void CreateJwtGraphQLQueryUsesCorrectMutation()
        {
            // Arrange
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "Test Business",
                ReturnUrl = "https://example.com/success"
            };

            var mockResponse = CreateSuccessfulJwtResponse();
            mockGraphQLClient.Setup(client => client.Query(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                           .Returns(mockResponse);

            // Act
            gateway.CreateJwt(request);

            // Assert
            mockGraphQLClient.Verify(client => client.Query(
                It.Is<string>(query => 
                    query.Contains("mutation CreateBankAccountInstantVerificationJwt") &&
                    query.Contains("createBankAccountInstantVerificationJwt(input: $input)")
                ),
                It.IsAny<Dictionary<string, object>>()
            ), Times.Once);
        }

        [Test]
        public void CreateJwtMinimalRequest()
        {
            // Arrange
            var request = new BankAccountInstantVerificationJwtRequest
            {
                BusinessName = "Test Business",
                ReturnUrl = "https://example.com/success"
            };

            var mockResponse = CreateSuccessfulJwtResponse();
            mockGraphQLClient.Setup(client => client.Query(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
                           .Returns(mockResponse);

            // Act
            var result = gateway.CreateJwt(request);

            // Assert
            Assert.IsTrue(result.IsSuccess());
            Assert.AreEqual("test-jwt-token", result.Target.Jwt);
        }

        private GraphQLResponse CreateSuccessfulJwtResponse()
        {
            var jwtData = new Dictionary<string, object>
            {
                ["jwt"] = "test-jwt-token"
            };

            var data = new Dictionary<string, object>
            {
                ["createBankAccountInstantVerificationJwt"] = jwtData
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