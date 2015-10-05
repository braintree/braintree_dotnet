using System;

namespace Braintree
{
    public interface IOAuthGateway
    {
        string ComputeSignature(string message);
        string ConnectUrl(OAuthConnectUrlRequest request);
        ResultImpl<OAuthCredentials> CreateTokenFromCode(OAuthCredentialsRequest request);
        ResultImpl<OAuthCredentials> CreateTokenFromRefreshToken(OAuthCredentialsRequest request);
    }
}