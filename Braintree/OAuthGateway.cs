using System.Xml;

namespace Braintree
{
    public class OAuthGateway
    {
        private BraintreeService Service;

        public OAuthGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasClientCredentials();
            Service = new BraintreeService(gateway.Configuration);
        }

        public ResultImpl<OAuthCredentials> CreateTokenFromCode(OAuthCredentialsRequest request)
        {
            request.GrantType = "authorization_code";
            XmlNode accessTokenXML = Service.Post("/oauth/access_tokens", request);

            return new ResultImpl<OAuthCredentials>(new NodeWrapper(accessTokenXML), Service);
        }

        public ResultImpl<OAuthCredentials> CreateTokenFromRefreshToken(OAuthCredentialsRequest request)
        {
            request.GrantType = "refresh_token";
            XmlNode accessTokenXML = Service.Post("/oauth/access_tokens", request);

            return new ResultImpl<OAuthCredentials>(new NodeWrapper(accessTokenXML), Service);
        }
    }
}
