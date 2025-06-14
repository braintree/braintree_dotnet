using System;
using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <summary>
    /// Represents the payment method and priority associated with a PayPal customer session.
    /// </summary>
    // [Obsolete("Use PaymentRecommendations instead.", false)]
    public class PaymentOptions
    {
        public virtual RecommendedPaymentOption PaymentOption { get; protected set; }
        public virtual int RecommendedPriority { get; protected set; }

        public PaymentOptions(int recommendedPriority, RecommendedPaymentOption paymentOption)
        {
            PaymentOption = paymentOption;
            RecommendedPriority = recommendedPriority;
        }
    }
}
