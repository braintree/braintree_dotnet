using Braintree.Exceptions;
using System;

namespace Braintree
{
    public class CredentialsParser
    {
        public Environment Environment;
        public string ClientId;
        public string ClientSecret;

        public CredentialsParser(string clientId, string clientSecret, string accessToken)
        {
            if (clientId.StartsWith("client_id")) {
                ClientId = clientId;
            } else {
                throw new ConfigurationException("Value passed for clientId is not a clientId");
            }

            if (clientSecret.StartsWith("client_secret")) {
                ClientSecret = clientSecret;
            } else {
                throw new ConfigurationException("Value passed for clientSecret is not a clientSecret");
            }

            Environment clientIdEnvironment = getEnvironment(clientId);
            Environment clientSecretEnvironment = getEnvironment(clientSecret);

            if (clientIdEnvironment != clientSecretEnvironment) {
                throw new ConfigurationException("Mismatched credential environments: clientId environment is " + clientIdEnvironment + " and clientSecret environment is " + clientSecretEnvironment);
            }

            Environment = clientIdEnvironment;
        }

        private Environment getEnvironment(string credential)
        {
            char [] separators = new Char [] { '$' };
            string[] parts = credential.Split(separators);
            switch (parts[1]) {
                case "development":
                    return Environment.DEVELOPMENT;
                case "qa":
                    return Environment.QA;
                case "sandbox":
                    return Environment.SANDBOX;
                case "production":
                    return Environment.PRODUCTION;
                default:
                    throw new Exception("Unsupported environment.");
            }
        }
    }
}
