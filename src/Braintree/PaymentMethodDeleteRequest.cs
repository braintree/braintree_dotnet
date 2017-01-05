namespace Braintree
{
    public class PaymentMethodDeleteRequest : Request
    {
        public bool? RevokeAllGrants { get; set; }

        public override string ToQueryString()
        {
            return BuildRequest().ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest()
        {
            return new RequestBuilder("").
                AddTopLevelElement("revoke-all-grants", RevokeAllGrants.ToString().ToLower());
        }
    }
}
