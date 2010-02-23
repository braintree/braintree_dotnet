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
                Append("api_version", "1").
                Append("public_key", Configuration.PublicKey).
                Append("redirect_url", redirectURL).
                Append("time", dateString).
                ToString();

            String requestQueryString = request.ToQueryString();

            if (requestQueryString.Length > 0)
            {
                trContent += "&" + requestQueryString;
            }

            String trHash = new Crypto().HmacHash(Configuration.PrivateKey, trContent);
            return trHash + "|" + trContent;
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
