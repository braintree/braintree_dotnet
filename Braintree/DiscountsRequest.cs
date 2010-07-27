using System;
namespace Braintree
{
    public class DiscountsRequest : Request
    {
        public AddDiscountRequest[] Add { get; set; }
        public String[] Remove { get; set; }
        public UpdateDiscountRequest[] Update { get; set; }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("add", Add).
                AddElement("remove", Remove).
                AddElement("update", Update);
        }
    }
}
