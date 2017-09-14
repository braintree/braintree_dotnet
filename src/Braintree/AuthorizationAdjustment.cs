#pragma warning disable 1591

using System;

namespace Braintree
{
    public class AuthorizationAdjustment
    {
        public virtual decimal? Amount { get; protected set; }
        public virtual bool? Success { get; protected set; }
        public virtual DateTime? Timestamp { get; protected set; }

        public AuthorizationAdjustment(NodeWrapper node)
        {
            if (node == null)
                return;

            Amount = node.GetDecimal("amount");
            Success = node.GetBoolean("success");
            Timestamp = node.GetDateTime("timestamp");
        }

        [Obsolete("Mock Use Only")]
        protected internal AuthorizationAdjustment() { }
    }
}
