using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Braintree
{
    public class ThreeDSecureLookupResponse
    {
        public virtual ThreeDSecureLookup Lookup { get; protected set; }
        public virtual PaymentMethodNonce PaymentMethod { get; protected set; }
        public virtual string PayloadString { get; protected set; }
        public virtual dynamic Error { get; protected set; }

        public ThreeDSecureLookupResponse(string lookupResponse)
        {
            var result = JsonConvert.DeserializeObject<dynamic>(lookupResponse);

            PayloadString = lookupResponse;

            if (result.error != null) {
                Error = result.error;
            } else {
                Lookup = new ThreeDSecureLookup(result.lookup);
                PaymentMethod = new PaymentMethodNonce(result.paymentMethod);
            }
        }

        [Obsolete("Mock Use Only")]
        protected internal ThreeDSecureLookupResponse() { }
    }
}

