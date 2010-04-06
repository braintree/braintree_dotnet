using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using NUnit.Framework;

namespace Braintree.Tests
{
    public class TestHelper
    {
        public static String QueryStringForTR(Request trParams, Request req, String postURL)
        {
            String trData = TrUtil.BuildTrData(trParams, "http://example.com");
            String postData = "tr_data=" + HttpUtility.UrlEncode(trData, Encoding.UTF8) + "&";
            postData += req.ToQueryString();

            var request = WebRequest.Create(postURL) as HttpWebRequest;
            request.Headers.Add("X-ApiVersion", "1");
            request.UserAgent = "Braintree .NET Tests";

            request.Method = "POST";
            request.KeepAlive = false;

            byte[] buffer = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = buffer.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();

            var response = request.GetResponse() as HttpWebResponse;
            String query = response.ResponseUri.Query;

            response.Close();

            return query;
        }

        public static void AreDatesEqual(DateTime expected, DateTime actual)
        {
            Assert.AreEqual(expected.Day, actual.Day);
            Assert.AreEqual(expected.Month, actual.Month);
            Assert.AreEqual(expected.Year, actual.Year);
        }

        public static Boolean IncludesOnAnyPage(PagedCollection<Subscription> collection, Subscription subscription)
        {
            foreach (Subscription item in collection.Items)
            {
                if (item.Id.Equals(subscription.Id))
                {
                    return true;
                }
            }

            if (collection.IsLastPage()) {
                return false;
            }

            return IncludesOnAnyPage(collection.GetNextPage(), subscription);
        }

        public static PagedCollection<T> MockPagedCollection<T>(int currentPageNumber, int totalItems, int pageSize)
        {
            return new PagedCollection<T>(new List<T>(), currentPageNumber, totalItems, pageSize, delegate() {
                return MockPagedCollection<T>(currentPageNumber + 1, totalItems, pageSize);
            });
        }
    }
}
