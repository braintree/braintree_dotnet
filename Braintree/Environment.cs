#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class Environment
    {
        public static Environment DEVELOPMENT = new Environment("http://localhost:3000");
        public static Environment QA = new Environment("https://qa-master.braintreegateway.com");
        public static Environment SANDBOX = new Environment("https://sandbox.braintreegateway.com:443");
        public static Environment PRODUCTION = new Environment("https://www.braintreegateway.com:443");

        public String GatewayURL { get; protected set; }

        private Environment(String url)
		{
			GatewayURL = url;
		}
    }
}
