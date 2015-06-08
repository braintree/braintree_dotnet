#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using Braintree.Exceptions;

namespace Braintree
{
    public class Configuration
    {
        public Environment Environment { get; set; }
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }

        public Configuration() {}
        public Configuration(string accessToken) {
            CredentialsParser parser = new CredentialsParser(accessToken);
            MerchantId = parser.MerchantId;
            AccessToken = parser.AccessToken;
            Environment = parser.Environment;
        }

        public Configuration(string clientId, string clientSecret, string accessToken = null)
        {
            CredentialsParser parser = new CredentialsParser(clientId, clientSecret, accessToken);
            ClientId = parser.ClientId;
            ClientSecret = parser.ClientSecret;
            Environment = parser.Environment;
        }

        public Configuration(Environment environment, string merchantId, string publicKey, string privateKey)
        {
            if (environment == null) {
                throw new ConfigurationException("Configuration.environment needs to be set");
            }
            else if (merchantId == null) {
                throw new ConfigurationException("Configuration.merchantId needs to be set");
            }
            else if (publicKey == null) {
                throw new ConfigurationException("Configuration.publicKey needs to be set");
            }
            else if (privateKey == null) {
                throw new ConfigurationException("Configuration.privateKey needs to be set");
            }

            Environment = environment;
            MerchantId = merchantId;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public bool IsClientCredentials()
        {
           return ClientId != null;
        }

        public bool IsAccessToken()
        {
           return AccessToken != null;
        }
    }
}
