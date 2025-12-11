using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Braintree;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Runtime.InteropServices;
using Params = System.Collections.Generic.Dictionary<string, object>;
using Response = System.Collections.Generic.Dictionary<string, string>;
#if netcore
using System.Net.Http;
#else
using System.Web;
#endif

namespace Braintree.TestUtil
{
    public class TestHelper
    {
        public static Response SampleNotificationFromXml(BraintreeGateway gateway, string xml)
        {
            var response = new Response();
            var service = new BraintreeService(gateway.Configuration);
            response["bt_payload"] = xml;
            response["bt_signature"] =
                $"{service.PublicKey}|{new Sha1Hasher().HmacHash(service.PrivateKey, xml).Trim().ToLower()}";
            return response;
        }

        public static string GenerateDecodedClientToken(
            BraintreeGateway gateway,
            ClientTokenRequest request = null
        )
        {
            var encodedClientToken = gateway.ClientToken.Generate(request);
            var decodedClientToken = Encoding.UTF8.GetString(
                Convert.FromBase64String(encodedClientToken)
            );
            var unescapedClientToken = Regex.Unescape(decodedClientToken);
            return unescapedClientToken;
        }

        public static string GenerateValidUsBankAccountNonce(
            BraintreeGateway gateway,
            string accountNumber = "1000000000"
        )
        {
            var clientToken = GenerateDecodedClientToken(gateway);
            var def = new
            {
                braintree_api = new
                {
                    url = "",
                    access_token = "",
                    port = "",
                },
            };
            var config = JsonConvert.DeserializeAnonymousType(clientToken, def);
            var url = config.braintree_api.url + "/graphql";
            var accessToken = config.braintree_api.access_token;

            string query =
                @"
                mutation tokenizeUsBankAccount($input: TokenizeUsBankAccountInput!) {
                    tokenizeUsBankAccount(input: $input) {
                        paymentMethod {
                            id
                        }
                    }
                }";

            var variables = new Dictionary<string, object>
            {
                {
                    "input",
                    new Dictionary<string, object>
                    {
                        {
                            "usBankAccount",
                            new Dictionary<string, object>
                            {
                                { "accountNumber", accountNumber },
                                { "routingNumber", "021000021" },
                                { "accountType", "CHECKING" },
                                {
                                    "billingAddress",
                                    new Dictionary<string, object>
                                    {
                                        { "streetAddress", "123 Ave" },
                                        { "city", "San Francisco" },
                                        { "state", "CA" },
                                        { "zipCode", "94112" },
                                    }
                                },
                                {
                                    "individualOwner",
                                    new Dictionary<string, object>
                                    {
                                        { "firstName", "Dan" },
                                        { "lastName", "Schulman" },
                                    }
                                },
                                { "achMandate", "cl mandate text" },
                            }
                        },
                    }
                },
            };

            Dictionary<string, object> body = new Dictionary<string, object>();
            body["query"] = query;
            body["variables"] = variables;

            string postData = JsonConvert.SerializeObject(body, Newtonsoft.Json.Formatting.None);

#if netcore
            var request = new HttpRequestMessage(new HttpMethod("POST"), url);
            byte[] buffer = Encoding.UTF8.GetBytes(postData);
            request.Content = new StringContent(postData, Encoding.UTF8, "application/json");
            request.Headers.Add("Braintree-Version", "2016-10-07");
            request.Headers.Add("Authorization", "Bearer " + accessToken);

            var httpClientHandler = new HttpClientHandler { };

            HttpResponseMessage response;
            using (var client = new HttpClient(httpClientHandler))
            {
                response = client.SendAsync(request).GetAwaiter().GetResult();
            }
            StreamReader reader = new StreamReader(
                response.Content.ReadAsStreamAsync().Result,
                Encoding.UTF8
            );
            string responseBody = reader.ReadToEnd();
#else
            string curlCommand =
                $@"-s -H ""Content-type: application/json"" -H ""Braintree-Version: 2016-10-07"" -H ""Authorization: Bearer {accessToken}"" -d '{postData}' -XPOST ""{url}""";
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "curl",
                    Arguments = curlCommand,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                },
            };
            process.Start();

