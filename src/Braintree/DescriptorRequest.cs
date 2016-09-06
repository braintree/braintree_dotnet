#pragma warning disable 1591

namespace Braintree
{
    public class DescriptorRequest : Request
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Url { get; set; }

        public override string ToXml()
        {
            return ToXml("descriptor");
        }

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
            return new RequestBuilder(root).
                AddElement("name", Name).
                AddElement("phone", Phone).
                AddElement("url", Url);
        }
    }
}
