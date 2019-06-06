using Braintree;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
#if netcore
using System.Net.Http;
#else
using System.Web;
#endif
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Newtonsoft.Json;
using Params = System.Collections.Generic.Dictionary<string, object>;
using Response = System.Collections.Generic.Dictionary<string, string>;
using System.Diagnostics;

namespace Braintree.TestUtil
{
    public class TestHelper
    {
        public static Response SampleNotificationFromXml(BraintreeGateway gateway, string xml)
        {
            var response = new Response();
            var service = new BraintreeService(gateway.Configuration);
            response["bt_payload"] = xml;
            response["bt_signature"] = string.Format("{0}|{1}", service.PublicKey, new Sha1Hasher().HmacHash(service.PrivateKey, xml).Trim().ToLower());
            return response;
        }

        public static string GenerateDecodedClientToken(BraintreeGateway gateway, ClientTokenRequest request = null)
        {
            var encodedClientToken = gateway.ClientToken.Generate(request);
            var decodedClientToken = Encoding.UTF8.GetString(Convert.FromBase64String(encodedClientToken));
            var unescapedClientToken = Regex.Unescape(decodedClientToken);
            return unescapedClientToken;
        }

        public static string GenerateValidUsBankAccountNonce(BraintreeGateway gateway, string accountNumber = "1000000000")
        {
            var clientToken = GenerateDecodedClientToken(gateway);
            var def =  new {
                braintree_api = new {
                    url = "",
                    access_token = ""
                }
            };
            var config = JsonConvert.DeserializeAnonymousType(clientToken, def);
            var url = config.braintree_api.url + "/tokens";
            var accessToken = config.braintree_api.access_token;
            string postData = @"
            {
                ""type"": ""us_bank_account"",
                ""billing_address"": {
                    ""street_address"": ""123 Ave"",
                    ""region"": ""CA"",
                    ""locality"": ""San Francisco"",
                    ""postal_code"": ""94112""
                },
                ""account_type"": ""checking"",
                ""routing_number"": ""021000021"",
                ""account_number"": """ + accountNumber + @""",
                ""ownership_type"": ""personal"",
                ""first_name"": ""Dan"",
                ""last_name"": ""Schulman"",
                ""ach_mandate"": {
                    ""text"": ""cl mandate text""
                }
            }";

#if netcore
            var request = new HttpRequestMessage(new HttpMethod("POST"), url);
            byte[] buffer = Encoding.UTF8.GetBytes(postData);
            request.Content = new StringContent(postData, Encoding.UTF8, "application/json");
            request.Headers.Add("Braintree-Version", "2016-10-07");
            request.Headers.Add("Authorization", "Bearer " + accessToken);

            var httpClientHandler = new HttpClientHandler{};

            HttpResponseMessage response;
            using (var client = new HttpClient(httpClientHandler))
            {
                response = client.SendAsync(request).GetAwaiter().GetResult();
            }
            StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);
            string responseBody = reader.ReadToEnd();
#else
            string curlCommand = $@"-s -H ""Content-type: application/json"" -H ""Braintree-Version: 2016-10-07"" -H ""Authorization: Bearer {accessToken}"" -d '{postData}' -XPOST ""{url}""";
            Process process = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "curl",
                             Arguments = curlCommand,
                             UseShellExecute = false,
                             RedirectStandardOutput = true,
                }
            };
            process.Start();

            StringBuilder responseBodyBuilder = new StringBuilder();
            while (!process.HasExited) {
                responseBodyBuilder.Append(process.StandardOutput.ReadToEnd());
            }
            responseBodyBuilder.Append(process.StandardOutput.ReadToEnd());
            string responseBody = responseBodyBuilder.ToString();
