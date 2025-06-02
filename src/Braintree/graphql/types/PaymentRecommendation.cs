using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// Represents a single  payment method and priority associated with a PayPal customer session.
    /// </summary>
    public class PaymentRecommendation
    {
        public virtual RecommendedPaymentOption PaymentOption { get; protected set; }
        public virtual int RecommendedPriority { get; protected set; }

        public PaymentRecommendation(RecommendedPaymentOption paymentOption, int recommendedPriority)
        {
            PaymentOption = paymentOption;
            RecommendedPriority = recommendedPriority;
        }
    }
}