            StringBuilder responseBodyBuilder = new StringBuilder();
            while (!process.HasExited)
            {
                responseBodyBuilder.Append(process.StandardOutput.ReadToEnd());
            }
            responseBodyBuilder.Append(process.StandardOutput.ReadToEnd());
            string responseBody = responseBodyBuilder.ToString();
#endif

            var resDef = new
            {
                data = new { tokenizeUsBankAccount = new { paymentMethod = new { id = "" } } },
            };
            var json = JsonConvert.DeserializeAnonymousType(responseBody, resDef);
            return json.data.tokenizeUsBankAccount.paymentMethod.id;
        }

        public static string GenerateInvalidUsBankAccountNonce()
        {
            var valid_characters = "bcdfghjkmnpqrstvwxyz23456789";
            var token = "tokenusbankacct";
            Random rnd = new Random();
            for (int i = 0; i < 4; i++)
            {
                token += "_";
                for (int j = 0; j < 6; j++)
                {
                    token += valid_characters[rnd.Next(0, valid_characters.ToCharArray().Length)];
                }
            }
            return token + "_xxx";
        }

        public static int CompareModificationsById(Modification left, Modification right)
        {
            return left.Id.CompareTo(right.Id);
        }

        public static void AreDatesEqual(DateTime expected, DateTime actual)
        {
            Assert.AreEqual(expected.Day, actual.Day);
            Assert.AreEqual(expected.Month, actual.Month);
            Assert.AreEqual(expected.Year, actual.Year);
        }

        public static void AssertIncludes(string expected, string all)
        {
            Assert.IsTrue(
                all.IndexOf(expected) >= 0,
                "Expected:\n" + all + "\nto include:\n" + expected
            );
        }

        public static bool IncludesSubscription(
            ResourceCollection<Subscription> collection,
            Subscription subscription
        )
        {
            foreach (Subscription item in collection)
            {
                if (item.Id.Equals(subscription.Id))
                {
                    return true;
                }
            }
            return false;
        }

        public static void Escrow(BraintreeService service, string transactionId)
        {
            NodeWrapper response = new NodeWrapper(
                service.Put(service.MerchantPath() + "/transactions/" + transactionId + "/escrow")
            );
            Assert.IsTrue(response.IsSuccess());
        }

        public static void PrintNestedDictionary(Dictionary<string, object> dict, int indentLevel)
        {
            string indent = new string(' ', indentLevel * 4); // Indentation for nested levels
            foreach (KeyValuePair<string, object> kvp in dict)
            {
                Console.WriteLine($"{indent}{kvp.Key}");
                if (kvp.Value is Dictionary<string, object>)
                {
                    PrintNestedDictionary((Dictionary<string, object>)kvp.Value, indentLevel + 1);
                }
                else if (kvp.Value is IList)
                {
                    var list = (List<Dictionary<string, object>>)kvp.Value;
                    foreach (var item in list)
                    {
                        PrintNestedDictionary(item, indentLevel + 1);
                    }
                }
                else
                {
                    Console.WriteLine($"{indent}{kvp.Key}: {kvp.Value}");
                }
            }
        }



        public static Dictionary<string, object> ReadJsonFile(string filePath = "", bool isPayload = false)
        {
            var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var path = System.IO.Path.Combine(directory, $"Resources/{filePath}");
            var jsonString = File.ReadAllText(path);
            // TODO: Resolve need to use different deserializers
            var dict = (isPayload) ? JsonConvert.DeserializeObject<Dictionary<string, object>>(
                jsonString,
                new JsonConverter[] { new GraphQLResponse.Deserializer() }

            ) :
            JsonConvert.DeserializeObject<Dictionary<string, object>>(
                jsonString,
                new JsonToDictionaryDeserializer()
            )
            ;
            return dict;
        }

        public static GraphQLResponse ReadGraphQLResponse(string filePath = "", bool isPayload = false)
        {
            var dict = ReadJsonFile(filePath, isPayload);
            GraphQLResponse response = new GraphQLResponse();
            if (dict.ContainsKey("data"))
            {
                response.data = (Dictionary<string, object>)dict["data"];
            }
            if (dict.ContainsKey("errors"))
            {
                var errorList = (List<Dictionary<string, object>>)dict["errors"];
                response.errors = new List<GraphQLError>();
                foreach (var err in errorList)
                {
                    var gqlError = new GraphQLError();
                    if (err.ContainsKey("message"))
                    {
                        gqlError.message = err["message"].ToString();
                    }
                    if (err.ContainsKey("extensions"))
                    {
                        gqlError.extensions = (Dictionary<string, object>)err["extensions"];

                    }
                    response.errors.Add(gqlError);
                }
            }
            return response;
        }




        public static JObject ReadResponseFromJsonResource(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var adjustedResourcePath = resourcePath.Replace("/", "."); //Replaces '/' with '.'
            using (var stream = assembly.GetManifestResourceStream(adjustedResourcePath))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Resource not found: {resourcePath}");
                }

                using (var reader = new StreamReader(stream))
                {
                    var jsonString = reader.ReadToEnd();
                    return JObject.Parse(jsonString);
                }
            }
        }

