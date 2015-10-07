#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
//using ConfigurationException = Braintree.Exceptions.ConfigurationException;

namespace Braintree
{
    public class Environment
    {
        public static readonly Environment DEVELOPMENT = new Environment("development", DevelopmentUrl(), "http://auth.venmo.dev:9292");
        public static readonly Environment QA = new Environment("qa", "https://gateway.qa.braintreepayments.com", "https://auth.qa.venmo.com");
        public static readonly Environment SANDBOX = new Environment("sandbox", "https://api.sandbox.braintreegateway.com:443", "https://auth.sandbox.venmo.com");
        public static readonly Environment PRODUCTION = new Environment("production", "https://api.braintreegateway.com:443", "https://auth.venmo.com");

        const string ENVIRONMENT = "BRAINTREE.ENVIRONMENT";
        const string ENVIRONMENT_OLD = "ENVIRONMENT";
        public static Environment CONFIGURED
        {
            get
            {
                string env = GetValue(ENVIRONMENT);
                if (env == null)
                    env = GetValue(ENVIRONMENT_OLD);
                if (env != null)
                    env = env.ToLower().Trim();

                switch(env)
                {
                    case "development":
                        return DEVELOPMENT;
                    case "qa":
                        return QA;
                    case "production":
                        return PRODUCTION;
                    case "sandbox": //fall back to sandbox
                    default:
                        return SANDBOX;
                }
            }
        }

        readonly string gatewayURL;
        public string GatewayURL { get { return gatewayURL; } }

        readonly string authURL;
        public string AuthURL { get { return authURL; } }

        readonly string environmentName;
        public string EnvironmentName { get { return environmentName; } }

        public Environment(string environmentName, string gatewayUrl, string authUrl)
        {
            this.gatewayURL = gatewayUrl;
            this.authURL = authUrl;
            this.environmentName = environmentName;
        }

        const string GATEWAY_HOST = "BRAINTREE.GATEWAY_HOST";
        const string GATEWAY_PORT = "BRAINTREE.GATEWAY_PORT";

        private static string DevelopmentUrl()
        {
            // Access environment variables lazily to avoid issues on servers where access to environment variables is restricted
            string host = GetValue(GATEWAY_HOST, "localhost");
            string port = GetValue(GATEWAY_PORT, "3000");

            return string.Format("http://{0}:{1}", host, port);
        }

        /// <summary>
        /// Get a configured setting variable.
        /// Will fetch first from application (or web) configuration first then fall back to environment variable.
        /// </summary>
        /// <param name="name">Key name for application configuration or environment variable</param>
        /// <param name="defaultValue">If provided, will be returned when value is null</param>
        /// <returns>Configured setting string value</returns>
        internal static string GetValue(string name, string defaultValue = null)
        {
            if (string.IsNullOrEmpty("name"))
                throw new ArgumentException("Name can not be null or empty.");

            string v = ConfigurationManager.AppSettings.Get(name);
            if (v == null)
                v = System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User);
            if (v == null)
                v = System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            if (v == null)
                v = System.Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
            if (v == null)
                v = defaultValue;

            return v;
        }
    }
}
