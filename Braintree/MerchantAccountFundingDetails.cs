#pragma warning disable 1591

using System;

namespace Braintree
{
    public class MerchantAccountFundingDetails
    {
        public FundingDestination Destination { get; protected set; }
        public string RoutingNumber { get; protected set; }
        public string AccountNumberLast4 { get; protected set; }
        public string Email { get; protected set; }
        public string MobilePhone { get; protected set; }
        public string Descriptor { get; protected set; }

        protected internal MerchantAccountFundingDetails(NodeWrapper node)
        {
            Destination = (FundingDestination)CollectionUtil.Find(
                FundingDestination.ALL, 
                node.GetString("destination"), 
                FundingDestination.UNRECOGNIZED);
            RoutingNumber = node.GetString("routing-number");
            AccountNumberLast4 = node.GetString("account-number-last-4");
            Email = node.GetString("email");
            MobilePhone = node.GetString("mobile-phone");
            Descriptor = node.GetString("descriptor");
        }
    }
}