#if netcore
        public static string GetResponseContent(HttpResponseMessage response)
        {
            using (var reader = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                return reader.ReadToEnd();
        }

        public static string extractParamFromJson(string keyName, HttpResponseMessage response)
        {
            var param = extractParamFromJson(keyName, GetResponseContent(response));
            return param;
        }
#else
        public static string GetResponseContent(HttpWebResponse response)
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
                return reader.ReadToEnd();
        }

        public static string extractParamFromJson(string keyName, HttpWebResponse response)
        {
            var param = extractParamFromJson(keyName, GetResponseContent(response));
            response.Close();
            return param;
        }
#endif

        public static string extractParamFromJson(string keyName, string json)
        {
            string regex = $"\"{keyName}\":\\s?\"([^\"]+)\"";
            Match match = Regex.Match(json, regex);
            string keyValue = match.Groups[1].Value;

            return keyValue;
        }

        public static int extractIntParamFromJson(string keyName, string json)
        {
            string regex = $"\"{keyName}\":\\s?(\\d+)";
            Match match = Regex.Match(json, regex);
            int keyValue = Convert.ToInt32(match.Groups[1].Value);

            return keyValue;
        }

        public static string GenerateAuthorizationFingerprint(
            BraintreeGateway gateway,
            string customerId = null
        )
        {
            var clientTokenRequest =
                customerId == null ? null : new ClientTokenRequest { CustomerId = customerId };
            var clientToken = GenerateDecodedClientToken(gateway, clientTokenRequest);

            return extractParamFromJson("authorizationFingerprint", clientToken);
        }

        public static string GetNonceForPayPalAccount(
            BraintreeGateway gateway,
            Params paypalAccountDetails
        )
        {
            var authorizationFingerprint = GenerateAuthorizationFingerprint(gateway);
            var builder = new RequestBuilder();
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint);
            foreach (var param in paypalAccountDetails)
                builder.AddTopLevelElement($"paypal_account[{param.Key}]", param.Value.ToString());

            var response = new BraintreeTestHttpService().Post(
                gateway.MerchantId,
                "v1/payment_methods/paypal_accounts",
                builder.ToQueryString()
            );

#if netcore
            StreamReader reader = new StreamReader(
                response.Content.ReadAsStreamAsync().Result,
                Encoding.UTF8
            );
            string responseBody = reader.ReadToEnd();
            return extractParamFromJson("nonce", responseBody);
