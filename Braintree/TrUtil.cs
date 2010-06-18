#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class TrUtil
    {
        public static String BuildTrData(Request request, String redirectURL)
        {
            String dateString = DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss");

            String trContent = new QueryString().
                Append("api_version", Configuration.ApiVersion).
                Append("public_key", Configuration.PublicKey).
                Append("redirect_url", redirectURL).
                Append("time", dateString).
                Append("kind", request.Kind()).
                ToString();

            String requestQueryString = request.ToQueryString();

            if (requestQueryString.Length > 0)
            {
                trContent += "&" + requestQueryString;
            }

            String trHash = new Crypto().HmacHash(Configuration.PrivateKey, trContent);
            return trHash + "|" + trContent;
        }

        public static Boolean IsValidTrQueryString(String queryString)
        {
            string[] delimeters = new string[1];
            delimeters[0] = "&hash=";
            String[] dataSections = queryString.TrimStart('?').Split(delimeters, StringSplitOptions.None);
            return dataSections[1] == new Crypto().HmacHash(Configuration.PrivateKey, dataSections[0]).ToLower();
        }

        public static Boolean IsTrDataValid(String trData)
        {
            String[] dataSections = trData.Split('|');
            String trHash = dataSections[0];
            String trContent = dataSections[1];

            return trHash  == new Crypto().HmacHash(Configuration.PrivateKey, trContent);
        }
    }
}
