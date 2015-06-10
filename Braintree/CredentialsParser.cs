using Braintree.Exceptions;
using System;

namespace Braintree
{
    public class CredentialsParser
    {
        public Environment Environment;
        public string MerchantId;
        public string AccessToken;
        public string ClientId;
        public string ClientSecret;

        public CredentialsParser(string clientId, string clientSecret)
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

            Environment clientIdEnvironment = GetEnvironment(clientId);
            Environment clientSecretEnvironment = GetEnvironment(clientSecret);

            if (clientIdEnvironment != clientSecretEnvironment) {
                throw new ConfigurationException("Mismatched credential environments: clientId environment is " + clientIdEnvironment + " and clientSecret environment is " + clientSecretEnvironment);
            }

            Environment = clientIdEnvironment;
        }

        public CredentialsParser(string accessToken)
        {
            AccessToken = accessToken;
            Environment = GetEnvironment(accessToken);
            MerchantId = GetMerchantId(accessToken);
        }

        private Environment GetEnvironment(string credential)
        {
            var separators = new Char [] { '$' };
            var parts = credential.Split(separators);
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

        private string GetMerchantId(string accessToken)
        {
            var separators = new Char [] { '$' };
            var parts = accessToken.Split(separators);
            return parts[2];
        }
    }
}
