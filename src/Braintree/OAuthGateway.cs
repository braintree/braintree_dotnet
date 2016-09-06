using System;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Braintree
{
    public class OAuthGateway : IOAuthGateway
    {
        private readonly BraintreeService service;
        private readonly BraintreeGateway gateway;

        public OAuthGateway(BraintreeGateway gateway)
        {
            gateway.Configuration.AssertHasClientCredentials();
            this.gateway = gateway;
            service = new BraintreeService(gateway.Configuration);
        }

        public ResultImpl<OAuthCredentials> CreateTokenFromCode(OAuthCredentialsRequest request)
        {
            request.GrantType = "authorization_code";
            XmlNode accessTokenXML = service.Post("/oauth/access_tokens", request);

            return new ResultImpl<OAuthCredentials>(new NodeWrapper(accessTokenXML), gateway);
        }

        public ResultImpl<OAuthCredentials> CreateTokenFromRefreshToken(OAuthCredentialsRequest request)
        {
            request.GrantType = "refresh_token";
            XmlNode accessTokenXML = service.Post("/oauth/access_tokens", request);

            return new ResultImpl<OAuthCredentials>(new NodeWrapper(accessTokenXML), gateway);
        }

        public ResultImpl<OAuthResult> RevokeAccessToken(string accessToken)
        {
            OAuthRevokeAccessTokenRequest request = new OAuthRevokeAccessTokenRequest();
            request.Token = accessToken;
            XmlNode accessTokenXML = service.Post("/oauth/revoke_access_token", request);

            return new ResultImpl<OAuthResult>(new NodeWrapper(accessTokenXML), gateway);
        }

        public string ConnectUrl(OAuthConnectUrlRequest request)
        {
            request.ClientId = gateway.ClientId;
            string queryString = request.ToQueryString();
            string url = gateway.Environment.GatewayURL+"/oauth/connect?"+queryString;
            return string.Format("{0}&signature={1}&algorithm=SHA256", url, ComputeSignature(url));
        }

        public string ComputeSignature(string message)
        {
            byte[] key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(gateway.ClientSecret));
            byte[] signatureBytes = new HMACSHA256(key).ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();
        }
    }
}
