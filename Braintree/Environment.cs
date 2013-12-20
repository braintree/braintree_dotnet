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
                        return "https://api.sandbox.braintreegateway.com:443";
                    case "production":
                        return "https://api.braintreegateway.com:443";
                    default:
                        throw new Exception("Unsupported environment.");
                }
            }
        }

        public String AuthURL
        {
            get
            {
                switch (environmentName)
                {
                    case "development":
                        return "http://auth.venmo.dev:4567";
                    case "qa":
                        return "https://auth.qa.venmo.com";
                    case "sandbox":
                        return "https://auth.sandbox.venmo.com";
                    case "production":
                        return "https://auth.venmo.com";
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
            // Access environment variables lazily to avoid issues on servers where access to environment variables is restricted
            var host = System.Environment.GetEnvironmentVariable("GATEWAY_HOST") ?? "localhost";
            var port = System.Environment.GetEnvironmentVariable("GATEWAY_PORT") ?? "3000";

            return String.Format("http://{0}:{1}", host, port);
        }
    }
}
