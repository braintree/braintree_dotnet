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
            string dateString = DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss");

            string trContent = new QueryString().
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

            SignatureService signatureService = new SignatureService {
              Key = service.PrivateKey,
              Hasher = new Sha1Hasher()
            };
            return signatureService.Sign(trContent);
        }

        public static bool IsValidTrQueryString(string queryString, BraintreeService service)
        {
            string[] delimeters = new string[1];
            delimeters[0] = "&hash=";
            string[] dataSections = queryString.TrimStart('?').Split(delimeters, StringSplitOptions.None);
            return dataSections[1] == new Sha1Hasher().HmacHash(service.PrivateKey, dataSections[0]).ToLower();
        }

        public static bool IsTrDataValid(string trData, BraintreeService service)
        {
            string[] dataSections = trData.Split('|');
            string trHash = dataSections[0];
            string trContent = dataSections[1];

            return trHash  == new Sha1Hasher().HmacHash(service.PrivateKey, trContent);
        }
    }
}
