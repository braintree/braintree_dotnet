#pragma warning disable 1591

namespace Braintree
{
    public class AddModificationRequest : ModificationRequest
    {
        public string InheritedFromId { get; set; }

        protected override RequestBuilder BuildRequest(string root)
        {
            return base.BuildRequest(root).AddElement("inherited-from-id", InheritedFromId);
        }
    }
}
