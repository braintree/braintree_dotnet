using System;
using System.Xml;
using System.Security.Cryptography;
using System.Text;

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

        public string ConnectUrl(OAuthConnectUrlRequest request)
        {
            request.ClientId = Gateway.ClientId;
            string queryString = request.ToQueryString();
            string url = Gateway.Environment.GatewayURL+"/oauth/connect?"+queryString;
            return url+"&signature="+ComputeSignature(url)+"&algorithm=SHA256";
        }

        private string ComputeSignature(string message)
        {
            byte[] key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Gateway.ClientSecret));
            byte[] signatureBytes = new HMACSHA256(key).ComputeHash(Encoding.UTF8.GetBytes(message));
            return BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();
        }
    }
}
