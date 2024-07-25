using System;
using System.Collections.Generic;

namespace Braintree
{
    public class ThreeDSecureLookupPriorAuthenticationDetails
    {
        public string AcsTransactionId { get; set; }
        public string AuthenticationMethod { get; set; }
        public DateTime AuthenticationTime { get; set; }
        public string DsTransactionId { get; set; }

        public Dictionary<string, object> ToDictionary() {
            var details = new Dictionary<string, object>();

            details.Add("acs_transaction_id", AcsTransactionId);
            details.Add("authentication_method", AuthenticationMethod);
            details.Add("authentication_time", AuthenticationTime);
            details.Add("ds_transaction_id", DsTransactionId);

            return details;
        }

    }
}
