#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;

namespace Braintree
{
    public class Environment
    {
        public static Environment DEVELOPMENT = new Environment("development");
        public static Environment QA = new Environment("qa");
        public static Environment SANDBOX = new Environment("sandbox");
        public static Environment PRODUCTION = new Environment("production");

        private String environmentName;

        public String GatewayURL
        {
            get
            {
                switch (environmentName)
                {
                    case "development":
                        return DevelopmentUrl();
                    case "qa":
                        return "https://qa-master.braintreegateway.com";
                    case "sandbox":
                        return "https://sandbox.braintreegateway.com:443";
                    case "production":
                        return "https://www.braintreegateway.com:443";
                    default:
                        throw new Exception("Unsupported environment.");
                }
            }
        }

        private Environment(String name)
        {
            environmentName = name;
        }

        private static String DevelopmentUrl()
        {
            var host = System.Environment.GetEnvironmentVariable("GATEWAY_HOST") ?? "localhost";
            var port = System.Environment.GetEnvironmentVariable("GATEWAY_PORT") ?? "3000";

            return String.Format("http://{0}:{1}", host, port);
        }
    }
}
