#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class TrUtil
    {
        public static String BuildTrData(Request request, String redirectURL, BraintreeService service)
        {
            String dateString = DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss");

            String trContent = new QueryString().
                Append("api_version", service.ApiVersion).
                Append("public_key", service.PublicKey).
                Append("redirect_url", redirectURL).
                Append("time", dateString).
                Append("kind", request.Kind()).
                ToString();

            String requestQueryString = request.ToQueryString();

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

        public static Boolean IsValidTrQueryString(String queryString, BraintreeService service)
        {
            string[] delimeters = new string[1];
            delimeters[0] = "&hash=";
            String[] dataSections = queryString.TrimStart('?').Split(delimeters, StringSplitOptions.None);
            return dataSections[1] == new Sha1Hasher().HmacHash(service.PrivateKey, dataSections[0]).ToLower();
        }

        public static Boolean IsTrDataValid(String trData, BraintreeService service)
        {
            String[] dataSections = trData.Split('|');
            String trHash = dataSections[0];
            String trContent = dataSections[1];

            return trHash  == new Sha1Hasher().HmacHash(service.PrivateKey, trContent);
        }
    }
}
