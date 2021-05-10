namespace Braintree
{
    public class TaxIdentifierRequest: Request
    {
        public string CountryCode { get; set; }
        public string Identifier { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        public override string ToQueryString(string root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root)
                .AddElement("countryCode", CountryCode)
                .AddElement("identifier", Identifier);
        }
    }
}
