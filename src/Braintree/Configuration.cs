using Braintree.Exceptions;
using System.Net;
#if netcore
using System.Net.Http;
#endif

namespace Braintree
{
#if netcore
    public delegate HttpRequestMessage HttpRequestMessageFactory(HttpMethod method, string requestUriString);
#else
    public delegate HttpWebRequest HttpWebRequestFactory(string requestUriString);
#endif

    public class Configuration
    {
        private Environment _Environment;
        public Environment Environment {
            get => this._Environment;
            set {
                if (this.Environment != null && this.AccessToken != null && this.Environment != value) {
                    throw new ConfigurationException("AccessToken Environment does not match Environment passed in Config");
                }
                this._Environment = value;
            }
        }
        private string _AccessToken;
        public string AccessToken {
            get => this._AccessToken;
            set {
                CredentialsParser parser = new CredentialsParser(value);
                if (this.Environment != null && parser.Environment != this.Environment) {
                    throw new ConfigurationException("AccessToken Environment does not match Environment passed in Config");
                }
                this.MerchantId = parser.MerchantId;
                this._AccessToken = parser.AccessToken;
                this.Environment = parser.Environment;
            }
        }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string MerchantId { get; set; }
        public string PrivateKey { get; set; }
        public IWebProxy WebProxy { get; set; }
        public string PublicKey { get; set; }
#if netcore
        public HttpRequestMessageFactory HttpRequestMessageFactory { get; set; }
#else
        public HttpWebRequestFactory HttpWebRequestFactory { get; set; }
#endif

        private int timeout;
        public int Timeout
        {
            get => timeout == 0 ? 60000 : timeout;
            set => timeout = value;
        }

        public Configuration()
        {
#if netcore
            HttpRequestMessageFactory = (method, requestUriString) => new HttpRequestMessage(method, requestUriString);
#else
            HttpWebRequestFactory = requestUriString => WebRequest.Create(requestUriString) as HttpWebRequest;
#endif
        }

        public Configuration(string accessToken) : this()
        {
            AccessToken = accessToken;
        }

        public Configuration(string clientId, string clientSecret) : this()
        {
            CredentialsParser parser = new CredentialsParser(clientId, clientSecret);
            ClientId = parser.ClientId;
            ClientSecret = parser.ClientSecret;
            Environment = parser.Environment;
        }

        public Configuration(Environment environment, string merchantId, string publicKey, string privateKey) : this()
        {
            if (environment == null)
            {
                throw new ConfigurationException("Configuration.environment needs to be set");
            }
            else if (string.IsNullOrEmpty(merchantId))
            {
                throw new ConfigurationException("Configuration.merchantId needs to be set");
            }
            else if (string.IsNullOrEmpty(publicKey))
            {
                throw new ConfigurationException("Configuration.publicKey needs to be set");
            }
            else if (string.IsNullOrEmpty(privateKey))
            {
                throw new ConfigurationException("Configuration.privateKey needs to be set");
            }

            Environment = environment;
            MerchantId = merchantId;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public Configuration(string environment, string merchantId, string publicKey, string privateKey) :
            this(Environment.ParseEnvironment(environment), merchantId, publicKey, privateKey)
        {
        }

        public bool IsClientCredentials => ClientId != null;

        public bool IsAccessToken => AccessToken != null;

        public void AssertHasClientCredentials()
        {
            if (ClientId == null)
            {
                throw new ConfigurationException("Missing ClientId when constructing BraintreeGateway");
            }
            if (ClientSecret == null)
            {
                throw new ConfigurationException("Missing ClientSecret when constructing BraintreeGateway");
            }
            if (AccessToken != null)
            {
                throw new ConfigurationException("AccessToken must not be passed when constructing BraintreeGateway");
            }
        }

        public void AssertHasAccessTokenOrKeys()
        {
            if (AccessToken == null)
            {
                if (MerchantId == null)
                {
                    throw new ConfigurationException("Missing MerchantId (or AccessToken) when constructing BraintreeGateway");
                }
                if (Environment == null)
                {
                    throw new ConfigurationException("Missing Environment when constructing BraintreeGateway");
                }
                if (PublicKey == null)
                {
                    throw new ConfigurationException("Missing PublicKey when constructing BraintreeGateway");
                }
                if (PrivateKey == null)
                {
                    throw new ConfigurationException("Missing PrivateKey when constructing BraintreeGateway");
                }
            }
            if (ClientId != null)
            {
                throw new ConfigurationException("ClientId must not be passed when constructing BraintreeGateway");
            }
            if (ClientSecret != null)
            {
                throw new ConfigurationException("ClientSecret must not be passed when constructing BraintreeGateway");
            }
        }

        public string GetGraphQLUrl()
        {
            return Environment.GraphQLUrl;
        }
    }
}
