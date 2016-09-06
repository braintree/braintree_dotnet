namespace Braintree
{
    public class OAuthConnectUrlRequest : Request
    {
        public string MerchantId { get; set; }
        public string RedirectUri { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
        public string ClientId { get; set; }
        public string LandingPage { get; set; }
        public string[] PaymentMethods { get; set; }
        public OAuthConnectUrlUserRequest User { get; set; }
        public OAuthConnectUrlBusinessRequest Business { get; set; }

        public OAuthConnectUrlRequest()
        {
            PaymentMethods = new string[0];
        }

        public override string ToQueryString()
        {
            var builder = new RequestBuilder();
            builder.AddTopLevelElement("merchant_id", MerchantId);
            builder.AddTopLevelElement("redirect_uri", RedirectUri);
            builder.AddTopLevelElement("scope", Scope);
            builder.AddTopLevelElement("state", State);
            builder.AddTopLevelElement("landing_page", LandingPage);
            builder.AddTopLevelElement("client_id", ClientId);

            foreach (var paymentMethod in PaymentMethods)
            {
                builder.AddTopLevelElement("payment_methods[]", paymentMethod);
            }

            builder.AddElement("user", User);
            builder.AddElement("business", Business);
            return builder.ToQueryString();
        }
    }
}
