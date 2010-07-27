using System;
namespace Braintree
{
    public class DiscountsRequest : Request
    {
        public UpdateDiscountRequest[] Update { get; set; }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("update", Update);
        }
    }
}
