using System;
using System.ComponentModel;

namespace Braintree
{
    public class PackageDetails
    {
        public virtual string Carrier { get; protected set; }
        public virtual string Id { get; protected set; }
        // NEXT_MAJOR_VERSION Remove PaypalTrackingId
        [ObsoleteAttribute("the attribute PaypalTrackingId has been deprecated, use PaypalTrackerId going forward.", false)]
        public virtual string PaypalTrackingId { get; protected set; }
        public virtual string PaypalTrackerId { get; protected set; }
        public virtual string TrackingNumber { get; protected set; }

        protected internal PackageDetails(NodeWrapper node) 
        {
            Carrier = node.GetString("carrier");
            Id = node.GetString("id");
            PaypalTrackerId = node.GetString("paypal-tracker-id");
            // NEXT_MAJOR_VERSION Remove this pragma warning when we remove PaypalTrackingId
            #pragma warning disable 618
            PaypalTrackingId = node.GetString("paypal-tracking-id");
            #pragma warning restore 618
            TrackingNumber = node.GetString("tracking-number");
        }

        [Obsolete("Mock Use Only")]
        protected internal PackageDetails() { }
    }
}