#pragma warning disable 1591

using System;

namespace Braintree
{
    [Serializable]
    public class AddModificationRequest : ModificationRequest
    {
        public String InheritedFromId { get; set; }

        protected override RequestBuilder BuildRequest(String root)
        {
            return base.BuildRequest(root).
                AddElement("inherited-from-id", InheritedFromId);
        }
    }
}
