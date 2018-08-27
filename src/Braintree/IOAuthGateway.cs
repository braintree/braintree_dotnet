#pragma warning disable 1591

namespace Braintree
{
    public interface IOAuthGateway
    {
        string ConnectUrl(OAuthConnectUrlRequest request);
        ResultImpl<OAuthCredentials> CreateTokenFromCode(OAuthCredentialsRequest request);
        ResultImpl<OAuthCredentials> CreateTokenFromRefreshToken(OAuthCredentialsRequest request);
        ResultImpl<OAuthResult> RevokeAccessToken(string token);
    }
}