#endif
            var resDef =  new {
                data = new {
                    id = "",
                }
            };
            var json = JsonConvert.DeserializeAnonymousType(responseBody, resDef);
            return json.data.id;
        }

        public static string GenerateInvalidUsBankAccountNonce()
        {
            var valid_characters = "bcdfghjkmnpqrstvwxyz23456789";
            var token = "tokenusbankacct";
            Random rnd = new Random();
            for(int i=0; i<4; i++) {
                token += "_";
                for(int j=0; j<6; j++) {
                    token += valid_characters[rnd.Next(0,valid_characters.ToCharArray().Length)];
                }
            }
            return token + "_xxx";
        }

        public static int CompareModificationsById(Modification left, Modification right)
        {
            return left.Id.CompareTo(right.Id);
        }

        public static string QueryStringForTR(Request trParams, Request req, string postURL, BraintreeService service)
        {
#if netcore
            string trData = TrUtil.BuildTrData(trParams, "http://example.com", service);
            string postData = "tr_data=" + WebUtility.UrlEncode(trData) + "&";
            postData += req.ToQueryString();

            var request = new HttpRequestMessage(new HttpMethod("POST"), postURL);

            request.Headers.Add("KeepAlive", "false");
            request.Headers.Add("Accept", "application/json");
            byte[] buffer = Encoding.UTF8.GetBytes(postData);
            request.Content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
            request.Content.Headers.Add("Content-Length", buffer.Length.ToString());
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = false,
            };

            HttpResponseMessage response;

            using (var client = new HttpClient(httpClientHandler))
            {
                response = client.SendAsync(request).GetAwaiter().GetResult();
            }
            
            StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);
            string responseBody = reader.ReadToEnd();
            string query = response.Headers.Location.Query;
            return query;
#else
            string trData = TrUtil.BuildTrData(trParams, "http://example.com", service);
            string postData = "tr_data=" + HttpUtility.UrlEncode(trData, Encoding.UTF8) + "&";
            postData += req.ToQueryString();

            var request = WebRequest.Create(postURL) as HttpWebRequest;

            request.Method = "POST";
            request.KeepAlive = false;
            request.AllowAutoRedirect = false;

            byte[] buffer = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = buffer.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();

            var response = request.GetResponse() as HttpWebResponse;
            string query = new Uri(response.GetResponseHeader("Location")).Query;

            response.Close();

            return query;
#endif
        }

        public static void AreDatesEqual(DateTime expected, DateTime actual)
        {
            Assert.AreEqual(expected.Day, actual.Day);
            Assert.AreEqual(expected.Month, actual.Month);
            Assert.AreEqual(expected.Year, actual.Year);
        }

        public static void AssertIncludes(string expected, string all)
        {
            Assert.IsTrue(all.IndexOf(expected) >= 0, "Expected:\n" + all + "\nto include:\n" + expected);
        }

        public static bool IncludesSubscription(ResourceCollection<Subscription> collection, Subscription subscription)
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
            NodeWrapper response = new NodeWrapper(service.Put(service.MerchantPath() + "/transactions/" + transactionId + "/escrow"));
            Assert.IsTrue(response.IsSuccess());
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
            string regex = string.Format("\"{0}\":\\s?\"([^\"]+)\"", keyName);
            Match match = Regex.Match(json, regex);
            string keyValue = match.Groups[1].Value;

            return keyValue;
        }

        public static int extractIntParamFromJson(string keyName, string json)
        {
            string regex = string.Format("\"{0}\":\\s?(\\d+)", keyName);
            Match match = Regex.Match(json, regex);
            int keyValue = Convert.ToInt32(match.Groups[1].Value);

            return keyValue;
        }

        public static string GetNonceForPayPalAccount(BraintreeGateway gateway, Params paypalAccountDetails)
        {
            var clientToken = GenerateDecodedClientToken(gateway);
            var authorizationFingerprint = extractParamFromJson("authorizationFingerprint", clientToken);
            var builder = new RequestBuilder();
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint);
            foreach (var param in paypalAccountDetails)
                builder.AddTopLevelElement(string.Format("paypal_account[{0}]", param.Key), param.Value.ToString());

            var response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/paypal_accounts", builder.ToQueryString());

#if netcore
            StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);
            string responseBody = reader.ReadToEnd();
            return extractParamFromJson("nonce", responseBody);
#else
            return extractParamFromJson("nonce", response);
