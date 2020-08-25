#pragma warning disable 1591

using System;

namespace Braintree
{
    public class AuthorizationAdjustment
    {
        public virtual decimal? Amount { get; protected set; }
        public virtual bool? Success { get; protected set; }
        public virtual DateTime? Timestamp { get; protected set; }
        public virtual string ProcessorResponseCode { get; protected set; }
        public virtual string ProcessorResponseText { get; protected set; }
        public virtual ProcessorResponseType ProcessorResponseType { get; protected set; }

        public AuthorizationAdjustment(NodeWrapper node)
        {
            if (node == null)
                return;

            Amount = node.GetDecimal("amount");
            Success = node.GetBoolean("success");
            Timestamp = node.GetDateTime("timestamp");
            ProcessorResponseCode = node.GetString("processor-response-code");
            ProcessorResponseText = node.GetString("processor-response-text");
            ProcessorResponseType = node.GetEnum("processor-response-type", ProcessorResponseType.UNRECOGNIZED);
        }

        [Obsolete("Mock Use Only")]
        protected internal AuthorizationAdjustment() { }
    }
}
