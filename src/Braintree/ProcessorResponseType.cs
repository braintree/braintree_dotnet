using System.ComponentModel;

namespace Braintree
{
    /// <summary>
    /// An Enum representing all of the processor response types from the
    /// gateway that are present on Transaction, AuthorizationAdjustment and
    /// CreditCardVerification.
    /// </summary>
    public enum ProcessorResponseType
    {
        [Description("approved")] APPROVED,
        [Description("soft_declined")] SOFT_DECLINED,
        [Description("hard_declined")] HARD_DECLINED,
        [Description("unrecognized")] UNRECOGNIZED
    }
}