#endif
        }

        public static string GetNonceForNewCreditCard(BraintreeGateway gateway, Params creditCardDetails, string customerId = null)
        {
            var clientToken = GenerateDecodedClientToken(
                gateway,
                customerId == null ? null : new ClientTokenRequest { CustomerId = customerId });

            var authorizationFingerprint = extractParamFromJson("authorizationFingerprint", clientToken);

            var builder = new RequestBuilder();
            builder.
                AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("shared_customer_identifier_type", "testing");

            foreach (var param in creditCardDetails) {
                var nested = param.Value as Params;
                if (null != nested) {
                    foreach (var nestedParam in nested) {
                        builder.AddTopLevelElement(string.Format("credit_card[{0}][{1}]", param.Key, nestedParam.Key), nestedParam.Value.ToString());
                    }
                } else
                    builder.AddTopLevelElement(string.Format("credit_card[{0}]", param.Key), param.Value.ToString());
            }

            var response = new BraintreeTestHttpService().Post(
                gateway.MerchantId,
                "v1/payment_methods/credit_cards",
                builder.ToQueryString());

            return extractParamFromJson("nonce", response);
        }

        public static string GetNonceForNewPaymentMethod(BraintreeGateway gateway, Params @params, bool isCreditCard)
        {
            var clientToken = GenerateDecodedClientToken(gateway);
            var authorizationFingerprint = extractParamFromJson("authorizationFingerprint", clientToken);

            var paymentMethodType = isCreditCard ? "credit_card" : "paypal_account";
            var paymentMethodTypePlural = paymentMethodType + "s";
            var builder = new RequestBuilder();
            builder.
                AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("shared_customer_identifier_type", "testing");
            foreach (var param in @params)
                builder.AddTopLevelElement(string.Format("{0}[{1}]", paymentMethodType, param.Key), param.Value.ToString());

            var response = new BraintreeTestHttpService().Post(
                gateway.MerchantId,
                "v1/payment_methods/" + paymentMethodTypePlural,
                builder.ToQueryString());

#if netcore
            StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);
            string responseBody = reader.ReadToEnd();
            return extractParamFromJson("nonce", responseBody);
#else
            return extractParamFromJson("nonce", response);
#endif
        }

        public static string GenerateUnlockedNonce(BraintreeGateway gateway, string creditCardNumber, string customerId)
        {
            var clientToken = "";
            if (customerId == null) {
                clientToken = GenerateDecodedClientToken(gateway);
            } else {
                clientToken = GenerateDecodedClientToken(gateway, new ClientTokenRequest
                {
                    CustomerId = customerId
                }
                );
            }
            var authorizationFingerprint = extractParamFromJson("authorizationFingerprint", clientToken);
            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
            AddTopLevelElement("shared_customer_identifier_type", "testing").
            AddTopLevelElement("shared_customer_identifier", "test-identifier").
            AddTopLevelElement("credit_card[number]", creditCardNumber).
            AddTopLevelElement("share", "true").
            AddTopLevelElement("credit_card[expiration_month]", "11").
            AddTopLevelElement("credit_card[expiration_year]", "2099");

            var response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/credit_cards", builder.ToQueryString());

#if netcore
            StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);
#else
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
#endif

            string responseBody = reader.ReadToEnd();

            return extractParamFromJson("nonce", responseBody);
        }

        public static string GenerateUnlockedNonce(BraintreeGateway gateway)
        {
            return GenerateUnlockedNonce(gateway, "4111111111111111", null);
        }

        public static string GenerateOneTimePayPalNonce(BraintreeGateway gateway)
        {
            var clientToken = GenerateDecodedClientToken(gateway);
            var authorizationFingerprint = extractParamFromJson("authorizationFingerprint", clientToken);
            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier_type", "testing").
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("paypal_account[access_token]", "access_token").
                AddTopLevelElement("paypal_account[correlation_id]", Guid.NewGuid().ToString()).
                AddTopLevelElement("paypal_account[options][validate]", "false");

            var response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/paypal_accounts", builder.ToQueryString());

#if netcore
            StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);
#else
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
#endif

            string responseBody = reader.ReadToEnd();

            Regex regex = new Regex("nonce\":\"(?<nonce>[a-f0-9\\-]+)\"");
            Match match = regex.Match(responseBody);
            return match.Groups["nonce"].Value;
        }

        public static string GenerateFuturePaymentPayPalNonce(BraintreeGateway gateway)
        {
            var clientToken = GenerateDecodedClientToken(gateway);
            var authorizationFingerprint = extractParamFromJson("authorizationFingerprint", clientToken);
            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier_type", "testing").
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("paypal_account[consent_code]", "consent").
                AddTopLevelElement("paypal_account[correlation_id]", Guid.NewGuid().ToString()).
                AddTopLevelElement("paypal_account[options][validate]", "false");

            var response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/paypal_accounts", builder.ToQueryString());

#if netcore
            StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);
