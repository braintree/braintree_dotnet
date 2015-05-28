using System;

namespace Braintree
{
    public class OAuthCredentialsRequest : Request
    {
        public string Code { get; set; }
        public string Scope { get; set; }
        public string GrantType { get; set; }

        public override String ToXml()
        {
            return new RequestBuilder("credentials")
                .AddElement("code", Code)
                .AddElement("scope", Scope)
                .AddElement("grant_type", GrantType)
                .ToXml();
        }
    }
}
