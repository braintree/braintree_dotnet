using System;

namespace Braintree
{
    /// <summary>
    /// An Enum representing all of the processor response types from the
    /// gateway that are present on Transaction, AuthorizationAdjustment and
    /// CreditCardVerification.
    /// </summary>
    public class ProcessorResponseType : Enumeration
    {
        public static readonly ProcessorResponseType APPROVED = new ProcessorResponseType("approved");
        public static readonly ProcessorResponseType SOFT_DECLINED = new ProcessorResponseType("soft_declined");
        public static readonly ProcessorResponseType HARD_DECLINED = new ProcessorResponseType("hard_declined");
        public static readonly ProcessorResponseType UNRECOGNIZED = new ProcessorResponseType("unrecognized");

        public static readonly ProcessorResponseType[] ALL = {
            APPROVED, SOFT_DECLINED, HARD_DECLINED, UNRECOGNIZED
        };

        protected ProcessorResponseType(string name) : base(name) {}
    }
}