#else
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
#endif

            string responseBody = reader.ReadToEnd();

            Regex regex = new Regex("nonce\":\"(?<nonce>[a-f0-9\\-]+)\"");
            Match match = regex.Match(responseBody);
            return match.Groups["nonce"].Value;
        }

        public static string GenerateOrderPaymentPayPalNonce(BraintreeGateway gateway)
        {
            var clientToken = GenerateDecodedClientToken(gateway);
            var authorizationFingerprint = extractParamFromJson("authorizationFingerprint", clientToken);
            RequestBuilder builder = new RequestBuilder("");
            builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
                AddTopLevelElement("shared_customer_identifier_type", "testing").
                AddTopLevelElement("shared_customer_identifier", "test-identifier").
                AddTopLevelElement("paypal_account[intent]", "order").
                AddTopLevelElement("paypal_account[payment_token]", Guid.NewGuid().ToString()).
                AddTopLevelElement("paypal_account[payer_id]", "false");

            var response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/paypal_accounts", builder.ToQueryString());

#if netcore
            StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().Result, Encoding.UTF8);
#else
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
#endif

            string responseBody = reader.ReadToEnd();

            Regex regex = new Regex("nonce\":\"(?<nonce>[a-f0-9\\-]+)\"");
            Match match = regex.Match(responseBody);
            return match.Groups["nonce"].Value;
        }

        public static string Create3DSVerification(BraintreeService service, string merchantAccountId, ThreeDSecureRequestForTests request)
        {
            string url = "/three_d_secure/create_verification/" + merchantAccountId;
            NodeWrapper response = new NodeWrapper(service.Post(service.MerchantPath() + url, request));
            Assert.IsTrue(response.IsSuccess());
            return response.GetString("three-d-secure-token");
        }

        public static string Generate3DSNonce(BraintreeService service, CreditCardRequest request)
        {
            string url = "/three_d_secure/create_nonce/" + MerchantAccountIDs.THREE_D_SECURE_MERCHANT_ACCOUNT_ID;
            NodeWrapper response = new NodeWrapper(service.Post(service.MerchantPath() + url, request));
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
            XmlNode node = service.Post("/oauth_testing/grants", new OAuthGrantRequest
            {
                MerchantId = merchantId,
                Scope = scope
            });

            return node["code"].InnerText;
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

        private HttpResponseMessage GetJsonResponse(string MerchantId, string Path, string method, string requestBody)
        {

            try
            {
                var request = new HttpRequestMessage(new HttpMethod(method), Environment.DEVELOPMENT.GatewayURL + "/merchants/" + MerchantId + "/client_api/" + Path);
                request.Headers.Add("X-ApiVersion", ApiVersion);
                request.Headers.Add("Accept", "application/xml");
                request.Headers.Add("UserAgent", "Braintree .NET " + typeof(TestHelper).GetTypeInfo().Assembly.GetName().Version.ToString());
                request.Headers.Add("KeepAlive", "false");
                request.Headers.Add("Timeout", "60000");
                request.Headers.Add("ReadWriteTimeout", "60000");

                if (requestBody != null)
                {
                    var content = requestBody;
                    var utf8_string = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(requestBody));
                    request.Content = new StringContent(utf8_string, Encoding.UTF8,"application/x-www-form-urlencoded");
                    request.Content.Headers.ContentLength = UTF8Encoding.UTF8.GetByteCount(utf8_string);
                }
                    
                var httpClientHandler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
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
                errorResponseMessage.Content = new StringContent(e.Response.ToString(), Encoding.UTF8, "application/json");
                if (errorWebResponse == null) throw e;
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

        private HttpWebResponse GetJsonResponse(string MerchantId, string Path, string method, string requestBody)
        {
            try
            {
                var request = WebRequest.Create(Environment.DEVELOPMENT.GatewayURL + "/merchants/" + MerchantId + "/client_api/" + Path) as HttpWebRequest;
                request.Headers.Add("X-ApiVersion", ApiVersion);
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
                return response;
            }

            catch (WebException e)
            {
                var response = (HttpWebResponse)e.Response;
                if (response == null) throw e;
                return response;
            }
        }
#endif

    }
}
