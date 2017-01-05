namespace Braintree
{
    public class MerchantAccountCreateForCurrencyRequest : Request
    {
        public string Currency { get; set; }
        public string Id { get; set; }

        public override string ToXml()
        {
            return new RequestBuilder("merchant_account")
                .AddElement("currency", Currency)
                .AddElement("id", Id)
                .ToXml();
        }
    }
}
