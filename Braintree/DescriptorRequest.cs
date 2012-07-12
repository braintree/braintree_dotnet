#pragma warning disable 1591

using System;

namespace Braintree
{
    [Serializable]
    public class DescriptorRequest : Request
    {
        public String Name { get; set; }
        public String Phone { get; set; }

        public override String ToXml()
        {
            return ToXml("descriptor");
        }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        public override String ToQueryString(String root)
        {
            return BuildRequest(root).ToQueryString();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return new RequestBuilder(root).
                AddElement("name", Name).
                AddElement("phone", Phone);
        }
    }
}
