using Braintree.Exceptions;

namespace Braintree
{
    public class Environment
    {
        public static Environment DEVELOPMENT = new Environment("development", DevelopmentUrl(), "http://auth.venmo.dev:9292", "http://graphql.bt.local:8080/graphql");
        public static Environment QA = new Environment("qa", "https://gateway.qa.braintreepayments.com", "https://auth.qa.venmo.com", "https://payments-qa.dev.braintree-api.com/graphql");
        public static Environment SANDBOX = new Environment("sandbox", "https://api.sandbox.braintreegateway.com:443", "https://auth.sandbox.venmo.com", "https://payments.sandbox.braintree-api.com/graphql");
        public static Environment PRODUCTION = new Environment("production", "https://api.braintreegateway.com:443", "https://auth.venmo.com", "https://payments.braintree-api.com/graphql");

        public string GatewayURL { get; private set; }
        public string AuthURL { get; private set; }
        public string EnvironmentName { get; private set; }
        public string GraphQLUrl { get; private set; }

        public Environment(string environmentName, string gatewayUrl, string authUrl, string graphQLUrl)
        {
            this.GatewayURL = gatewayUrl;
            this.AuthURL = authUrl;
            this.EnvironmentName = environmentName;
            this.GraphQLUrl = graphQLUrl;
        }

        private static string DevelopmentUrl()
        {
            // Access environment variables lazily to avoid issues on servers where access to environment variables is restricted
            var host = System.Environment.GetEnvironmentVariable("GATEWAY_HOST") ?? "localhost";
            var port = System.Environment.GetEnvironmentVariable("GATEWAY_PORT") ?? "3000";

            return $"http://{host}:{port}";
        }

        /// <summary>
        /// Generates a configured Environment for use in a Braintree Gateway; targeted by Environment name.
        /// </summary>
        /// <param name="environment">The name of the target Environment (not case-sensitive)</param>
        /// <returns>A new configured instance of a Braintree Environment</returns>
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
