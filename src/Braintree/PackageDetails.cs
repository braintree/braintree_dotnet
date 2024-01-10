using System;
using System.ComponentModel;

namespace Braintree
{
    public class PackageDetails
    {
        public virtual string Carrier { get; protected set; }
        public virtual string Id { get; protected set; }
        public virtual string PaypalTrackingId { get; protected set; }
        public virtual string TrackingNumber { get; protected set; }

        protected internal PackageDetails(NodeWrapper node) 
        {
            Carrier = node.GetString("carrier");
            Id = node.GetString("id");
            PaypalTrackingId = node.GetString("paypal-tracking-id");
            TrackingNumber = node.GetString("tracking-number");
        }

        [Obsolete("Mock Use Only")]
        protected internal PackageDetails() { }
    }
}