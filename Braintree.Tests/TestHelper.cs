using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;
using System.IO;
using System.IO.Compression;
using NUnit.Framework;
using Braintree;

namespace Braintree.Tests
{
  public class TestHelper
  {

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

    public static void Settle(BraintreeService service, String transactionId)
    {
      NodeWrapper response = new NodeWrapper(service.Put("/transactions/" + transactionId + "/settle"));
      Assert.IsTrue(response.IsSuccess());
    }

    public static void Escrow(BraintreeService service, String transactionId)
    {
      NodeWrapper response = new NodeWrapper(service.Put("/transactions/" + transactionId + "/escrow"));
      Assert.IsTrue(response.IsSuccess());
    }

    public static DateTime NowInEastern()
    {
      return DateTime.UtcNow - new TimeSpan(05, 00, 00);
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

    public static String GenerateUnlockedNonce(BraintreeGateway gateway, String creditCardNumber, String customerId)
    {
      var clientToken = "";
      if (customerId ==  null) {
        clientToken = gateway.ClientToken.generate();
      } else {
        clientToken = gateway.ClientToken.generate(new ClientTokenRequest
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

      HttpWebResponse response = new BraintreeTestHttpService().Post(gateway.MerchantId, "nonces.json", builder.ToQueryString());
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

    public static String Create3DSVerification(BraintreeService service, String merchantAccountId, ThreeDSecureRequestForTests request)
    {
      String url = "/three_d_secure/create_verification/" + merchantAccountId;
      NodeWrapper response = new NodeWrapper(service.Post(url, request));
      Assert.IsTrue(response.IsSuccess());
      return response.GetString("three-d-secure-token");
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
