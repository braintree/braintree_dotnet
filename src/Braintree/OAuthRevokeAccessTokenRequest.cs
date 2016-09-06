namespace Braintree
{
    public class OAuthRevokeAccessTokenRequest : Request
    {
        public string Token { get; set; }

        public override string ToXml()
        {
            return new RequestBuilder("credentials")
                .AddElement("token", Token)
                .ToXml();
        }
    }
}
