using System;
using System.Collections.Generic;
using System.Linq;

namespace Braintree.GraphQL
{
    /// <remarks>
    /// <b>Experimental:</b> This class is experimental and may change in future releases.
    /// </remarks>
    /// <summary>
    /// A union of all possible customer recommendations associated with a PayPal customer session.
    /// </summary>
    public class CustomerRecommendations
    {
        // [Obsolete("Use PaymentRecommendations instead.", false)]
        public virtual List<PaymentOptions> PaymentOptions { get; protected set; }

        public virtual List<PaymentRecommendation> PaymentRecommendations { get; protected set; }

        public CustomerRecommendations(
            List<PaymentRecommendation> paymentRecommendations
        )
        {
            
            PaymentRecommendations = paymentRecommendations ?? new List<PaymentRecommendation>();

            PaymentOptions = paymentRecommendations.Select(
                paymentRecommendation => new PaymentOptions(
                    paymentRecommendation.RecommendedPriority,
                    paymentRecommendation.PaymentOption
                )
            ).ToList();
        }

        public CustomerRecommendations()
        {
            PaymentRecommendations = new List<PaymentRecommendation>();
            PaymentOptions = new List<PaymentOptions>();
        }
    }
}
