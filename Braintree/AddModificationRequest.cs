using System;

namespace Braintree
{
    public class AddModificationRequest : ModificationRequest
    {
        public String InheritedFromId { get; set; }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected virtual RequestBuilder BuildRequest(String root)
        {
            return base.BuildRequest(root).
                AddElement("inherited-from-id", InheritedFromId);
        }
    }
}
