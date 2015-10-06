#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using ConfigurationException = Braintree.Exceptions.ConfigurationException;

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

        const string ACCESS_TOKEN = "BRAINTREE.ACCESS_TOKEN";
        const string CLIENT_ID = "BRAINTREE.CLIENT_ID";
        const string CLIENT_SECRET = "BRAINTREE.CLIENT_SECRET";

        public Configuration()
        {
            string accessToken = Environment.GetValue(ACCESS_TOKEN);
            if (accessToken != null)
            {
                CredentialsParser parser = new CredentialsParser(accessToken);
                MerchantId = parser.MerchantId;
                AccessToken = parser.AccessToken;
                Environment = parser.Environment;
            }
            else
            {
                string clientId = Environment.GetValue(CLIENT_ID);
                string clientSecret = Environment.GetValue(CLIENT_SECRET);
                if (clientId != null && ClientSecret != null)
                {
                    CredentialsParser parser = new CredentialsParser(clientId, clientSecret);
                    ClientId = parser.ClientId;
                    ClientSecret = parser.ClientSecret;
                    Environment = parser.Environment;
                }
                else
                {
                    const string msg = "Default constructor requires environment or application configuration for: " +
                        ACCESS_TOKEN + " or (" + CLIENT_ID + " and " + CLIENT_SECRET + ")";
                    throw new ConfigurationException(msg);
                }
            }
        }

        public Configuration(string accessToken)
        {
            CredentialsParser parser = new CredentialsParser(accessToken);
            MerchantId = parser.MerchantId;
            AccessToken = parser.AccessToken;
            Environment = parser.Environment;
        }

        public Configuration(string clientId, string clientSecret)
        {
            CredentialsParser parser = new CredentialsParser(clientId, clientSecret);
            ClientId = parser.ClientId;
            ClientSecret = parser.ClientSecret;
            Environment = parser.Environment;
        }

        const string MERCHANT_ID = "BRAINTREE.MERCHANT_ID";
        const string PUBLIC_KEY = "BRAINTREE.PUBLIC_KEY";
        const string PRIVATE_KEY = "BRAINTREE.PRIVATE_KEY";

        public Configuration(Environment environment)
        {
            if (environment == null) {
                throw new ConfigurationException("Configuration.environment needs to be set");
            }

            string merchantId = Environment.GetValue(MERCHANT_ID);
            if (merchantId == null) {
                throw new ConfigurationException("Environment or application configuration is required for: " + MERCHANT_ID);
            }

            string publicKey = Environment.GetValue(PUBLIC_KEY);
            if (publicKey == null) {
                throw new ConfigurationException("Environment or application configuration is required for: " + PUBLIC_KEY);
            }

            string privateKey = Environment.GetValue(PRIVATE_KEY);
            if (privateKey == null) {
                throw new ConfigurationException("Environment or application configuration is required for: " + PRIVATE_KEY);
            }

            Environment = environment;
            MerchantId = merchantId;
            PublicKey = publicKey;
            PrivateKey = privateKey;
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

        public bool IsClientCredentials
        {
           get { return ClientId != null; }
        }

        public bool IsAccessToken
        {
           get { return AccessToken != null; }
        }

        public void AssertHasClientCredentials()
        {
            if (ClientId == null) {
                throw new ConfigurationException("Missing ClientId when constructing BraintreeGateway");
            }
            if (ClientSecret == null) {
                throw new ConfigurationException("Missing ClientSecret when constructing BraintreeGateway");
            }
            if (AccessToken != null) {
                throw new ConfigurationException("AccessToken must not be passed when constructing BraintreeGateway");
            }
        }

        public void AssertHasAccessTokenOrKeys()
        {
            if (AccessToken == null) {
                if (MerchantId == null) {
                    throw new ConfigurationException("Missing MerchantId (or AccessToken) when constructing BraintreeGateway");
                }
                if (Environment == null) {
                    throw new ConfigurationException("Missing Environment when constructing BraintreeGateway");
                }
                if (PublicKey == null) {
                    throw new ConfigurationException("Missing PublicKey when constructing BraintreeGateway");
                }
                if (PrivateKey == null) {
                    throw new ConfigurationException("Missing PrivateKey when constructing BraintreeGateway");
                }
            }
            if (ClientId != null) {
                throw new ConfigurationException("ClientId must not be passed when constructing BraintreeGateway");
            }
            if (ClientSecret != null) {
                throw new ConfigurationException("ClientSecret must not be passed when constructing BraintreeGateway");
            }
        }
    }
}
