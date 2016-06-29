using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Reflection;
using System.Xml;
using NUnit.Framework;
using Braintree;

using Params = System.Collections.Generic.Dictionary<string, object>;
using Microsoft.AspNetCore.WebUtilities;

namespace Braintree.Tests
{
  public class TestHelper
  {

    public static string GenerateDecodedClientToken(BraintreeGateway gateway, ClientTokenRequest request = null)
    {
      var encodedClientToken = gateway.ClientToken.generate(request);
      var decodedClientToken = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedClientToken));
      var unescapedClientToken = System.Text.RegularExpressions.Regex.Unescape(decodedClientToken);
      return unescapedClientToken;
    }

    public static int CompareModificationsById(Modification left, Modification right)
    {
      return left.Id.CompareTo(right.Id);
    }

    public static string QueryStringForTR(Request trParams, Request req, string postURL, BraintreeService service)
    {
      string trData = TrUtil.BuildTrData(trParams, "http://example.com", service);
      string postData = "tr_data=" + WebUtility.UrlEncode(trData) + "&";
      postData += req.ToQueryString();

      var request = new HttpRequestMessage(HttpMethod.Post, postURL);

      request.Content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");

      using (var client = new HttpClient())
      {
        var response = client.SendAsync(request).GetAwaiter().GetResult();
        return response.Headers.Location.Query;
      }
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
        if (item.Id.Equals(subscription.Id)) {
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

    public static string GetResponseContent(HttpResponseMessage response)
    {
        using (var reader = new StreamReader(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult()))
            return reader.ReadToEnd();
    }

    public static string extractParamFromJson(string keyName, HttpResponseMessage response)
    {
        var param = extractParamFromJson(keyName, GetResponseContent(response));

        return param;
    }

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
        return extractParamFromJson("nonce", response);
    }

    public static string GetNonceForNewCreditCard(BraintreeGateway gateway, Params creditCardDetails, string customerId = null)
    {
        var clientToken = TestHelper.GenerateDecodedClientToken(
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

        return extractParamFromJson("nonce", response);
    }

    public static string GenerateUnlockedNonce(BraintreeGateway gateway, string creditCardNumber, string customerId)
    {
      var clientToken = "";
      if (customerId ==  null) {
        clientToken = TestHelper.GenerateDecodedClientToken(gateway);
      } else {
        clientToken = TestHelper.GenerateDecodedClientToken(gateway, new ClientTokenRequest
          {
            CustomerId = customerId
          }
        );
      }
      var authorizationFingerprint  = extractParamFromJson("authorizationFingerprint", clientToken);
      RequestBuilder builder = new RequestBuilder("");
      builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
        AddTopLevelElement("shared_customer_identifier_type", "testing").
        AddTopLevelElement("shared_customer_identifier", "test-identifier").
        AddTopLevelElement("credit_card[number]", creditCardNumber).
        AddTopLevelElement("share", "true").
        AddTopLevelElement("credit_card[expiration_month]", "11").
        AddTopLevelElement("credit_card[expiration_year]", "2099");

      var response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/credit_cards.json", builder.ToQueryString());
      StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult(), Encoding.UTF8);
      string responseBody = reader.ReadToEnd();

      Regex regex = new Regex("nonce\":\"(?<nonce>[a-f0-9\\-]+)\"");
      Match match = regex.Match(responseBody);
      return match.Groups["nonce"].Value;
    }

    public static string GenerateUnlockedNonce(BraintreeGateway gateway)
    {
      return GenerateUnlockedNonce(gateway, "4111111111111111", null);
    }

    public static string GenerateOneTimePayPalNonce(BraintreeGateway gateway)
    {
        var clientToken = TestHelper.GenerateDecodedClientToken(gateway);
        var authorizationFingerprint  = extractParamFromJson("authorizationFingerprint", clientToken);
        RequestBuilder builder = new RequestBuilder("");
        builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
            AddTopLevelElement("shared_customer_identifier_type", "testing").
            AddTopLevelElement("shared_customer_identifier", "test-identifier").
            AddTopLevelElement("paypal_account[access_token]", "access_token").
            AddTopLevelElement("paypal_account[correlation_id]", System.Guid.NewGuid().ToString()).
            AddTopLevelElement("paypal_account[options][validate]", "false");

        var response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/paypal_accounts", builder.ToQueryString());
        StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult(), Encoding.UTF8);
        string responseBody = reader.ReadToEnd();

        Regex regex = new Regex("nonce\":\"(?<nonce>[a-f0-9\\-]+)\"");
        Match match = regex.Match(responseBody);
        return match.Groups["nonce"].Value;
    }

    public static string GenerateFuturePaymentPayPalNonce(BraintreeGateway gateway)
    {
        var clientToken = TestHelper.GenerateDecodedClientToken(gateway);
        var authorizationFingerprint  = extractParamFromJson("authorizationFingerprint", clientToken);
        RequestBuilder builder = new RequestBuilder("");
        builder.AddTopLevelElement("authorization_fingerprint", authorizationFingerprint).
            AddTopLevelElement("shared_customer_identifier_type", "testing").
            AddTopLevelElement("shared_customer_identifier", "test-identifier").
            AddTopLevelElement("paypal_account[consent_code]", "consent").
            AddTopLevelElement("paypal_account[correlation_id]", System.Guid.NewGuid().ToString()).
            AddTopLevelElement("paypal_account[options][validate]", "false");

        var response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/paypal_accounts", builder.ToQueryString());
        StreamReader reader = new StreamReader(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult(), Encoding.UTF8);
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
          XmlNode node = service.Post("/oauth_testing/grants", new OAuthGrantRequest {
              MerchantId = merchantId,
              Scope = scope
          });

          return node["code"].InnerText;
      }
  }

  public class BraintreeTestHttpService
  {
    public string ApiVersion = "3";

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
        request.Headers.Accept.ParseAdd("application/json");
        request.Headers.UserAgent.ParseAdd("Braintree .NET " + typeof(BraintreeService).GetTypeInfo().Assembly.GetName().Version);

        if (requestBody != null)
        {
            request.Content = new StringContent(requestBody);
        }

        var client = new HttpClient()
        {
            Timeout = TimeSpan.FromMilliseconds(60000)
        };

        return client.SendAsync(request).GetAwaiter().GetResult();
      }

      catch (WebException e)
      {
          throw;
      }
    }
  }
}
