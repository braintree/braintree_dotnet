#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class Configuration
    {
        public static Environment Environment { get; set; }
        public static String MerchantId { get; set; }
        public static String PublicKey { get; set; }
        public static String PrivateKey { get; set; }
        public static String ApiVersion = "2";

        private Configuration() { }

        public static String BaseMerchantURL()
        {
            return Environment.GatewayURL + "/merchants/" + MerchantId;
        }

        public static String GetAuthorizationHeader()
        {
            return "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(PublicKey + ":" + PrivateKey)).Trim();

        }
    }
}
