using Braintree.Exceptions;

namespace Braintree
{
    public class Environment
    {
        public static Environment DEVELOPMENT = new Environment("development", DevelopmentUrl(), "http://auth.venmo.dev:9292");
        public static Environment QA = new Environment("qa", "https://gateway.qa.braintreepayments.com", "https://auth.qa.venmo.com");
        public static Environment SANDBOX = new Environment("sandbox", "https://api.sandbox.braintreegateway.com:443", "https://auth.sandbox.venmo.com");
        public static Environment PRODUCTION = new Environment("production", "https://api.braintreegateway.com:443", "https://auth.venmo.com");

        public string GatewayURL { get; private set; }
        public string AuthURL { get; private set; }
        public string EnvironmentName { get; private set; }

        public Environment(string environmentName, string gatewayUrl, string authUrl)
        {
            this.GatewayURL = gatewayUrl;
            this.AuthURL = authUrl;
            this.EnvironmentName = environmentName;
        }

        private static string DevelopmentUrl()
        {
            // Access environment variables lazily to avoid issues on servers where access to environment variables is restricted
            var host = System.Environment.GetEnvironmentVariable("GATEWAY_HOST") ?? "localhost";
            var port = System.Environment.GetEnvironmentVariable("GATEWAY_PORT") ?? "3000";

            return string.Format("http://{0}:{1}", host, port);
        }

        public static Environment ParseEnvironment(string environment)
        {
            switch (environment.ToLowerInvariant())
            {
                case "integration":
                case "development":
                    return Environment.DEVELOPMENT;
                case "qa":
                    return Environment.QA;
                case "sandbox":
                    return Environment.SANDBOX;
                case "production":
                    return Environment.PRODUCTION;
                default:
                    throw new ConfigurationException("Unsupported environment: " + environment);
            }
        }
    }
}
