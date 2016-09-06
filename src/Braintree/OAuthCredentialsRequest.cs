namespace Braintree
{
    public class OAuthCredentialsRequest : Request
    {
        public string Code { get; set; }
        public string Scope { get; set; }
        public string GrantType { get; set; }
        public string RefreshToken { get; set; }

        public override string ToXml()
        {
            return new RequestBuilder("credentials")
                .AddElement("code", Code)
                .AddElement("refresh_token", RefreshToken)
                .AddElement("scope", Scope)
                .AddElement("grant_type", GrantType)
                .ToXml();
        }
    }
}