#else
            return extractParamFromJson("nonce", response);
#endif
        }

        public static string GetNonceForNewCreditCard(
            BraintreeGateway gateway,
            Params creditCardDetails,
            string customerId = null
        )
        {
            var authorizationFingerprint = GenerateAuthorizationFingerprint(gateway, customerId);

            var builder = new RequestBuilder();
            builder
                .AddTopLevelElement("authorization_fingerprint", authorizationFingerprint)
                .AddTopLevelElement("shared_customer_identifier", "test-identifier")
                .AddTopLevelElement("shared_customer_identifier_type", "testing");

            foreach (var param in creditCardDetails)
            {
                var nested = param.Value as Params;
                if (null != nested)
                {
                    foreach (var nestedParam in nested)
                    {
                        builder.AddTopLevelElement(
                            $"credit_card[{param.Key}][{nestedParam.Key}]",
                            nestedParam.Value.ToString()
                        );
                    }
                }
                else
                    builder.AddTopLevelElement($"credit_card[{param.Key}]", param.Value.ToString());
            }

            var response = new BraintreeTestHttpService().Post(
                gateway.MerchantId,
                "v1/payment_methods/credit_cards",
                builder.ToQueryString()
            );

            return extractParamFromJson("nonce", response);
        }

        public static string GetNonceForNewPaymentMethod(
            BraintreeGateway gateway,
            Params @params,
            bool isCreditCard
        )
        {
            var authorizationFingerprint = GenerateAuthorizationFingerprint(gateway);

            var paymentMethodType = isCreditCard ? "credit_card" : "paypal_account";
            var paymentMethodTypePlural = paymentMethodType + "s";
            var builder = new RequestBuilder();
            builder
                .AddTopLevelElement("authorization_fingerprint", authorizationFingerprint)
                .AddTopLevelElement("shared_customer_identifier", "test-identifier")
                .AddTopLevelElement("shared_customer_identifier_type", "testing");
            foreach (var param in @params)
                builder.AddTopLevelElement(
                    $"{paymentMethodType}[{param.Key}]",
                    param.Value.ToString()
                );

            var response = new BraintreeTestHttpService().Post(
                gateway.MerchantId,
                "v1/payment_methods/" + paymentMethodTypePlural,
                builder.ToQueryString()
            );

#if netcore
            StreamReader reader = new StreamReader(
                response.Content.ReadAsStreamAsync().Result,
                Encoding.UTF8
            );
            string responseBody = reader.ReadToEnd();
            return extractParamFromJson("nonce", responseBody);
#else
            return extractParamFromJson("nonce", response);
#endif
        }

        public static string GenerateUnlockedNonce(
            BraintreeGateway gateway,
            string creditCardNumber,
            string customerId
        )
        {
            var authorizationFingerprint = GenerateAuthorizationFingerprint(gateway, customerId);

            RequestBuilder builder = new RequestBuilder("");
            builder
                .AddTopLevelElement("authorization_fingerprint", authorizationFingerprint)
                .AddTopLevelElement("shared_customer_identifier_type", "testing")
                .AddTopLevelElement("shared_customer_identifier", "test-identifier")
                .AddTopLevelElement("credit_card[number]", creditCardNumber)
                .AddTopLevelElement("share", "true")
                .AddTopLevelElement("credit_card[expiration_month]", "11")
                .AddTopLevelElement("credit_card[expiration_year]", "2099");

            var response = new BraintreeTestHttpService().Post(
                gateway.MerchantId,
                "v1/payment_methods/credit_cards",
                builder.ToQueryString()
            );

#if netcore
            StreamReader reader = new StreamReader(
                response.Content.ReadAsStreamAsync().Result,
                Encoding.UTF8
            );
#else
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
#endif

            string responseBody = reader.ReadToEnd();
            reader.Close();

            return extractParamFromJson("nonce", responseBody);
        }

        public static string GenerateUnlockedNonce(BraintreeGateway gateway)
        {
            return GenerateUnlockedNonce(gateway, "4111111111111111", null);
        }

        public static string GenerateOneTimePayPalNonce(BraintreeGateway gateway)
        {
            var authorizationFingerprint = GenerateAuthorizationFingerprint(gateway);
            RequestBuilder builder = new RequestBuilder("");
            builder
                .AddTopLevelElement("authorization_fingerprint", authorizationFingerprint)
                .AddTopLevelElement("shared_customer_identifier_type", "testing")
                .AddTopLevelElement("shared_customer_identifier", "test-identifier")
                .AddTopLevelElement("paypal_account[access_token]", "access_token")
                .AddTopLevelElement("paypal_account[correlation_id]", Guid.NewGuid().ToString())
                .AddTopLevelElement("paypal_account[options][validate]", "false");

            var response = new BraintreeTestHttpService().Post(
                gateway.MerchantId,
                "v1/payment_methods/paypal_accounts",
                builder.ToQueryString()
            );

#if netcore
            StreamReader reader = new StreamReader(
                response.Content.ReadAsStreamAsync().Result,
                Encoding.UTF8
            );
#else
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
#endif

            string responseBody = reader.ReadToEnd();
            reader.Close();

            Regex regex = new Regex("nonce\":\"(?<nonce>[a-f0-9\\-]+)\"");
            Match match = regex.Match(responseBody);
            return match.Groups["nonce"].Value;
        }

        public static string GenerateFuturePaymentPayPalNonce(BraintreeGateway gateway)
        {
            var authorizationFingerprint = GenerateAuthorizationFingerprint(gateway);
            RequestBuilder builder = new RequestBuilder("");
            builder
                .AddTopLevelElement("authorization_fingerprint", authorizationFingerprint)
                .AddTopLevelElement("shared_customer_identifier_type", "testing")
                .AddTopLevelElement("shared_customer_identifier", "test-identifier")
                .AddTopLevelElement("paypal_account[consent_code]", "consent")
                .AddTopLevelElement("paypal_account[correlation_id]", Guid.NewGuid().ToString())
                .AddTopLevelElement("paypal_account[options][validate]", "false");

            var response = new BraintreeTestHttpService().Post(
                gateway.MerchantId,
                "v1/payment_methods/paypal_accounts",
                builder.ToQueryString()
            );

#if netcore
            StreamReader reader = new StreamReader(
                response.Content.ReadAsStreamAsync().Result,
                Encoding.UTF8
            );
#else
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
#endif

            string responseBody = reader.ReadToEnd();
            reader.Close();

            Regex regex = new Regex("nonce\":\"(?<nonce>[a-f0-9\\-]+)\"");
            Match match = regex.Match(responseBody);
            return match.Groups["nonce"].Value;
        }

        public static string GenerateOrderPaymentPayPalNonce(BraintreeGateway gateway)
        {
            var authorizationFingerprint = GenerateAuthorizationFingerprint(gateway);
            RequestBuilder builder = new RequestBuilder("");
            builder
                .AddTopLevelElement("authorization_fingerprint", authorizationFingerprint)
                .AddTopLevelElement("shared_customer_identifier_type", "testing")
                .AddTopLevelElement("shared_customer_identifier", "test-identifier")
                .AddTopLevelElement("paypal_account[intent]", "order")
                .AddTopLevelElement("paypal_account[payment_token]", Guid.NewGuid().ToString())
                .AddTopLevelElement("paypal_account[payer_id]", "false");

            var response = new BraintreeTestHttpService().Post(
                gateway.MerchantId,
                "v1/payment_methods/paypal_accounts",
                builder.ToQueryString()
            );

#if netcore
            StreamReader reader = new StreamReader(
                response.Content.ReadAsStreamAsync().Result,
                Encoding.UTF8
            );
#else
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
#endif

            string responseBody = reader.ReadToEnd();
            reader.Close();

            Regex regex = new Regex("nonce\":\"(?<nonce>[a-f0-9\\-]+)\"");
            Match match = regex.Match(responseBody);
            return match.Groups["nonce"].Value;
        }

        public static string Create3DSVerification(
            BraintreeService service,
            string merchantAccountId,
            ThreeDSecureRequestForTests request
        )
        {
            string url = "/three_d_secure/create_verification/" + merchantAccountId;
            NodeWrapper response = new NodeWrapper(
                service.Post(service.MerchantPath() + url, request)
            );
            Assert.IsTrue(response.IsSuccess());
            return response.GetString("three-d-secure-authentication-id");
        }

        public static string Generate3DSNonce(BraintreeService service, CreditCardRequest request)
        {
            string url =
                "/three_d_secure/create_nonce/"
                + MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID;
            NodeWrapper response = new NodeWrapper(
                service.Post(service.MerchantPath() + url, request)
            );
            Assert.IsTrue(response.IsSuccess());
            return response.GetString("nonce");
        }
    }

    public class OAuthTestHelper
    {
        private class OAuthGrantRequest : Request
        {
            public string MerchantId { get; set; }
            public string Scope { get; set; }

            public override string ToXml()
            {
                return new RequestBuilder("grant")
                    .AddElement("merchant_public_id", MerchantId)
                    .AddElement("scope", Scope)
                    .ToXml();
            }
        }

        public static string CreateGrant(BraintreeGateway gateway, string merchantId, string scope)
        {
            var service = new BraintreeService(gateway.Configuration);
            XmlNode node = service.Post(
                "/oauth_testing/grants",
                new OAuthGrantRequest { MerchantId = merchantId, Scope = scope }
            );

            return node["code"].InnerText;
        }

        public static MerchantResult GetMerchant(string merchantId = "partner_merchant_id")
        {
            var gateway = new BraintreeGateway(
                "client_id$development$integration_client_id",
                "client_secret$development$integration_client_secret"
            );

            string code = CreateGrant(gateway, merchantId, "read_write");

            var accessTokenResult = gateway.OAuth.CreateTokenFromCode(new OAuthCredentialsRequest {
                Code = code,
                Scope = "read_write"
            });

            var merchantGateway = new BraintreeGateway(accessTokenResult.Target.AccessToken);
            var merchantAccounts = merchantGateway.MerchantAccount.All();

            return new MerchantResult
            {
                Credentials = accessTokenResult.Target,
                MerchantAccounts = merchantAccounts
            };
        }

        public class MerchantResult
        {
            public OAuthCredentials Credentials { get; set; }
            public PaginatedCollection<MerchantAccount> MerchantAccounts { get; set; }
        }
    }

    public class BraintreeTestHttpService
    {
        public string ApiVersion = "3";

#if netcore
        public HttpResponseMessage Get(string MerchantId, string URL)
        {
            return GetJsonResponse(MerchantId, URL, "GET", null);
        }

        public HttpResponseMessage Post(string MerchantId, string URL, string requestBody)
        {
            return GetJsonResponse(MerchantId, URL, "POST", requestBody);
        }

        private HttpResponseMessage GetJsonResponse(
            string MerchantId,
            string Path,
            string method,
            string requestBody
        )
        {
            try
            {
                var request = new HttpRequestMessage(
                    new HttpMethod(method),
                    Environment.DEVELOPMENT.GatewayURL
                        + "/merchants/"
                        + MerchantId
                        + "/client_api/"
                        + Path
                );
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("Accept", "application/xml");
                request.Headers.Add(
                    "UserAgent",
                    "Braintree .NET "
                        + typeof(TestHelper).GetTypeInfo().Assembly.GetName().Version.ToString()
                );
                request.Headers.Add("KeepAlive", "false");
                request.Headers.Add("Timeout", "60000");
                request.Headers.Add("ReadWriteTimeout", "60000");

                if (requestBody != null)
                {
                    var content = requestBody;
                    var utf8_string = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(requestBody));
                    request.Content = new StringContent(
                        utf8_string,
                        Encoding.UTF8,
                        "application/x-www-form-urlencoded"
                    );
                    request.Content.Headers.ContentLength = UTF8Encoding.UTF8.GetByteCount(
                        utf8_string
                    );
                }

                var httpClientHandler = new HttpClientHandler
                {
                    AutomaticDecompression =
                        DecompressionMethods.GZip | DecompressionMethods.Deflate,
                };

                HttpResponseMessage response;

                using (var client = new HttpClient(httpClientHandler))
                {
                    response = client.SendAsync(request).GetAwaiter().GetResult();
                }

                return response;
            }
            catch (WebException e)
            {
                var errorWebResponse = e.Response as HttpWebResponse;
                var errorResponseMessage = new HttpResponseMessage(errorWebResponse.StatusCode);
                errorResponseMessage.Content = new StringContent(
                    e.Response.ToString(),
                    Encoding.UTF8,
                    "application/json"
                );
                if (errorWebResponse == null)
                    throw e;
                return errorResponseMessage;
            }
        }
