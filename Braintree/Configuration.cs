using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class Configuration
    {
        public static Environment Environment { get; set; }
        public static String MerchantID { get; set; }
        public static String PublicKey { get; set; }
        public static String PrivateKey { get; set; }

        private Configuration() { }

        public static String BaseMerchantURL()
        {
            return Environment.GatewayURL + "/merchants/" + MerchantID;
        }

        public static String GetAuthorizationHeader()
        {
            return "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(PublicKey + ":" + PrivateKey)).Trim();

        }
    }
}
