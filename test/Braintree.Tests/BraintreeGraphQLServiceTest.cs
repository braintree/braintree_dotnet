#pragma warning disable CS0618

using Braintree.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;

namespace Braintree.Tests
{
    [TestFixture]
    public class BraintreeGraphQLServiceTest
    {
        [Test]
        public void ThrowExceptionIfGraphQLErrorResponseHasError_whenErrorExceptionIsNull_throwsUnexpectedException()
        {
            GraphQLError error = new GraphQLError();
            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(new GraphQLError());

            Assert.Throws<UnexpectedException> (() => BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response));
        }

        [Test]
        public void ThrowExceptionIfGraphQLErrorResponseHasError_WhenErrorClassIsAuthentication_ThrowsAuthenitcationException()
        {
            Dictionary<string, string> extensions = new Dictionary<string, string>();
            extensions.Add("errorClass", "AUTHENTICATION");

            GraphQLError error = new GraphQLError();
            error.extensions = extensions;

            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(error);

            Assert.Throws<AuthenticationException> (() => BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response));
        }

        [Test]
        public void ThrowExceptionIfGraphQLErrorResponseHasError_whenErrorClassIsAuthorization_throwsAuthorizationException()
        {
            Dictionary<string, string> extensions = new Dictionary<string, string>();
            extensions.Add("errorClass", "AUTHORIZATION");

            GraphQLError error = new GraphQLError();
            error.extensions = extensions;

            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(error);

            Assert.Throws<AuthorizationException> (() => BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response));
        }

        [Test]
        public void ThrowExceptionIfGraphQLErrorResponseHasError_whenErrorClassIsNotFound_throwsNotFoundException()
        {
            Dictionary<string, string> extensions = new Dictionary<string, string>();
            extensions.Add("errorClass", "NOT_FOUND");

            GraphQLError error = new GraphQLError();
            error.extensions = extensions;

            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(error);

            Assert.Throws<NotFoundException> (() => BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response));
        }

        [Test]
        public void ThrowExceptionIfGraphQLErrorResponseHasError_whenErrorClassIsUnsupportedClient_throwsUnsupportedClientException()
        {
            Dictionary<string, string> extensions = new Dictionary<string, string>();
            extensions.Add("errorClass", "UNSUPPORTED_CLIENT");

            GraphQLError error = new GraphQLError();
            error.extensions = extensions;

            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(error);

            Assert.Throws<UpgradeRequiredException> (() => BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response));
        }

        [Test]
        public void ThrowExceptionIfGraphQLErrorResponseHasError_whenErrorClassIsResourceLimit_throwsTooManyRequestsException()
        {
            Dictionary<string, string> extensions = new Dictionary<string, string>();
            extensions.Add("errorClass", "RESOURCE_LIMIT");

            GraphQLError error = new GraphQLError();
            error.extensions = extensions;

            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(error);

            Assert.Throws<TooManyRequestsException> (() => BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response));
        }

        [Test]
        public void ThrowExceptionIfGraphQLErrorResponseHasError_whenErrorClassIsInternal_throwsServerException()
        {
            Dictionary<string, string> extensions = new Dictionary<string, string>();
            extensions.Add("errorClass", "INTERNAL");

            GraphQLError error = new GraphQLError();
            error.extensions = extensions;

            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(error);

            Assert.Throws<ServerException> (() => BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response));
        }

        [Test]
        public void ThrowExceptionIfGraphQLErrorResponseHasError_whenErrorClassIsBraintreeServiceAvailability_throwsDownForMaintanceException()
        {
            Dictionary<string, string> extensions = new Dictionary<string, string>();
            extensions.Add("errorClass", "SERVICE_AVAILABILITY");

            GraphQLError error = new GraphQLError();
            error.extensions = extensions;

            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(error);

            Assert.Throws<DownForMaintenanceException> (() => BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response));
        }

        [Test]
        public void ThrowExceptionIfGraphQLErrorResponseHasError_whenErrorClassIsUnknown_throwsUnexpectedException()
        {
            Dictionary<string, string> extensions = new Dictionary<string, string>();
            extensions.Add("errorClass", "UNKNOWN");

            GraphQLError error = new GraphQLError();
            error.extensions = extensions;

            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(error);

            Assert.Throws<UnexpectedException> (() => BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response));
        }

        [Test]
        public void ThrowExceptionIfGraphQLErrorResponseHasError_whenErrorClassIsNotMapped_throwsUnexpectedException()
        {
            Dictionary<string, string> extensions = new Dictionary<string, string>();
            extensions.Add("errorClass","FOO");

            GraphQLError error = new GraphQLError();
            error.extensions = extensions;

            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(error);

            Assert.Throws<UnexpectedException> (() => BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response));
        }

        [Test]
        public void DoNotThrowExceptionIfGraphQLErrorResponseHasError_whenErrorClassIsValidation()
        {
            Dictionary<string, string> extensions = new Dictionary<string, string>();
            extensions.Add("errorClass", "VALIDATION");

            GraphQLError error = new GraphQLError();
            error.extensions = extensions;

            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(error);

            BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response);
        }

        [Test]
        public void ThrowExceptionIfGraphQLErrorResponseHasErrors_whenValidationAndNotFoundErrorClassesExist_throwsNotFoundException()
        {
            Dictionary<string, string> validationExtensions = new Dictionary<string, string>();
            validationExtensions.Add("errorClass", "VALIDATION");
            Dictionary<string, string> notFoundExtensions = new Dictionary<string, string>();
            notFoundExtensions.Add("errorClass", "NOT_FOUND");

            GraphQLError validationError = new GraphQLError();
            validationError.extensions = validationExtensions;
            GraphQLError notFoundError = new GraphQLError();
            notFoundError.extensions = notFoundExtensions;

            GraphQLResponse response = new GraphQLResponse();
            response.errors = new List<GraphQLError>();
            response.errors.Add(validationError);
            response.errors.Add(notFoundError);

            Assert.Throws<NotFoundException> (() => BraintreeGraphQLService.ThrowExceptionIfGraphQLErrorResponseHasError(response));
        }
    }
}
#pragma warning restore CS0618
