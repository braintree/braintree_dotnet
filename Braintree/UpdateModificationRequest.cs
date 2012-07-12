#pragma warning disable 1591

using System;

namespace Braintree
{
    [Serializable]
    public class UpdateModificationRequest : ModificationRequest
    {
        public String ExistingId { get; set; }

        protected override RequestBuilder BuildRequest(String root)
        {
            return base.BuildRequest(root).
                AddElement("existing-id", ExistingId);
        }
    }
}
