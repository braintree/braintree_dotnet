#pragma warning disable 1591

using System;

namespace Braintree
{
    public class UpdateModificationRequest : ModificationRequest
    {
        public string ExistingId { get; set; }

        protected override RequestBuilder BuildRequest(string root)
        {
            return base.BuildRequest(root).
                AddElement("existing-id", ExistingId);
        }
    }
}