#else
        public HttpWebResponse Get(string MerchantId, string URL)
        {
            return GetJsonResponse(MerchantId, URL, "GET", null);
        }

        public HttpWebResponse Post(string MerchantId, string URL, string requestBody)
        {
            return GetJsonResponse(MerchantId, URL, "POST", requestBody);
        }

        private HttpWebResponse GetJsonResponse(
            string MerchantId,
            string Path,
            string method,
            string requestBody
        )
        {
            try
            {
                var request =
                    WebRequest.Create(
                        Environment.DEVELOPMENT.GatewayURL
                            + "/merchants/"
                            + MerchantId
                            + "/client_api/"
                            + Path
                    ) as HttpWebRequest;
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Accept = "application/json";
                request.UserAgent =
                    "Braintree .NET "
                    + typeof(BraintreeService).Assembly.GetName().Version.ToString();
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
                return response;
            }
            catch (WebException e)
            {
                var response = (HttpWebResponse)e.Response;
                if (response == null)
                    throw e;
                return response;
            }
        }
#endif
    }

    public class JsonToDictionaryDeserializer : CustomCreationConverter<IDictionary<string, object>>
    {
        public override IDictionary<string, object> Create(Type objectType)
        {
            return new Dictionary<string, object>();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object)
                || objectType == typeof(Dictionary<string, object>)
                || objectType.IsGenericType
                    && objectType.GetGenericTypeDefinition() == typeof(List<Dictionary<string, object>>)
                || objectType.IsGenericType
                    && objectType.GetGenericTypeDefinition() == typeof(List<>);
        }


        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer
        )
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject jObject = JObject.Load(reader);
                return ConvertJObject(jObject);
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                JArray jArray = JArray.Load(reader);
                return ConvertJArray(jArray);
            }

            return serializer.Deserialize(reader);
        }

        private List<Dictionary<string, object>> ConvertJArray(JArray array)
        {
            List<Dictionary<string, object>> converted = new List<Dictionary<string, object>>();
            foreach (var elem in array)
            {
                Dictionary<string, object> dict = ConvertJObject((JObject)elem);
                converted.Add(dict);
            }
            return converted;
        }

        private Dictionary<string, object> ConvertJObject(JObject jObject)
        {
            var dict = new Dictionary<string, object>();
            foreach (var prop in jObject.Properties())
            {
                dict.Add(prop.Name, ConvertJToken(prop.Value));
            }

            return dict;
        }

        private object ConvertJToken(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    var ret = ConvertJObject((JObject)token);
                    return ret;

                case JTokenType.Array:
                    var retArray = ConvertJArray((JArray)token);
                    return retArray;
                default:
                    return token.Value<object>();
            }
        }
    }
}
