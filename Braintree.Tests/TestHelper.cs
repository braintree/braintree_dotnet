using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Xml;
using NUnit.Framework;
using Braintree;

using Params = System.Collections.Generic.Dictionary<string, object>;

namespace Braintree.Tests
{
  public class TestHelper
  {

    public static String GenerateDecodedClientToken(BraintreeGateway gateway, ClientTokenRequest request = null)
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

    public static String QueryStringForTR(Request trParams, Request req, String postURL, BraintreeService service)
    {
      String trData = TrUtil.BuildTrData(trParams, "http://example.com", service);
      String postData = "tr_data=" + HttpUtility.UrlEncode(trData, Encoding.UTF8) + "&";
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
      String query = new Uri(response.GetResponseHeader("Location")).Query;

      response.Close();

      return query;
    }

    public static void AreDatesEqual(DateTime expected, DateTime actual)
    {
      Assert.AreEqual(expected.Day, actual.Day);
      Assert.AreEqual(expected.Month, actual.Month);
      Assert.AreEqual(expected.Year, actual.Year);
    }

    public static void AssertIncludes(String expected, String all)
    {
      Assert.IsTrue(all.IndexOf(expected) >= 0, "Expected:\n" + all + "\nto include:\n" + expected);
    }

    public static Boolean IncludesSubscription(ResourceCollection<Subscription> collection, Subscription subscription)
    {
      foreach (Subscription item in collection)
      {
        if (item.Id.Equals(subscription.Id)) {
          return true;
        }
      }
      return false;
    }

    public static NodeWrapper Settle(BraintreeService service, String transactionId)
    {
      NodeWrapper response = new NodeWrapper(service.Put(service.MerchantPath() + "/transactions/" + transactionId + "/settle"));
      Assert.IsTrue(response.IsSuccess());
      return response;
    }

    public static void SettlementDecline(BraintreeService service, String transactionId)
    {
      NodeWrapper response = new NodeWrapper(service.Put(service.MerchantPath() + "/transactions/" + transactionId + "/settlement_decline"));
      Assert.IsTrue(response.IsSuccess());
    }

    public static void SettlementPending(BraintreeService service, String transactionId)
    {
      NodeWrapper response = new NodeWrapper(service.Put(service.MerchantPath() + "/transactions/" + transactionId + "/settlement_pending"));
      Assert.IsTrue(response.IsSuccess());
    }

    public static void Escrow(BraintreeService service, String transactionId)
    {
      NodeWrapper response = new NodeWrapper(service.Put(service.MerchantPath() + "/transactions/" + transactionId + "/escrow"));
      Assert.IsTrue(response.IsSuccess());
    }

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

    public static string extractParamFromJson(String keyName, String json)
    {
      String regex = string.Format("\"{0}\":\\s?\"([^\"]+)\"", keyName);
      Match match = Regex.Match(json, regex);
      string keyValue = match.Groups[1].Value;

      return keyValue;
    }

    public static int extractIntParamFromJson(String keyName, String json)
    {
      String regex = string.Format("\"{0}\":\\s?(\\d+)", keyName);
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

    public static String GetNonceForNewCreditCard(BraintreeGateway gateway, Params creditCardDetails, string customerId = null)
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

    public static String GenerateUnlockedNonce(BraintreeGateway gateway, String creditCardNumber, String customerId)
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

      HttpWebResponse response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/credit_cards.json", builder.ToQueryString());
      StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
      String responseBody = reader.ReadToEnd();

      Regex regex = new Regex("nonce\":\"(?<nonce>[a-f0-9\\-]+)\"");
      Match match = regex.Match(responseBody);
      return match.Groups["nonce"].Value;
    }

    public static String GenerateUnlockedNonce(BraintreeGateway gateway)
    {
      return GenerateUnlockedNonce(gateway, "4111111111111111", null);
    }

    public static String GenerateOneTimePayPalNonce(BraintreeGateway gateway)
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

        HttpWebResponse response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/paypal_accounts", builder.ToQueryString());
        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        String responseBody = reader.ReadToEnd();

        Regex regex = new Regex("nonce\":\"(?<nonce>[a-f0-9\\-]+)\"");
        Match match = regex.Match(responseBody);
        return match.Groups["nonce"].Value;
    }

    public static String GenerateFuturePaymentPayPalNonce(BraintreeGateway gateway)
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

        HttpWebResponse response = new BraintreeTestHttpService().Post(gateway.MerchantId, "v1/payment_methods/paypal_accounts", builder.ToQueryString());
        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        String responseBody = reader.ReadToEnd();

        Regex regex = new Regex("nonce\":\"(?<nonce>[a-f0-9\\-]+)\"");
        Match match = regex.Match(responseBody);
        return match.Groups["nonce"].Value;
    }

    public static String Create3DSVerification(BraintreeService service, String merchantAccountId, ThreeDSecureRequestForTests request)
    {
      String url = "/three_d_secure/create_verification/" + merchantAccountId;
      NodeWrapper response = new NodeWrapper(service.Post(service.MerchantPath() + url, request));
      Assert.IsTrue(response.IsSuccess());
      return response.GetString("three-d-secure-token");
    }
  }

  public class OAuthTestHelper
  {
      private class OAuthGrantRequest : Request
      {
          public String MerchantId { get; set; }
          public String Scope { get; set; }

          public override String ToXml()
          {
              return new RequestBuilder("grant")
                  .AddElement("merchant_public_id", MerchantId)
                  .AddElement("scope", Scope)
                  .ToXml();
          }
      }

      public static String CreateGrant(BraintreeGateway gateway, String merchantId, string scope)
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
    public String ApiVersion = "3";

    public HttpWebResponse Get(String MerchantId, String URL)
    {
      return GetJsonResponse(MerchantId, URL, "GET", null);
    }

    public HttpWebResponse Post(String MerchantId, String URL, String requestBody)
    {
      return GetJsonResponse(MerchantId, URL, "POST", requestBody);
    }

    private HttpWebResponse GetJsonResponse(String MerchantId, String Path, String method, String requestBody)
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
  }
}
