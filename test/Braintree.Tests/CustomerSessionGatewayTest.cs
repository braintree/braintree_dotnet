using Braintree;
using Braintree.Exceptions;
using Braintree.GraphQL;
using Braintree.TestUtil;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Braintree.Tests
{

    public class CustomerSessionGatewayTest
    {

        [TestFixture]
        public class CreateCustomerSessionGatewayTest
        {

            private readonly Mock<IGraphQLClient> mockGraphQLClient = new Mock<IGraphQLClient>(); // Use interface if available

            private CustomerSessionGateway customerSessionGateway;

            [SetUp]
            public void Setup()
            {
                customerSessionGateway = new CustomerSessionGateway(mockGraphQLClient.Object);
                mockGraphQLClient.ResetCalls();
            }

            [Test]
            public void CreateCustomerSession_Invokes_GraphQLClient()
            {
                var successResponse = TestHelper.ReadGraphQLResponse("CustomerSession/create_session_successful_response.json");

                var parameters = new List<Dictionary<string, object>>();
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    Capture.In(parameters)
                   )
                ).Returns(successResponse);

                var createCustomerSessionInput = CreateCustomerSessionInput
                    .Builder()
                    .Build();

                customerSessionGateway.CreateCustomerSession(createCustomerSessionInput);

                mockGraphQLClient.Verify(x => x.Query(
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, object>>()
                    ),
                    Times.Once);
                Assert.AreEqual(createCustomerSessionInput.ToGraphQLVariables(), parameters[0]["input"]);
            }

            [Test]
            public void CreateCustomerSession_OnSuccess()
            {
                var successResponse = TestHelper.ReadGraphQLResponse("CustomerSession/create_session_successful_response.json");
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()
                   )
                ).Returns(successResponse);

                var createCustomerSessionInput = CreateCustomerSessionInput
                    .Builder()
                    .Build();

                var result = customerSessionGateway.CreateCustomerSession(createCustomerSessionInput);

                Assert.AreEqual("customer-session-id", result.Target);
            }

            [Test]
            public void CreateCustomerSession_OnValidationErrors()
            {
                var errorResponse = TestHelper.ReadGraphQLResponse("CustomerSession/create_session_with_errors.json");
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()
                   )
                ).Returns(errorResponse);

                var createCustomerSessionInput = CreateCustomerSessionInput
                    .Builder()
                    .Build();

                var result = customerSessionGateway.CreateCustomerSession(createCustomerSessionInput);

                Assert.AreEqual("validation error", result.Errors.All()[0].Message);
            }

            [Test]
            public void CreateCustomerSession_OnParsingError()
            {
                var errorResponse = TestHelper.ReadGraphQLResponse("CustomerSession/create_session_unparseable_response.json");
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()
                   )
                ).Returns(errorResponse);

                var createCustomerSessionInput = CreateCustomerSessionInput
                    .Builder()
                    .Build();

                Assert.Throws<UnexpectedException>(() => customerSessionGateway.CreateCustomerSession(createCustomerSessionInput));

            }
        }

        [TestFixture]
        public class UpdateCustomerSessionGatewayTest
        {
            private readonly Mock<IGraphQLClient> mockGraphQLClient = new Mock<IGraphQLClient>(); // Use interface if available

            private CustomerSessionGateway customerSessionGateway;

            [SetUp]
            public void Setup()
            {
                customerSessionGateway = new CustomerSessionGateway(mockGraphQLClient.Object);
                mockGraphQLClient.ResetCalls();
            }

            [Test]
            public void UpdateCustomerSession_Invokes_GraphQLClient()
            {
                var successResponse = TestHelper.ReadGraphQLResponse("CustomerSession/update_session_successful_response.json");

                var parameters = new List<Dictionary<string, object>>();
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    Capture.In(parameters)
                   )
                ).Returns(successResponse);

                var updateCustomerSessionInput = UpdateCustomerSessionInput
                    .Builder("session-id")
                    .Build();

                customerSessionGateway.UpdateCustomerSession(updateCustomerSessionInput);

                mockGraphQLClient.Verify(x => x.Query(
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, object>>()
                    ),
                    Times.Once);
                Assert.AreEqual(updateCustomerSessionInput.ToGraphQLVariables(), parameters[0]["input"]);
            }

            [Test]
            public void UpdateCustomerSession_OnSuccess()
            {
                var successResponse = TestHelper.ReadGraphQLResponse("CustomerSession/update_session_successful_response.json");
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()
                   )
                ).Returns(successResponse);

                var updateCustomerSessionInput = UpdateCustomerSessionInput
                    .Builder("customer-session-id")
                    .Build();

                var result = customerSessionGateway.UpdateCustomerSession(updateCustomerSessionInput);

                Assert.AreEqual("customer-session-id", result.Target);
            }

            [Test]
            public void updateCustomerSession_OnValidationErrors()
            {
                var errorResponse = TestHelper.ReadGraphQLResponse("CustomerSession/update_session_with_errors.json");
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()
                   )
                ).Returns(errorResponse);

                var updateCustomerSessionInput = UpdateCustomerSessionInput
                    .Builder("customer-session-id")
                    .Build();

                var result = customerSessionGateway.UpdateCustomerSession(updateCustomerSessionInput);

                Assert.AreEqual("validation error", result.Errors.All()[0].Message);
            }

            [Test]
            public void UpdateCustomerSession_OnParsingError()
            {
                var errorResponse = TestHelper.ReadGraphQLResponse("CustomerSession/update_session_unparseable_response.json");
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()
                   )
                ).Returns(errorResponse);

                var updateCustomerSessionInput = UpdateCustomerSessionInput
                    .Builder("customer-session-id")
                    .Build();

                Assert.Throws<UnexpectedException>(() => customerSessionGateway.UpdateCustomerSession(updateCustomerSessionInput));

            }
        }

        [TestFixture]
        public class GetCustomerRecommendationsGatewayTest
        {
            private readonly Mock<IGraphQLClient> mockGraphQLClient = new Mock<IGraphQLClient>(); // Use interface if available

            private CustomerSessionGateway customerSessionGateway;

            [SetUp]
            public void Setup()
            {
                customerSessionGateway = new CustomerSessionGateway(mockGraphQLClient.Object);
                mockGraphQLClient.ResetCalls();
            }

            [Test]
            public void GetCustomerRecommendations_Invokes_GraphQLClient()
            {
                var successResponse = TestHelper.ReadGraphQLResponse("CustomerSession/customer_recommendations_successful_response.json", true);

                var parameters = new List<Dictionary<string, object>>();
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    Capture.In(parameters)
                   )
                ).Returns(successResponse);

                var recommendations = new List<Recommendations> { Recommendations.PAYMENT_RECOMMENDATIONS };
                var customerRecommendationsInput = CustomerRecommendationsInput
                    .Builder("session-id", recommendations)
                    .Build();

                customerSessionGateway.GetCustomerRecommendations(customerRecommendationsInput);

                mockGraphQLClient.Verify(x => x.Query(
                        It.IsAny<string>(),
                        It.IsAny<Dictionary<string, object>>()
                    ),
                    Times.Once);
                Assert.AreEqual(customerRecommendationsInput.ToGraphQLVariables(), parameters[0]["input"]);
            }

            [Test]
            public void GetCustomerRecommendations_OnSuccess()
            {
                var successResponse = TestHelper.ReadGraphQLResponse("CustomerSession/customer_recommendations_successful_response.json", true);
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()
                   )
                ).Returns(successResponse);

                var recommendations = new List<Recommendations> { Recommendations.PAYMENT_RECOMMENDATIONS };
                var customerRecommendationsInput = CustomerRecommendationsInput
                    .Builder("session-id", recommendations)
                    .Build();

                var result = customerSessionGateway.GetCustomerRecommendations(customerRecommendationsInput);
                var actualPayload = result.Target;

                Assert.AreEqual(RecommendedPaymentOption.PAYPAL, actualPayload.Recommendations.PaymentOptions[0].PaymentOption);
            }

            [Test]
            public void GetCustomerRecommendations_OnValidationErrors()
            {
                var errorResponse = TestHelper.ReadGraphQLResponse("CustomerSession/customer_recommendations_with_errors.json");
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()
                   )
                ).Returns(errorResponse);

                var recommendations = new List<Recommendations> { Recommendations.PAYMENT_RECOMMENDATIONS };
                var customerRecommendationsInput = CustomerRecommendationsInput
                    .Builder("session-id", recommendations)
                    .Build();

                var result = customerSessionGateway.GetCustomerRecommendations(customerRecommendationsInput);

                Assert.AreEqual("validation error", result.Errors.All()[0].Message);
            }

            [Test]
            public void GetCustomerRecommendations_OnParsingError()
            {
                var errorResponse = TestHelper.ReadGraphQLResponse("CustomerSession/customer_recommendations_unparseable_response.json");
                mockGraphQLClient.Setup(x => x.Query(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, object>>()
                   )
                ).Returns(errorResponse);

                var recommendations = new List<Recommendations> { Recommendations.PAYMENT_RECOMMENDATIONS };
                var customerRecommendationsInput = CustomerRecommendationsInput
                    .Builder("session-id", recommendations)
                    .Build();

                Assert.Throws<UnexpectedException>(() => customerSessionGateway.GetCustomerRecommendations(customerRecommendationsInput));

            }
        }
    }
}
