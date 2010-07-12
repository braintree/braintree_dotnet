#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class Environment
    {
        public static Environment DEVELOPMENT = new Environment(DevelopmentUrl());
        public static Environment QA = new Environment("https://qa-master.braintreegateway.com");
        public static Environment SANDBOX = new Environment("https://sandbox.braintreegateway.com:443");
        public static Environment PRODUCTION = new Environment("https://www.braintreegateway.com:443");

        public String GatewayURL { get; protected set; }

        public Environment(String url)
		{
			GatewayURL = url;
		}

        private static String DevelopmentUrl()
        {
            var host = System.Environment.GetEnvironmentVariable("GATEWAY_HOST") ?? "localhost";
            var port = System.Environment.GetEnvironmentVariable("GATEWAY_PORT") ?? "3000";

            return String.Format("http://{0}:{1}", host, port);
        }
    }
}
