#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class TrUtil
    {
        public static string BuildTrData(Request request, string redirectURL, BraintreeService service)
        {
            var dateString = DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss");

            var trContent = new QueryString().
                Append("api_version", service.ApiVersion).
                Append("public_key", service.PublicKey).
                Append("redirect_url", redirectURL).
                Append("time", dateString).
                Append("kind", request.Kind()).
                ToString();

            string requestQueryString = request.ToQueryString();

            if (requestQueryString.Length > 0)
            {
                trContent += "&" + requestQueryString;
            }

            var signatureService = new SignatureService {
              Key = service.PrivateKey,
              Hasher = new Sha1Hasher()
            };
            return signatureService.Sign(trContent);
        }

        public static bool IsValidTrQueryString(string queryString, BraintreeService service)
        {
            var delimiters = new string[1];
            delimiters[0] = "&hash=";
            string[] dataSections = queryString.TrimStart('?').Split(delimiters, StringSplitOptions.None);
            return dataSections[1] == new Sha1Hasher().HmacHash(service.PrivateKey, dataSections[0]).ToLower();
        }

        public static bool IsTrDataValid(string trData, BraintreeService service)
        {
            string[] dataSections = trData.Split('|');
            var trHash = dataSections[0];
            var trContent = dataSections[1];

            return trHash  == new Sha1Hasher().HmacHash(service.PrivateKey, trContent);
        }
    }
}
