using System;

namespace Braintree
{
    public class UpdateModificationRequest : ModificationRequest
    {
        public String ExistingId { get; set; }

        public override String ToXml(String root)
        {
            return BuildRequest(root).ToXml();
        }

        protected override RequestBuilder BuildRequest(String root)
        {
            return base.BuildRequest(root).
                AddElement("existing-id", ExistingId);
        }
    }
}
