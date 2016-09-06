#pragma warning disable 1591

namespace Braintree
{
    public class DiscountsRequest : Request
    {
        public AddDiscountRequest[] Add { get; set; }
        public string[] Remove { get; set; }
        public UpdateDiscountRequest[] Update { get; set; }

        public override string ToXml(string root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(string root)
        {
            return new RequestBuilder(root).
                AddElement("add", Add).
                AddElement("remove", Remove).
                AddElement("update", Update);
        }
    }
}
