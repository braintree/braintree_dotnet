#pragma warning disable 1591

using System;

namespace Braintree
{
    public class MerchantAccountFundingDetails
    {
        public virtual FundingDestination Destination { get; protected set; }
        public virtual string RoutingNumber { get; protected set; }
        public virtual string AccountNumberLast4 { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual string MobilePhone { get; protected set; }
        public virtual string Descriptor { get; protected set; }

        protected internal MerchantAccountFundingDetails(NodeWrapper node)
        {
            Destination = node.GetEnum("destination", FundingDestination.UNRECOGNIZED);
            RoutingNumber = node.GetString("routing-number");
            AccountNumberLast4 = node.GetString("account-number-last-4");
            Email = node.GetString("email");
            MobilePhone = node.GetString("mobile-phone");
            Descriptor = node.GetString("descriptor");
        }

        [Obsolete("Mock Use Only")]
        protected internal MerchantAccountFundingDetails() { }
    }
}
