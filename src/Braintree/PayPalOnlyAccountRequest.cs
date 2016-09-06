namespace Braintree
{
    public class PayPalOnlyAccountRequest : Request
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public override string ToXml(string root)
        {
            return new RequestBuilder(root)
                .AddElement("client_id", ClientId)
                .AddElement("client_secret", ClientSecret)
                .ToXml();
        }
    }
}
