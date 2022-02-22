using System;
using System.Collections.Generic;

namespace Braintree
{
    public class EnrichedCustomerData
    {
        public virtual List<string> FieldsUpdated { get; protected set; }
        public virtual VenmoProfileData ProfileData { get; protected set; }

        protected internal EnrichedCustomerData(NodeWrapper node) {
            FieldsUpdated = new List<string>();

            foreach(var field in node.GetList("fields-updated/item"))
            {
                FieldsUpdated.Add(field.GetString("."));
            }

            ProfileData = new VenmoProfileData(node.GetNode("profile-data"));
        }
    }
}
