#pragma warning disable 1591

using Braintree.Exceptions;
using System;
using System.IO;
using System.Net;
#if netcore
using System.Net.Http;
#endif
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Braintree
{
    public class GraphQLResponse
    {
        public Dictionary<string, object> data { get; set; }
        public Dictionary<string, object> extensions { get; set; }
        public IList<GraphQLError> errors { get; set; }

        public ValidationErrors GetValidationErrors()
        {
            ValidationErrors validationErrors = new ValidationErrors();
            if (errors != null)
            {
                foreach (GraphQLError error in errors)
                {
                    if (error.message != null)
                    {
                        var validationalidateErrorCode = GetValidationErrorCode(error.extensions);
                        if (validationalidateErrorCode != null)
                        {
                            validationErrors.AddError(
                                "",
                                new ValidationError("", validationalidateErrorCode, error.message)
                            );
                        }
                    }
                }
            }
            return validationErrors;
        }

        private static string GetValidationErrorCode(Dictionary<string, object> extensions)
        {

            if (extensions != null && extensions.ContainsKey("legacyCode"))
            {
                return extensions["legacyCode"].ToString();
            }
            return "99999";
        }


        public class Deserializer : CustomCreationConverter<IDictionary<string, object>>
        {
            public override IDictionary<string, object> Create(Type objectType)
            {
                return new Dictionary<string, object>();
            }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(object) || base.CanConvert(objectType);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.Null)
                {
                    return base.ReadJson(reader, objectType, existingValue, serializer);
                }

                return serializer.Deserialize(reader);
            }
        }
    }

    public class GraphQLError
    {
        public string message { get; set; }
        public Dictionary<string, object> extensions { get; set; }
    }

    public class BraintreeGraphQLService : HttpService
    {
        public BraintreeGraphQLService(Configuration configuration) : base(configuration) { }

        public string GraphQLApiVersion = "2018-09-10";

#if netcore
        public override void SetRequestHeaders(HttpRequestMessage request)
        {
            base.SetRequestHeaders(request);
            request.Headers.Add("Braintree-Version", GraphQLApiVersion);
            request.Headers.Add("Accept", "application/json");
        }
#else
        public override void SetRequestHeaders(HttpWebRequest request)
        {
            base.SetRequestHeaders(request);
            request.Headers.Add("Braintree-Version", GraphQLApiVersion);
            request.Accept = "application/json";
            request.ContentType = "application/json";
        }
#endif

        public GraphQLResponse QueryGraphQL(string definition, Dictionary<string, object> variables)
        {
            var request = GetGraphQLHttpRequest(definition, variables);
            var response = GetHttpResponse(request);
            var result = JsonConvert.DeserializeObject<GraphQLResponse>(response, new JsonConverter[] { new GraphQLResponse.Deserializer() });

            ThrowExceptionIfGraphQLErrorResponseHasError(result);

            return result;
        }

        public async Task<GraphQLResponse> QueryGraphQLAsync(string definition, Dictionary<string, object> variables)
        {
            var request = GetGraphQLHttpRequest(definition, variables);
            var response = await GetHttpResponseAsync(request).ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<GraphQLResponse>(response, new JsonConverter[] { new GraphQLResponse.Deserializer() });

            ThrowExceptionIfGraphQLErrorResponseHasError(result);

            return result;
        }

#if netcore
        public HttpRequestMessage GetGraphQLHttpRequest(string definition, Dictionary<string, object> variables)
        {
            var request = GetHttpRequest(Configuration.GetGraphQLUrl(), "post");

            var body = FormatGraphQLRequest(definition, variables);
            var body_bytes = encoding.GetBytes(body);

            var utf8_string = encoding.GetString(body_bytes);

            request.Content = new StringContent(utf8_string, encoding, "application/json");
            request.Content.Headers.ContentLength = System.Text.UTF8Encoding.UTF8.GetByteCount(utf8_string);

            return request;
        }
#else
        public HttpWebRequest GetGraphQLHttpRequest(string definition, Dictionary<string, object> variables)
        {
            var request = GetHttpRequest(Configuration.GetGraphQLUrl(), "post");

            var body = FormatGraphQLRequest(definition, variables);
            var body_bytes = encoding.GetBytes(body);

            request.ContentLength = body_bytes.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(body_bytes, 0, body_bytes.Length);
            }

            return request;
        }
#endif

        private string FormatGraphQLRequest(string definition, Dictionary<string, object> variables)
        {
            Dictionary<string, object> body = new Dictionary<string, object>();

            body["query"] = definition;
            body["variables"] = variables;

            return JsonConvert.SerializeObject(body, Newtonsoft.Json.Formatting.None);
        }

        public static void ThrowExceptionIfGraphQLErrorResponseHasError(GraphQLResponse response)
        {
            var errors = response.errors;

            if (errors == null)
            {
                return;
            }

            foreach (var error in errors)
            {
                var message = error.message;

                if (error.extensions == null)
                {
                    throw new UnexpectedException();
                }

                switch ((string)error.extensions["errorClass"])
                {
                    case "VALIDATION":
                        continue;
                    case "AUTHENTICATION":
                        throw new AuthenticationException();
                    case "AUTHORIZATION":
                        throw new AuthorizationException(message);
                    case "NOT_FOUND":
                        throw new NotFoundException(message);
                    case "UNSUPPORTED_CLIENT":
                        throw new UpgradeRequiredException();
                    case "RESOURCE_LIMIT":
                        throw new TooManyRequestsException();
                    case "INTERNAL":
                        throw new ServerException();
                    case "SERVICE_AVAILABILITY":
                        throw new ServiceUnavailableException();
                    case "UNKNOWN":
                    default:
                        throw new UnexpectedException();
                }
            }
        }
    }
}
#pragma warning restore 1591
