using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class Environment
    {
        public static Environment DEVELOPMENT = new Environment { GatewayURL = "http://192.168.65.1:3000" };
        public static Environment QA = new Environment { GatewayURL = "https://qa-master.braintreegateway.com" };
        public static Environment SANDBOX = new Environment { GatewayURL = "https://sandbox.braintreegateway.com:443" };
        public static Environment PRODUCTION = new Environment { GatewayURL = "https://www.braintreegateway.com:443" };

        public String GatewayURL { get; protected set; }

        private Environment() { }
    }
}
