using System;
using System.Collections.Generic;
using System.Text;
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

  }

  public class BraintreeTestHttpService
  {
    public String ApiVersion = "3";

    public HttpWebResponse Get(string URL)
    {
      return GetXmlResponse(URL, "GET", null);
    }

    public HttpWebResponse Post(string URL, String requestBody)
    {
      return GetXmlResponse(URL, "POST", requestBody);
    }

    private HttpWebResponse GetXmlResponse(String Path, String method, String requestBody)
    {
      try
      {
        var request = WebRequest.Create(Environment.DEVELOPMENT.GatewayURL + "/client_api/" + Path) as HttpWebRequest;
        request.Headers.Add("X-ApiVersion", ApiVersion);
        request.Headers.Add("Accept-Encoding", "gzip");
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
        response.Close();
        return response;
      }
      catch (WebException e)
      {
        var response = (HttpWebResponse)e.Response;
        if (response == null) throw e;
        return response;
      }
    }

    private Stream GetResponseStream(HttpWebResponse response)
    {
      var stream = response.GetResponseStream();
      if (response.ContentEncoding.Equals("gzip", StringComparison.CurrentCultureIgnoreCase))
      {
        stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
      }
      return stream;
    }
  }
}
