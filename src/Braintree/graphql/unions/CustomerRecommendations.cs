using System.Collections.Generic;

namespace Braintree.GraphQL
{
    /// <summary>
    /// A union of all possible customer recommendations associated with a PayPal customer session.
    /// </summary>
    public class CustomerRecommendations
    {
        public virtual List<PaymentOptions> PaymentOptions { get; protected set; }

        public CustomerRecommendations(List<PaymentOptions> paymentOptions)
        {
            PaymentOptions = paymentOptions;
        }

        public CustomerRecommendations()
        {
            PaymentOptions = new List<PaymentOptions>();
        }
    }
}
