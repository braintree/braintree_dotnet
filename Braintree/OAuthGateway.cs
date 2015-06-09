using System.Xml;

namespace Braintree
{
    public class OAuthGateway
    {
        private BraintreeService Service;
        private BraintreeGateway Gateway;

        public OAuthGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasClientCredentials();
            Gateway = gateway;
            Service = new BraintreeService(gateway.Configuration);
        }

        public ResultImpl<OAuthCredentials> CreateTokenFromCode(OAuthCredentialsRequest request)
        {
            request.GrantType = "authorization_code";
            XmlNode accessTokenXML = Service.Post("/oauth/access_tokens", request);

            return new ResultImpl<OAuthCredentials>(new NodeWrapper(accessTokenXML), Gateway);
        }

        public ResultImpl<OAuthCredentials> CreateTokenFromRefreshToken(OAuthCredentialsRequest request)
        {
            request.GrantType = "refresh_token";
            XmlNode accessTokenXML = Service.Post("/oauth/access_tokens", request);

            return new ResultImpl<OAuthCredentials>(new NodeWrapper(accessTokenXML), Gateway);
        }
    }
}
