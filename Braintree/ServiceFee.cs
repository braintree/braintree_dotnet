#pragma warning disable 1591

using System;

namespace Braintree
{
    public class ServiceFee
    {
        public String MerchantAccountId { get; protected set; }
        public Decimal? Amount { get; protected set; }

        protected internal ServiceFee(NodeWrapper node)
        {
            Amount = node.GetDecimal("amount");
            MerchantAccountId = node.GetString("merchant-account-id");
        }
    }
}
