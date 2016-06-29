#pragma warning disable 1591

using System;

namespace Braintree
{
    public class AddOnsRequest : Request
    {
        public AddAddOnRequest[] Add { get; set; }
        public string[] Remove { get; set; }
        public UpdateAddOnRequest[] Update { get; set; }

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
